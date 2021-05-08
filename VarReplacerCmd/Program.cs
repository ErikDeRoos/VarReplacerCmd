using System;
using System.Linq;
using VarReplacerCmd.Args;
using VarReplacerCmd.FileFinder;
using VarReplacerCmd.Replace;

namespace VarReplacerCmd
{
    class Program
    {
        static int Main(string[] args)
        {
            var whatToDo = new ConsumeArgs(args);
            if (whatToDo.ParseError() || whatToDo.ShowHelp())
            {
                if (whatToDo.ParseError())
                    Console.WriteLine("The syntax of the command is incorrect");

                foreach (var line in whatToDo.HelpData())
                    Console.WriteLine(line);

                return 1;
            }

            GoReplacing(whatToDo);

            return 0;
        }

        private static void GoReplacing(ConsumeArgs whatToDo)
        {
            Console.WriteLine("Starting replacing");
            var replacer = new Replacer(new EnvironmentLookupCached());

            foreach (var file in FindFiles.GetFiles(whatToDo.GetFileSearchPattern()))
            {
                Console.WriteLine($"Replacing in {file.FileName}");
                var unchangedFileLines = file.GetFileLines();
                var changedLines = replacer.ReplaceContent(unchangedFileLines).ToArray();
                file.SaveFileFiles(changedLines);
            }

            Console.WriteLine("Finished replacing");
        }
    }
}
