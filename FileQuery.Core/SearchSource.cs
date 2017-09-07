using System.Collections.Generic;

namespace FileQuery.Core
{
    public interface ISearchSource// : IEnumerable<FileInfo>
    {
    }

    /// <summary>
    /// Defines a directory to search for files in
    /// </summary>
    public class DirectorySearchSource : ISearchSource
    {
        public DirectorySearchSource(string path, bool isRecursive = true)
        {
            Path = path;
            IsRecursive = isRecursive;
        }

        public string Path { get; set; }
        public bool IsRecursive { get; set; }
    }

    /// <summary>
    /// Defines a list of files to search
    /// </summary>
    public class FileListSearchSource : ISearchSource
    {
        public IEnumerable<string> FilePaths { get; set; }
    }
}
