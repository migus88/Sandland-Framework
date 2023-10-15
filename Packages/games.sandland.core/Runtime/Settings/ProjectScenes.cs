using Sandland.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Sandland.Core.Settings
{
    [CreateAssetMenu(fileName = nameof(ProjectScenes), menuName = MenuItems.Settings + nameof(ProjectScenes), order = 0)]
    public class ProjectScenes : ScriptableObject
    {
        [field: SerializeField] public string[] Names { get; set; }
        [field:SerializeField] public int[] Indexes { get; set; }
        [field:SerializeField] public string[] Addressables { get; set; }
    }
}