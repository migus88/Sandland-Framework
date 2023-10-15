using System.ComponentModel;

namespace Sandland.Core.Editor.Enums
{
    public enum SceneImportType
    {
        [Description("Not Used")]
        None = 0,
        [Description("In Build Settings")]
        BuildSettings = 1,
        [Description("In Addressable Group")]
        Addressables = 2,
        [Description("In AssetBundle")]
        AssetBundle = 3
    }
}