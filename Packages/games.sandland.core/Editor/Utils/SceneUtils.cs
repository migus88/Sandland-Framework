using System.Collections.Generic;
using UnityEditor;

namespace Sandland.Core.Editor.Utils
{
    public static class SceneUtils
    {
        public static Dictionary<GUID, int> GetSceneBuildIndexes()
        {
            var collection = new Dictionary<GUID, int>();
            var scenesAmount = EditorBuildSettings.scenes.Length;

            for (var i = 0; i < scenesAmount; i++)
            {
                var scene = EditorBuildSettings.scenes[i];
                collection.Add(scene.guid, i);
            }

            return collection;
        }

        public static bool ContainsSceneGuid(this Dictionary<GUID, int> sceneBuildIndexes, string sceneGuid,
            out int buildIndex)
        {
            buildIndex = -1;
            var sceneGuidObj = new GUID(sceneGuid);

            var hasScene = sceneBuildIndexes.TryGetValue(sceneGuidObj, out var sceneIndex);

            if (hasScene)
            {
                buildIndex = sceneIndex;
            }

            return hasScene;
        }
    }
}