using System;
using Sandland.Domains.Core.Shared.Data;
using UnityEngine;

namespace Sandland.Domains.Core.Shared.Utils
{
    public static class EditorUtils
    {
        public static UnityVersion GetCurrentUnityVersion()
        {
            var versionString = Application.unityVersion;
            var version = new UnityVersion();

            var parts = versionString.Split('.');
            if (parts.Length < 3)
            {
                throw new ArgumentException("Invalid Unity version format. Expected at least three parts separated by dots.");
            }

            version.Year = parts[0];
            version.Major = parts[1];

            var minorAndPatch = parts[2].Split('f');
            if (minorAndPatch.Length < 1)
            {
                throw new ArgumentException("Invalid Unity version format. Expected 'f' followed by a patch number.");
            }

            version.Minor = minorAndPatch[0];
            version.PatchNumber = minorAndPatch.Length > 1 ? minorAndPatch[1] : "0";

            return version;
        }
    }
}