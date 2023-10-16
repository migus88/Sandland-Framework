using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandland.PackageCreator.Editor.Data
{
    internal class PackageModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Version { get; set; }
        public string MinUnityVersion { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }

        public string ToJson()
        {
            var dependencies = string.Join(",\n\t\t", Dependencies?.Select(kvp => $"\"{kvp.Key}\":\"{kvp.Value}\"") ?? Array.Empty<string>());
            return $"{{\n\t\"name\": \"{Name}\",\n\t\"displayName\": \"{DisplayName}\",\n\t\"version\": \"{Version}\",\n\t\"unity\": \"{MinUnityVersion}\",\n\t\"description\": \"{Description}\",\n\t\"dependencies\": {{\n\t\t{dependencies}\n\t}}\n}}";
        }
    }
}