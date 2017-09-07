//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FileQuery.Core.Filter;
//using FileQuery.Core.Logging;

//namespace FileQuery.Core.Internal
//{
//    internal class DirectoryFileEnumerator : IEnumerator<FileInfo>
//    {
//        private bool isRecursive;
//        private DirectoryInfo root;
//        private DirectoryInfo curDirectory;
//        private HashSet<string> excludePaths;
//        private FileNameFilter filter;
//        private IEnumerator<FileInfo> files;
//        private IEnumerator<FileInfo> directories;

//        public DirectoryFileEnumerator(string path, bool recurse, HashSet<string> excluded, FileNameFilter nameFilter)
//        {
//            isRecursive = recurse;
//            root = curDirectory = new DirectoryInfo(path);
//            excludePaths = excluded;
//            filter = nameFilter;
//            if (root.Exists)
//            {
//                GetFiles(root);
//            }
//        }

//        private void GetFiles(DirectoryInfo directory)
//        {
//            if (!excludePaths.Contains(directory.FullName.ToLower()))
//            {
//                if (Logger.IsDebugEnabled) Logger.Debug("Searching directory: " + directory);
//                try
//                {
//                    // File name filter is a special case that can be done much faster here
//                    if (filter != null)
//                    {
//                        if (Logger.IsDebugEnabled) Logger.Debug("Using simple file filter: " + filter.Pattern);
//                        files = directory.GetFiles(filter.Pattern).GetEnumerator() as IEnumerator<FileInfo>;
//                    }
//                    else
//                    {
//                        // Can't use simple filter, get all files
//                        files = directory.GetFiles().GetEnumerator() as IEnumerator<FileInfo>;
//                    }

//                    //if (recurse)
//                    //{
//                    //    foreach (DirectoryInfo child in directory.GetDirectories())
//                    //    {
//                    //        SearchDirectory(child, recurse, excludePaths, filters);
//                    //    }
//                    //}
//                }
//                catch (FileQueryAbortedException)
//                {
//                    // Kick it to the curb
//                    throw;
//                }
//                catch (Exception ex)
//                {
//                    if (Logger.IsDebugEnabled) Logger.Debug(ex.ToString());
//                }
//            }
//        }

//        public FileInfo Current
//        {
//            get
//            {
//                return files.Current;
//            }
//        }

//        object IEnumerator.Current
//        {
//            get
//            {
//                return Current;
//            }
//        }

//        public void Dispose()
//        {
            
//        }

//        public bool MoveNext()
//        {
//            if (!files.MoveNext())
//            {
//                if (isRecursive)
//                {

//                }
//                return false;
//            }
//            return true;
//        }

//        public void Reset()
//        {
//            //GetFiles(directory);
//        }
//    }
//}
