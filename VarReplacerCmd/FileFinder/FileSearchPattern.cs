namespace VarReplacerCmd.FileFinder
{
    public class FileSearchPattern
    {
        public string[] SearchPattern { get; }
        public bool IncludeSubdirectories { get; }

        public FileSearchPattern(string[] searchPattern, bool includeSubdirectories)
        {
            SearchPattern = searchPattern;
            IncludeSubdirectories = includeSubdirectories;
        }
    }
}
