using System.IO;

namespace VarReplacerCmd.FileFinder
{
    public class FileHandle
    {
        public string FileName { get; }

        public FileHandle(string fileName)
        {
            FileName = fileName;
        }

        public string[] GetFileLines()
        {
            return File.ReadAllLines(FileName);
        }

        public void SaveFileFiles(string[] newLines)
        {
            if (File.Exists(FileName))
                File.Delete(FileName);
            File.WriteAllLines(FileName, newLines);
        }
    }
}
