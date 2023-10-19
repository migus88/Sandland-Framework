using System;
using System.Collections.Generic;
using System.Linq;
using Sandland.EditorUI.Core.Editor.Utils;
using Sandland.EditorUI.Core.Editor.Views.Base;
using UnityEngine.UIElements;

namespace Sandland.EditorUI.Core.Editor.Views
{
    public class TabsView : SandlandView
    {
        private const string RootElementId = "sandland--tabsview-root";

        public override string PackageName => CurrentPackageInfo.PackageName;
        public override string VisualTreeName => nameof(TabsView);
        public override string StyleSheetName => nameof(TabsView);

        private VisualElement _tabLabelsContainer;
        private VisualElement _tabContentContainer;

        protected override void InitView()
        {
            _tabLabelsContainer = this.Q<TabsViewLabels>();
            _tabContentContainer = this.Q<TabsViewContents>();
            
            var root = this.Q<VisualElement>(RootElementId);
            root.Add(_tabLabelsContainer);
            root.Add(_tabContentContainer);

            var tabLabels = _tabLabelsContainer.Children().Cast<TabsViewLabel>().ToArray();
            var tabContents = _tabContentContainer.Children().ToArray();

            if (tabLabels.Length != tabContents.Length)
            {
                throw new Exception("Inconsistent amount of labels and contents");
            }
            
            foreach (var tabLabel in tabLabels)
            {
                var label = tabLabel ?? throw new Exception($"{nameof(TabsViewLabels)} can contain only {nameof(TabsViewLabel)} as direct children");

                if (string.IsNullOrEmpty(label.ContentId))
                {
                    throw new Exception($"{nameof(TabsViewLabel.ContentId)} cannot be empty");
                }
                
                var content = _tabContentContainer.Q<VisualElement>(label.ContentId);

                if (content == null)
                {
                    throw new Exception($"Content with ID '{label.ContentId}' not found.");
                }
                
                tabLabel.AddManipulator(new Clickable(OnTabLabelClicked));
            }

            var firstTab = tabLabels[0];
            SetTabActive(firstTab);
        }

        private void OnTabLabelClicked(EventBase obj)
        {
            if (obj.currentTarget is not TabsViewLabel label)
            {
                throw new Exception($"Current target is not a {nameof(TabsViewLabel)}");
            }
            
            SetTabActive(label);
        }

        private void SetTabActive(TabsViewLabel label)
        {
            var tabLabels = _tabLabelsContainer.Children();
            var tabContents = _tabContentContainer.Children();

            foreach (var tabLabel in tabLabels)
            {
                tabLabel.RemoveFromClassList(UssClasses.Selected);
            }
            label.AddToClassList(UssClasses.Selected);

            foreach (var content in tabContents)
            {
                content.RemoveFromClassList(UssClasses.Invisible);
                
                if(content.name != label.ContentId)
                {
                    content.AddToClassList(UssClasses.Invisible);
                }
            }
        }
        
        public new class UxmlFactory : UxmlFactory<TabsView, UxmlTraits> { }
    }
    
    public class TabsViewLabels : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TabsViewLabels, UxmlTraits> { }
    }

    public class TabsViewContents : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TabsViewContents, UxmlTraits> { }
    }

    public class TabsViewLabel : VisualElement
    {
        public string ContentId { get; set; }
        
        public new class UxmlFactory : UxmlFactory<TabsViewLabel, UxmlTraits> { }
        
        public new class UxmlTraits : UnityEngine.UIElements.UxmlTraits
        {
            private UxmlStringAttributeDescription _binding = new() { name = "content-id" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((TabsViewLabel)ve).ContentId = _binding.GetValueFromBag(bag, cc);
            }
        }
    }

    public class TabsViewContent : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TabsViewContent, UxmlTraits> { }
    }
}