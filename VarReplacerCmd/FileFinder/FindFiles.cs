using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VarReplacerCmd.FileFinder
{
    public class FindFiles
    {
        public static List<FileHandle> GetFiles(FileSearchPattern fromArgs, string useDir = null)
        {
            var totalList = new List<FileHandle>();

            if (useDir == null)
                useDir = Directory.GetCurrentDirectory();
            
            if (fromArgs.IncludeSubdirectories)
            {
                foreach (var dir in Directory.GetDirectories(useDir))
                    totalList.AddRange(GetFiles(fromArgs, dir));
            }

            foreach (var file in fromArgs.SearchPattern.SelectMany(s => Directory.GetFiles(useDir, s)).Distinct())
                totalList.Add(new FileHandle(file));

            return totalList;
        }
    }
}
