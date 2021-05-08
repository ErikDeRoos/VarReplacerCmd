using System.Collections.Generic;
using System.Linq;
using VarReplacerCmd.FileFinder;

namespace VarReplacerCmd.Args
{
    public class ConsumeArgs
    {
        private static readonly string[] CommandFile = new[] { "-f", };
        private static readonly string[] CommandSubdir = new[] { "-s", };
        private static readonly string[] CommandHelp = new[] { "-help", "-h", "-?" };
        
        private static readonly HashSet<string> AllCommands = new HashSet<string>(
            CommandFile
            .Union(CommandSubdir)
            .Union(CommandHelp)
            .Select(s => s.ToLower().Trim()));

        private readonly ArgContent[] _parsedArgs;

        public ConsumeArgs(string[] args)
        {
            _parsedArgs = ParseArgs(args)?.ToArray();

            if (!(FindFirstArg(CommandFile).Found || FindFirstArg(CommandHelp).Found))
                _parsedArgs = null;  // Error
        }

        public FileSearchPattern GetFileSearchPattern()
        {
            var searchPattern = FindFirstArg(CommandFile);
            var dirPattern = FindFirstArg(CommandSubdir);
            return new FileSearchPattern(searchPattern.Data, dirPattern.Found);
        }

        public bool ParseError()
        {
            return _parsedArgs == null || _parsedArgs.Length == 0;
        }

        public bool ShowHelp()
        {
            return FindFirstArg(CommandHelp).Found;
        }

        public string[] HelpData()
        {
            return HelpLines;
        }

        private ArgContent FindFirstArg(string[] argNames)
        {
            if (_parsedArgs == null)
                return ArgContent.NotFound;

            foreach (var arg in argNames)
            {
                var content = FindArg(arg);
                if (content.Found)
                    return content;
            }
            return ArgContent.NotFound;
        }

        private ArgContent FindArg(string argName)
        {
            if (_parsedArgs == null)
                return ArgContent.NotFound;

            var name = (argName ?? string.Empty).ToLower().Trim();
            return _parsedArgs.FirstOrDefault(p => p.NameLower == name) ?? ArgContent.NotFound;
        }

        private static List<ArgContent> ParseArgs(string[] args)
        {
            var parsedArgs = new List<ArgContent>();

            var argStart = -1;
            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].StartsWith('-'))
                    continue;
                if (!AllCommands.Contains(args[i].ToLower()))
                    return null;  // Error

                if (argStart != -1)
                    parsedArgs.Add(CreateFrom(args, argStart, i - 1));
                argStart = i;
            }
            
            if (argStart == -1)
                return null;  // Error

            parsedArgs.Add(CreateFrom(args, argStart, args.Length - 1));

            return parsedArgs;
        }

        private static ArgContent CreateFrom(string[] args, int first, int last)
        {
            if (first == last)
                return new ArgContent(args[first], new string[0], first);
            return new ArgContent(args[first], args.Skip(first + 1).Take(last - first).ToArray(), first);
        }

        private class ArgContent
        {
            public static readonly ArgContent NotFound = new ArgContent();

            public string NameLower { get; }
            public string Name { get; }
            public string[] Data { get; }
            public int Index { get; }

            public bool Found { get; }

            public ArgContent(string name, string[] data, int index)
            {
                NameLower = (name ?? string.Empty).ToLower().Trim();
                Name = name;
                Data = data;
                Index = index;
                Found = true;
            }
            public ArgContent()
            {
                Found = false;
            }
        }

        #region Help data

        private static readonly string[] HelpLines = new[]
        {
            "Processes all files in its current work directory and replaces build pipe variables in these files with the actual content.",
            "It replaces $(variable) with the intended content, by leveraging the environment variables as specified in https://docs.microsoft.com/en-us/azure/devops/pipelines/process/variablesUsed .",
            string.Empty,
            "varreplacercmd -f filepattern [+ filepattern [+ ...]] [-s]",
            string.Empty,
            "  -f \t\tThe file(s) to be manipulated. May use wildcards, but doesn't support regex.",
            "  -s \t\tAlso look into the sub directories.",
            "  -h \t\tHelp"
        };

        #endregion
    }
}
