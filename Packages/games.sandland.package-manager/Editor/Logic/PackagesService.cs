using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandland.Domains.Core.Shared.Utils;
using Sandland.PackageManager.Editor.Data;
using Sandland.PackageManager.Editor.Enums;
using UnityEditor;
using UnityEditor.PackageManager;

namespace Sandland.PackageManager.Editor.Logic
{
    internal class PackagesService
    {
        [MenuItem("Tools/Create Test Package")]
        public static void TestPackageCreation() =>
            new PackagesService().CreateNewPackage("games.sandland.editor-ui.core", "[Sandland] Editor UI Core", "Core functionality for Sandland Editor UI", "Sandland.EditorUI.Core");
        
        public void CreateNewPackage(string bundleId, string displayName, string description, string rootNamespace = null)
        {
            if (bundleId == null || bundleId.Contains(' ') || bundleId.Last() == '.')
            {
                throw new Exception("Incorrect Bundle ID format");
            }
            
            var creationResult = TryCreatePackageFolderStructure(bundleId, out var folderStructure);

            if (!creationResult.IsCreated)
            {
                throw new Exception(creationResult.Error);
            }

            CreatePackageDefinitionFile(bundleId, displayName, description, folderStructure);

            rootNamespace ??= bundleId;
            
            CreateAssemblyDefinitions(folderStructure, rootNamespace);
            CreatePackageInfoClass(folderStructure, rootNamespace, bundleId);
            
            AssetDatabase.ImportAsset(folderStructure.Root);

            // Refresh Unity Asset Database
            AssetDatabase.Refresh();
            Client.Resolve();
        }

        public static void CreatePackageInfoClass(PackageFolderStructureModel folderStructure, string rootNamespace, string packageBundleId)
        {
            const string filename = "CurrentPackageInfo.cs";
            
            // We do it for each assembly, because the class should remain internal in order to prevent usage of a class from another package
            var paths = new Dictionary<CodeAssembly, string>
            {
                [CodeAssembly.Shared] = Path.Combine(folderStructure.SharedCodePath, filename),
                [CodeAssembly.Runtime] = Path.Combine(folderStructure.RuntimeCodePath, filename),
                [CodeAssembly.Editor] = Path.Combine(folderStructure.EditorCodePath, filename),
                [CodeAssembly.Tests] = Path.Combine(folderStructure.TestsCodePath, filename)
            };

            foreach (var path in paths)
            {            
                var content = $@"namespace {rootNamespace}.{path.Key}
{{
    internal static class CurrentPackageInfo
    {{
        public static readonly string PackageName = ""{packageBundleId}"";
    }}
}}";
                File.WriteAllText(path.Value, content);
            }
        }

        private static void CreateAssemblyDefinitions(PackageFolderStructureModel folderStructure, string rootNamespace)
        {
            var runtime = GetAssemblyDefinition(rootNamespace, CodeAssembly.Runtime, new[] { CodeAssembly.Shared });
            CreateAssemblyDefinitionFile(folderStructure.RuntimeCodePath, rootNamespace, CodeAssembly.Runtime, runtime);
            
            var editor = GetAssemblyDefinition(rootNamespace, CodeAssembly.Editor, new[] { CodeAssembly.Shared }, true);
            CreateAssemblyDefinitionFile(folderStructure.EditorCodePath, rootNamespace, CodeAssembly.Editor, editor);
            
            var shared = GetAssemblyDefinition(rootNamespace, CodeAssembly.Shared);
            CreateAssemblyDefinitionFile(folderStructure.SharedCodePath, rootNamespace, CodeAssembly.Shared, shared);
            
            var tests = GetAssemblyDefinition(rootNamespace, CodeAssembly.Tests, new[] { CodeAssembly.Shared, CodeAssembly.Editor, CodeAssembly.Runtime }, true);
            CreateAssemblyDefinitionFile(folderStructure.TestsCodePath, rootNamespace, CodeAssembly.Tests, tests);
            
        }

        private static void CreateAssemblyDefinitionFile(string directory, string rootNamespace, CodeAssembly assemblyType, AssemblyDefinitionModel assemblyDefinition)
        {
            var path = Path.Combine(directory, $"{rootNamespace}.{assemblyType}.asmdef");
            File.WriteAllText(path, assemblyDefinition.ToJson());
        }

        private static AssemblyDefinitionModel GetAssemblyDefinition(string rootNamespace, CodeAssembly assemblyType, CodeAssembly[] dependencies = null, bool isEditor = false)
        {
            return new AssemblyDefinitionModel
            {
                Name = $"{rootNamespace}.{assemblyType}",
                RootNamespace = rootNamespace,
                References = dependencies?.Select(assembly => $"{rootNamespace}.{assembly}").ToArray() ?? Array.Empty<string>(),
                IncludedPlatforms = isEditor ? new []{ CodeAssembly.Editor.ToString() } : Array.Empty<string>()
            };
        }

        private static void CreatePackageDefinitionFile(string bundleId, string name, string description,
            PackageFolderStructureModel folderStructure)
        {
            var unityVersion = EditorUtils.GetCurrentUnityVersion();

            var package = new PackageModel
            {
                Name = bundleId,
                DisplayName = name,
                Version = "0.0.1",
                MinUnityVersion = $"{unityVersion.Year}.{unityVersion.Major}",
                Description = description
            };

            File.WriteAllText(Path.Combine(folderStructure.Root, "package.json"), package.ToJson());
        }

        private (bool IsCreated, string Error) TryCreatePackageFolderStructure(string bundleId, out PackageFolderStructureModel folderStructure)
        {
            folderStructure = new PackageFolderStructureModel
            {
                Root = Path.Combine("Packages", bundleId)
            };

            if (Directory.Exists(folderStructure.Root))
            {
                return (false, "Package already exists");
            }
            
            folderStructure.AssetsPath = Path.Combine(folderStructure.Root, "Assets");
            folderStructure.EditorCodePath = Path.Combine(folderStructure.Root, "Editor");
            folderStructure.TestsCodePath = Path.Combine(folderStructure.Root, "Tests");
            folderStructure.RuntimeCodePath = Path.Combine(folderStructure.Root, "Runtime");
            folderStructure.SharedCodePath = Path.Combine(folderStructure.Root, "Shared");
            
            Directory.CreateDirectory(folderStructure.AssetsPath);
            Directory.CreateDirectory(folderStructure.EditorCodePath);
            Directory.CreateDirectory(folderStructure.TestsCodePath);
            Directory.CreateDirectory(folderStructure.RuntimeCodePath);
            Directory.CreateDirectory(folderStructure.SharedCodePath);

            return (true, string.Empty);
        }
    }
}