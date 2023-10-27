using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Linq;
using System.Text;
using Sandland.Core.Runtime.Utils;
using UnityEditor.PackageManager;

namespace Sandland.Domains.Core.Inspectors
{
    [CustomEditor(typeof(Animator))]
    public class AnimatorExtensionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var animator = (Animator)target;

            if (!GUILayout.Button("Generate C# Class"))
            {
                return;
            }

            var path = EditorUtility.OpenFolderPanel("Save C# Class", string.Empty, string.Empty);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            
            path = Path.Combine(path, $"{animator.runtimeAnimatorController.name.ToPascalCase()}Animations.cs");
            GenerateClass(animator, path);
        }

        private void GenerateClass(Animator animator, string path)
        {
            var controller = animator.runtimeAnimatorController as AnimatorController;

            if (controller == null)
            {
                Debug.LogError("Runtime controller is not found");
                return;
            }

            var animations = new HashSet<string>();

            foreach (var layer in controller.layers)
            {
                TraverseStateMachine(layer.stateMachine, animations);
            }

            var parameters = animator.parameters.Select(p => p.name).ToHashSet();

            var content = GetClassContent(animations, parameters, controller.name.ToPascalCase());
            File.WriteAllText(path, content);
            
            AssetDatabase.Refresh();
            Client.Resolve();
        }

        private string GetClassContent(HashSet<string> animations, HashSet<string> parameters, string controllerName)
        {

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"public static class {controllerName}AnimatorUtils");
            stringBuilder.AppendLine("{");
            
            //////// Parameters Class
            stringBuilder.AppendLine($"\tpublic static class Parameters");
            stringBuilder.AppendLine("\t{");

            foreach (var parameter in parameters)
            {
                stringBuilder.AppendLine($"\t\tpublic static readonly int {parameter.ToPascalCase()} = Animator.StringToHash(\"{parameter}\");");
            }
            
            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine();
            
            //////// Animations Class
            stringBuilder.AppendLine($"\tpublic static class Animations");
            stringBuilder.AppendLine("\t{");

            foreach (var animation in animations)
            {
                stringBuilder.AppendLine($"\t\tpublic static readonly string {animation.ToPascalCase()} = \"{animation}\";");
            }
            
            stringBuilder.AppendLine("\t}");

            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        private void TraverseStateMachine(AnimatorStateMachine stateMachine, HashSet<string> animations)
        {
            foreach (var state in stateMachine.states)
            {
                HandleAnimatorState(animations, state.state.motion, state.state.name);
            }

            foreach (var subStateMachine in stateMachine.stateMachines)
            {
                TraverseStateMachine(subStateMachine.stateMachine, animations);
            }
        }

        private void TraverseBlendTree(BlendTree blendTree, HashSet<string> animations)
        {
            foreach (var childMotion in blendTree.children)
            {
                HandleAnimatorState(animations, childMotion.motion, childMotion.motion.name);
            }
        }

        private void HandleAnimatorState(HashSet<string> animations, Motion motion, string animationName)
        {
            if (motion is BlendTree tree)
            {
                TraverseBlendTree(tree, animations);
                return;
            }

            animations.Add(animationName);
        }
    }
}