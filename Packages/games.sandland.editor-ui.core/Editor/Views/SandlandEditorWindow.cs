using Sandland.Core.Editor.Utils;
using Sandland.EditorUI.Core.Editor;
using Sandland.EditorUI.Core.Editor.Services;
using Sandland.EditorUI.Core.Editor.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Views
{
    public abstract class SandlandEditorWindow : EditorWindow
    {
        public static readonly string GlobalStyleSheetName = "sandland-main-style";
        
        public abstract string PackageName { get; }
        public abstract float MinWidth { get; }
        public abstract float MinHeight { get; }
        public abstract string WindowName { get; }
        public abstract string VisualTreeName { get; }
        public abstract string StyleSheetName { get; }

        private StyleSheet _theme;
        
        public static void ShowWindow<TWindow>(Texture2D overrideIcon = null) where TWindow : SandlandEditorWindow
        {
            var window = EditorWindow.GetWindow<TWindow>();
            window.InitWindow(overrideIcon);
        }
        
        protected virtual void OnEnable()
        {
            ThemesService.ThemeChanged += RefreshTheme;
        }

        protected virtual void OnDisable()
        {
            ThemesService.ThemeChanged -= RefreshTheme;
        }

        protected virtual void InitWindow(Texture2D overrideIcon = null)
        {
            minSize = new Vector2(MinWidth, MinHeight);
            titleContent = new GUIContent(WindowName, overrideIcon ? overrideIcon : EditorIcons.SandlandLogoIcon);

            if (docked)
            {
                return;
            }

            var editorPos = EditorGUIUtility.GetMainWindowPosition();
            var x = editorPos.x + editorPos.width * 0.5f - MinWidth * 0.5f;
            var y = editorPos.y + editorPos.height * 0.5f - MinHeight * 0.5f;

            position = new Rect(x, y, MinWidth, MinHeight);
        }
        
        public virtual void CreateGUI()
        {
            var visualTree = AssetDatabaseUtils.FindAndLoadVisualTreeAsset(VisualTreeName, PackageName);
            visualTree.CloneTree(rootVisualElement);

            _theme = ThemesService.GetSelectedTheme();
            
            var globalStyleSheet = AssetDatabaseUtils.FindAndLoadStyleSheet(GlobalStyleSheetName, CurrentPackageInfo.PackageName);
            var styleSheet = AssetDatabaseUtils.FindAndLoadStyleSheet(StyleSheetName, PackageName);
            
            rootVisualElement.styleSheets.Add(_theme);
            rootVisualElement.styleSheets.Add(globalStyleSheet);
            rootVisualElement.styleSheets.Add(styleSheet);
            InitGui();
        }

        protected void RefreshTheme(StyleSheet theme)
        {
            if (_theme == theme)
            {
                return;
            }
            
            if(_theme != null)
            {
                rootVisualElement.styleSheets.Remove(_theme);
            }

            _theme = theme;
            rootVisualElement.styleSheets.Add(_theme);
        }
        
        protected abstract void InitGui();
    }
}