using Sandland.EditorUI.Core.Editor.Utils;
using UnityEngine.UIElements;

namespace Sandland.EditorUI.Core.Editor.Views.Base
{
    public abstract class SandlandView : VisualElement
    {
        public abstract string PackageName { get; }
        public abstract string VisualTreeName { get; }
        public abstract string StyleSheetName { get; }
        

        public SandlandView() => Initialize();

        private void Initialize()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            this.InitElement(PackageName, VisualTreeName, StyleSheetName);
            Init();
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            InitView();
        }

        protected virtual void Init() { }
        protected virtual void InitView() { }
    }
}