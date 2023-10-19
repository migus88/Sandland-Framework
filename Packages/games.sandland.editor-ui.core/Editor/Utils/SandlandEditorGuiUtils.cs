using System;
using Sandland.Core.Editor.Utils;
using Sandland.EditorUI.Core.Editor.Services;
using UnityEngine.UIElements;

namespace Sandland.EditorUI.Core.Editor.Utils
{
    public static class SandlandEditorGuiUtils
    {
        public static void InitElement(this VisualElement element, string packageName, string visualTreeName, string styleSheetName = null, string globalStyleSheetName = null, StyleSheet theme = null)
        {
            if (element == null || string.IsNullOrEmpty(visualTreeName))
            {
                throw new Exception("Visual Tree cannot be null or empty");
            }
            
            var visualTree = AssetDatabaseUtils.FindAndLoadVisualTreeAsset(visualTreeName, packageName);
            visualTree.CloneTree(element);
            
            if(theme != null)
            {
                element.styleSheets.Add(theme);
            }
            
            if(!string.IsNullOrEmpty(globalStyleSheetName))
            {
                var globalStyleSheet = AssetDatabaseUtils.FindAndLoadStyleSheet(globalStyleSheetName, CurrentPackageInfo.PackageName);
                element.styleSheets.Add(globalStyleSheet);
            }
            
            if(!string.IsNullOrEmpty(styleSheetName))
            {
                var styleSheet = AssetDatabaseUtils.FindAndLoadStyleSheet(styleSheetName, packageName);
                element.styleSheets.Add(styleSheet);
            }
        }
    }
}