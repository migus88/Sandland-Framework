using System.Collections.Generic;
using Sandland.Core.Editor.Enums;

namespace Sandland.Core.Editor.Data
{
    public class SceneFileInfo : AssetFileInfo
    {
        public int BuildIndex { get; set; }
        public string Address { get; set; }
        public SceneImportType ImportType { get; set; }

        private SceneFileInfo(string name, string path, string guid, string bundleName, List<string> labels) : base(name, path, guid, bundleName, labels)
        {
        }

        public static class Create
        {
            public static SceneFileInfo Addressable(string address, string name = null, string path = null, string guid = null, List<string> labels = null)
            {
                return new SceneFileInfo(name, path, guid, string.Empty, labels)
                {
                    ImportType = SceneImportType.Addressables,
                    Address = address
                };
            }
            
            public static SceneFileInfo AssetBundle(string name, string path, string guid, string bundleName, List<string> labels = null)
            {
                return new SceneFileInfo(name, path, guid, bundleName, labels)
                {
                    ImportType = SceneImportType.AssetBundle
                };
            }
            
            public static SceneFileInfo Default(string name, string path, string guid, List<string> labels = null)
            {
                return new SceneFileInfo(name, path, guid, string.Empty, labels)
                {
                    ImportType = SceneImportType.None
                };
            }
            
            public static SceneFileInfo BuiltIn(string name, int buildIndex, string path, string guid, List<string> labels = null)
            {
                return new SceneFileInfo(name, path, guid, string.Empty, labels)
                {
                    ImportType = SceneImportType.BuildSettings,
                    BuildIndex = buildIndex
                };
            }
        }
    }
}