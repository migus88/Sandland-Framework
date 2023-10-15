using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sandland.Core.Runtime.Utils
{
    public static class StringUtils
    {
        public static string ToWords(this string input)
        {
            // Insert a space before each uppercase character that isn't the start of the string
            // and isn't preceded or followed by another uppercase character
            var result = Regex.Replace(input, @"(?<!^)(?=[A-Z][a-z])", " ");
            // Insert a space before a number preceded by a letter
            result = Regex.Replace(result, @"(?<=[a-zA-Z])(?=\d)", " ");
            // Capitalize the first character of the result
            return $"{char.ToUpper(result[0])}{result[1..]}";
        }

        public static string ToPascalCase(this string original)
        {
            // TODO: optimize this method
            // Taken from here: https://stackoverflow.com/a/46095771

            var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            var whiteSpace = new Regex(@"(?<=\s)");
            var startsWithLowerCaseChar = new Regex("^[a-z]");
            var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }
    }
}