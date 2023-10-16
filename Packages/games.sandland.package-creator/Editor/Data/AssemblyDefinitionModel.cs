using System;
using System.Linq;

namespace Sandland.PackageCreator.Editor.Data
{
    public class AssemblyDefinitionModel
    {
        public string Name { get; set; } = string.Empty;
        public string RootNamespace { get; set; } = string.Empty;
        public string[] References { get; set; } = Array.Empty<string>();
        public string[] IncludedPlatforms { get; set; } = Array.Empty<string>();
        public string[] ExcludedPlatforms { get; set; } = Array.Empty<string>();
        public bool IsUnsafeCodeAllowed { get; set; } = false;
        public bool ShouldOverrideReferences { get; set; } = false;
        public bool IsAutoReferenced { get; set; } = false;
        public string[] DefineConstraints { get; set; } = Array.Empty<string>();
        public string[] VersionDefines { get; set; } = Array.Empty<string>();
        public bool IsNoEngineReferences { get; set; } = false;

        public string ToJson()
        {
            var references = string.Join(",", References?.Select(r => $"\"{r}\"") ?? Array.Empty<string>());
            var includedPlatforms = string.Join(",\n\t\t", IncludedPlatforms?.Select(p => $"\"{p}\"") ?? Array.Empty<string>());
            var excludedPlatforms = string.Join(",\n\t\t", ExcludedPlatforms?.Select(p => $"\"{p}\"") ?? Array.Empty<string>());
            var defineConstraints = string.Join(",\n\t\t", DefineConstraints?.Select(d => $"\"{d}\"") ?? Array.Empty<string>());
            var versionDefines = string.Join(",\n\t\t", VersionDefines?.Select(v => $"\"{v}\"") ?? Array.Empty<string>());

            return $@"{{
    ""name"": ""{Name}"",
    ""rootNamespace"": ""{RootNamespace}"",
    ""references"": [
        {references}
    ],
    ""includePlatforms"": [
        {includedPlatforms}
    ],
    ""excludePlatforms"": [
        {excludedPlatforms}
    ],
    ""unsafeFlags"": {IsUnsafeCodeAllowed.ToString().ToLower()},
    ""overrideReferences"": {ShouldOverrideReferences.ToString().ToLower()},
    ""autoReferenced"": {IsAutoReferenced.ToString().ToLower()},
    ""defineConstraints"": [
        {defineConstraints}
    ],
    ""versionDefines"": [
        {versionDefines}
    ],
    ""noEngineReferences"": {IsNoEngineReferences.ToString().ToLower()}
}}";
        }
    }
}