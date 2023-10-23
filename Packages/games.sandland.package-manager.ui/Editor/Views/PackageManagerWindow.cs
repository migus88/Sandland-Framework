using System;
using Sandland.Core.Shared.Utils;
using Sandland.EditorUI.Core.Editor.Views.Base;
using Sandland.PackageManager.Editor.Logic;
using UnityEditor;
using UnityEngine.UIElements;

namespace Sandland.PackageManager.UI.Editor.Views
{
    public class PackageManagerWindow : SandlandEditorWindow, IDisposable
    {
        public override string PackageName => CurrentPackageInfo.PackageName;
        public override float MinWidth => 600f;
        public override float MinHeight => 600f;
        public override string WindowName => "[Sandland] Packages";
        public override string VisualTreeName => nameof(PackageManagerWindow);
        public override string StyleSheetName => nameof(PackageManagerWindow);

        [MenuItem(MenuItems.Management + "Packages")]
        public static void ShowWindow() => ShowWindow<PackageManagerWindow>();

        private NewPackageTabController _newPackageTabController;

        private readonly IPackagesService _packagesService = new PackagesService();
        
        protected override void InitGui()
        {
            var newPackageContent = rootVisualElement.Q("new-package-creation");
            _newPackageTabController = new NewPackageTabController(newPackageContent, _packagesService);
        }

        public void Dispose()
        {
            _newPackageTabController?.Dispose();
        }
    }
}