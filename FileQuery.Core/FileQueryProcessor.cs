using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileQuery.Core.Logging;

namespace FileQuery.Core
{
    public class FileFoundEventArgs : EventArgs
    {
        public FileInfo fileInfo;
    }

    /// <summary>
    /// The procesor for executing file queries
    /// </summary>
    public class FileQueryProcessor
    {
        public event EventHandler<FileFoundEventArgs> FileFound;

        /// <summary>
        /// Used to determine if a query is currently running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// The collection of files found by the query
        /// </summary>
        public ObservableCollection<FileInfo> Results
        {
            get; private set;
        }

        private bool abortSearch;

        /// <summary>
        /// Causes the query to be canceled
        /// </summary>
        public void Cancel()
        {
            abortSearch = true;
        }

        public FileQueryProcessor()
        {
            IsRunning = false;
            Results = new ObservableCollection<FileInfo>();
        }

        /// <summary>
        /// Executes a file search using the info provided in the query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> Execute(Query query)
        {
            if (IsRunning) throw new FileQueryException("A query is already running");

            if (Logger.IsDebugEnabled) Logger.Debug("Starting query at: " + DateTime.Now);
            IsRunning = true;
            abortSearch = false;

            try
            {
                Results.Clear();
                var excludePaths = new HashSet<string>(query.ExcludePaths.Select(x => x.ToLower()));

                foreach (var source in query.FileSources)
                {
                    if (abortSearch) break;

                    if (source is FileListSearchSource)
                    {
                        // Search in a set of files
                        SearchResultSet(source as FileListSearchSource, query.Filters);
                    }
                    else if (source is DirectorySearchSource)
                    {
                        // Search in a directory
                        SearchDirectory(source as DirectorySearchSource, excludePaths, query.Filters);
                    }
                    else
                    {
                        throw new FileQueryException("Invalid search source: " + source);
                    }
                }

                return Results;
            }
            finally
            {
                IsRunning = false;
                if (Logger.IsDebugEnabled) Logger.Debug("Query finished at: " + DateTime.Now);
            }
        }

        private void SearchResultSet(FileListSearchSource source, FileQueryFilterList filters)
        {
            foreach (var path in source.FilePaths)
            {
                if (abortSearch) break;

                var file = new FileInfo(path);
                if (file.Exists)
                {
                    TestFile(file, filters);
                }
                else
                {
                    throw new FileQueryException("File does not exist: " + file.FullName);
                }
            }
        }

        #region Search Directory

        /// <summary>
        /// Searching a directory starts here
        /// </summary>
        /// <param name="path"></param>
        /// <param name="query"></param>
        private void SearchDirectory(DirectorySearchSource source, HashSet<string> excludePaths, FileQueryFilterList filters)
        {
            var dir = new DirectoryInfo(source.Path);

            if (dir.Exists)
            {
                SearchDirectory(dir, source.IsRecursive, excludePaths, filters);
            }
            else
            {
                throw new FileQueryException("Directory does not exist: " + source.Path);
            }
        }

        private void SearchDirectory(DirectoryInfo directory, bool recurse, HashSet<string> excludePaths, FileQueryFilterList filters)
        {
            if (!excludePaths.Contains(directory.FullName.ToLower()))
            {
                if (abortSearch) return;

                if (Logger.IsDebugEnabled) Logger.Debug("Searching directory: " + directory);
                try
                {
                    // File name filter is a special case that can be done much faster here
                    if (filters.SimpleFileNameFilter != null)
                    {
                        if (Logger.IsDebugEnabled) Logger.Debug("Using simple file filter: " + filters.SimpleFileNameFilter.Pattern);
                        TestFiles(directory.GetFiles(filters.SimpleFileNameFilter.Pattern), filters);
                    }
                    else
                    {
                        // Can't use simple filter, get all files
                        TestFiles(directory.GetFiles(), filters);
                    }

                    if (recurse)
                    {
                        foreach (DirectoryInfo child in directory.GetDirectories())
                        {
                            SearchDirectory(child, recurse, excludePaths, filters);
                        }
                    }
                }
                catch (FileQueryAbortedException)
                {
                    // Kick it to the curb
                    throw;
                }
                catch (Exception ex)
                {
                    if (Logger.IsDebugEnabled) Logger.Debug(ex.ToString());
                }
            }
        }

        #endregion Search Directory

        #region Test File

        private void TestFiles(FileInfo[] files, FileQueryFilterList filters)
        {
            foreach (FileInfo file in files)
            {
                if (abortSearch) break;
                TestFile(file, filters);
            }
        }

        private void TestFile(FileInfo file, FileQueryFilterList filters)
        {
            if (Logger.IsDebugEnabled) Logger.Debug("Testing file: " + file);

            bool accept = true;

            foreach (var filter in filters)
            {
                if (abortSearch || accept == false)
                {
                    // No need to continue
                    break;
                }

                // Apply the filter
                if (Logger.IsDebugEnabled) Logger.Debug("Checking filter: " + filter.ToString());
                accept = filter.Accept(file);
            }

            if (accept)
            {
                if (Logger.IsDebugEnabled) Logger.Debug("File passed all filters: " + file);
                Results.Add(file);
                FileFound?.Invoke(this, new FileFoundEventArgs() { fileInfo = file });
            }
        }
        
        #endregion Test File
    }
}
