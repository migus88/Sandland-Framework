using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

#if SANDLAND_ADDRESSABLES
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace Sandland.SceneTool.Editor.Common.Utils
{
    public static class Utils
    {
        public static bool IsAddressablesInstalled =>
            PackageInfo.FindForAssetPath("Packages/com.unity.addressables/AddressableAssets")?.name ==
            "com.unity.addressables";
    }
}