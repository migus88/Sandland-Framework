using Sandland.Core.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Sandland.EditorUI.Core.Editor.Utils
{
    public static class EditorIcons
    {
        public static string ThemePostfix => EditorGUIUtility.isProSkin ? "dark" : "light";

        public static Texture2D SandlandLogoIcon => _sandlandLogoIcon ??=
            AssetDatabaseUtils.FindAndLoadAsset<Texture2D>($"sandland_logo-{ThemePostfix}", CurrentPackageInfo.PackageName);

        private static Texture2D _sandlandLogoIcon;
    }
}