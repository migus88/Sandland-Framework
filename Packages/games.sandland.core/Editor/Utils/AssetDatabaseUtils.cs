using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandland.Core.Editor.Data;
using Sandland.SceneTool.Editor.Common.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Sandland.Core.Editor.Utils
{
    public static class AssetDatabaseUtils
    {
        public static VisualTreeAsset FindAndLoadVisualTreeAsset(string name = null) =>
            FindAndLoadAsset<VisualTreeAsset>(name);

        public static StyleSheet FindAndLoadStyleSheet(string name = null) => FindAndLoadAsset<StyleSheet>(name);
        
        public static string GetRelativePath(this string fullPath) => fullPath.StartsWith(Application.dataPath)
            ? $"Assets{fullPath.Substring(Application.dataPath.Length)}"
            : string.Empty;

        public static bool TryFindAndLoadAsset<T>(out T result, string name = null) where T : Object
        {
            try
            {
                result = FindAndLoadAsset<T>(name);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static T FindAndLoadAsset<T>(string name = null) where T : Object
        {
            // TODO: Reuse code from FindAssets
            var typeName = typeof(T).Name;
            var query = string.IsNullOrEmpty(name) ? $"t:{typeName}" : $"{name} t:{typeName}";
            var guids = AssetDatabase.FindAssets(query);

            switch (guids.Length)
            {
                case 0:
                    throw new FileNotFoundException($"Cant locate {typeName} file with the name: {name}");
                case > 1:
                    Debug.LogWarning(
                        $"Found more than one {typeName} file with the name: {name}; Loading only the first");
                    break;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids.First());

            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null)
            {
                throw new FileNotFoundException($"Unable to load the {typeName} with the name {name}");
            }

            return asset;
        }

        public static bool TryFindAssets<T>(out AssetFileInfo[] result, string name = null)
        {
            try
            {
                result = FindAssets<T>(name);
                return result.Length > 0;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static SceneFileInfo[] FindScenes(string name = null)
        {
            var assets = FindAssets<Scene>(name);

            var result = new List<SceneFileInfo>(assets.Length);
            var sceneBuildIndexes = SceneUtils.GetSceneBuildIndexes();
            var assetsInBundles = GetAssetsInBundles();

            const string packagesPrefix = "Packages/";

            foreach (var asset in assets)
            {
                if (asset.Path.StartsWith(packagesPrefix))
                {
                    continue;
                }
                
                SceneFileInfo info;
                
                if (IsAssetAddressable(asset.Guid, out var address))
                {
                    info = SceneFileInfo.Create.Addressable(address, asset.Name, asset.Path, asset.Guid, asset.Labels);
                }
                else if (IsAssetInBundle(assetsInBundles, asset.Path, out var bundleName))
                {
                    info = SceneFileInfo.Create.AssetBundle(asset.Name, asset.Path, asset.Guid, bundleName, asset.Labels);
                }
                else if (sceneBuildIndexes.ContainsSceneGuid(asset.Guid, out var buildIndex))
                {
                    info = SceneFileInfo.Create.BuiltIn(asset.Name, buildIndex, asset.Path, asset.Guid, asset.Labels);
                }
                else
                {
                    info = SceneFileInfo.Create.Default(asset.Name, asset.Path, asset.Guid, asset.Labels);
                }
                
                result.Add(info);
            }

            return result.ToArray();
        }

        public static AssetFileInfo[] FindAssets<T>(string name = null)
        {
            var typeName = typeof(T).Name;
            var query = string.IsNullOrEmpty(name) ? $"t:{typeName}" : $"{name} t:{typeName}";
            var guids = AssetDatabase.FindAssets(query);

            if (guids.Length == 0)
            {
                return Array.Empty<AssetFileInfo>();
            }

            var result = new AssetFileInfo[guids.Length];

            for (var i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var assetName = Path.GetFileNameWithoutExtension(path);
                var labels = AssetDatabase.GetLabels(new GUID(guid)).ToList();
                result[i] = new AssetFileInfo(assetName, path, guid, string.Empty, labels);
            }

            return result;
        }

        public static void SetLabels<T>(this AssetFileInfo info) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(info.Path);
            AssetDatabase.SetLabels(asset, info.Labels.ToArray());
        }

        public static void SetLabel<T>(string path, string label) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            var labels = AssetDatabase.GetLabels(asset).ToList();

            if (labels.Contains(label))
            {
                return;
            }

            labels.Add(label);
            AssetDatabase.SetLabels(asset, labels.ToArray());
        }
        
        public static bool IsAssetAddressable(string guid, out string address)
        {
            address = string.Empty;

#if SANDLAND_ADDRESSABLES
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings == null)
            {
                Debug.LogError("Addressable Asset Settings not found.");
                return false;
            }

            if (!string.IsNullOrEmpty(guid))
            {
                var entry = settings.FindAssetEntry(guid);
                address = entry != null ? entry.address : address;
                return entry != null;
            }

            Debug.LogError($"No valid asset path found: {guid}");
            return false;
#else
            return false;
#endif
        }
        
        public static bool IsAssetInBundle(Dictionary<string, string> assetsInBundles, string assetPath, out string bundleName) 
            => assetsInBundles.TryGetValue(assetPath, out bundleName);
        
        public static Dictionary<string, string> GetAssetsInBundles() =>
            AssetDatabase
                .GetAllAssetBundleNames()
                .SelectMany(AssetDatabase.GetAssetPathsFromAssetBundle, (bundleName, path) => new { bundleName, path })
                .ToDictionary(x => x.path, x => x.bundleName);
    }
}