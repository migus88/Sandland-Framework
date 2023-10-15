using System;
using System.Linq;
using System.Reflection;
using Sandland.Core.Editor.Utils;
using Sandland.Core.Runtime.Utils;
using Sandland.Core.Settings;
using Sandland.Domains.Core.Data;
using UnityEditor;
using UnityEngine;

namespace Sandland.Domains.Core.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(GameDomain))]
    public class GameDomainPropertyDrawer : PropertyDrawer
    {
        private const string Domain = "Domain";

        private static ProjectScenes _scenes;
    
        private string[] _options;
        private string[] _displayOptions;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                _scenes ??= AssetDatabaseUtils.FindAndLoadAsset<ProjectScenes>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception wile trying to load Project Scenes: \r\n{e}");
                return;
            }
            
            if (_options == null)
            {
                _options = _scenes.Addressables;

                _displayOptions = _options
                    .Select(o => o.Replace(Domain, string.Empty).ToWords())
                    .ToArray();
            }

            // Fetching the current value
            var currentValue = property.stringValue;
            var index = Array.IndexOf(_options, currentValue);

            if (index < 0)
            {
                index = 0;
            }

            // Drawing the popup
            index = EditorGUI.Popup(position, Domain, index, _displayOptions);

            // Storing the selected option
            property.stringValue = _options[index];
        }
    }
}