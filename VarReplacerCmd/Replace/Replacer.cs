using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VarReplacerCmd.Replace
{
    public class Replacer
    {
        private readonly IReferenceLookup _useLookupSystem;

        private readonly Regex VarRegEx = new Regex(@"\$\(([\w.]*)\)", RegexOptions.Compiled);

        public Replacer(IReferenceLookup useLookupSystem)
        {
            _useLookupSystem = useLookupSystem;
        }

        public IEnumerable<string> ReplaceContent(string[] content)
        {
            foreach(var processLine in content)
            {
                var result = VarRegEx.Matches(processLine);
                if (result.Count != 0)
                    yield return ParseVarsInLine(processLine, result);
                else
                    yield return processLine;
            }
        }

        public string ParseVarsInLine(string line, MatchCollection regExMatch)
        {
            var lineBuilder = new StringBuilder();
            var lastIndex = 0;

            foreach (Match match in regExMatch)
            {
                // Add the stuff between the matches to the output
                lineBuilder.Append(line.Substring(lastIndex, match.Groups[0].Index - lastIndex));
                lastIndex = match.Groups[0].Index + match.Groups[0].Length;

                lineBuilder.Append(_useLookupSystem.Lookup(match.Groups[1].Value));
            }
            
            // Aaaand add the remainder
            lineBuilder.Append(line.Substring(lastIndex, line.Length - lastIndex));

            return lineBuilder.ToString();
        }
    }
}
