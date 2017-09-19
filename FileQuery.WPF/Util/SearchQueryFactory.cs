using System;
using System.IO;
using System.Linq;
using FileQuery.Core;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Wpf.Util
{
    /// <summary>
    /// Used to generate a search query from a view model
    /// </summary>
    static class SearchQueryFactory
    {
        public static Query GetSearchQuery(SearchControlViewModel ViewModel)
        {
            Query query = new Query();

            // Get include paths
            foreach (var p in ViewModel.SearchPaths.Where(x => x.IsInclude && x.IsValid))
            {
                ValidateSearchPath(p.PathValue);
                query.AddFileSource(new DirectorySearchSource(p.PathValue, p.IsRecursive));
            }

            // Get exclude paths
            foreach (var ex in ViewModel.SearchPaths.Where(x => x.IsExclude))
            {
                query.AddExcludePath(ex.PathValue);
            }

            // Get results file paths
            foreach (var rf in ViewModel.SearchPaths.Where(x => x.IsResultsFile && x.IsValid))
            {
                query.AddFileSource(GetFileSource(rf.PathValue));
            }

            // Get search params
            foreach (var f in ViewModel.SearchParams.Where(x => x.IsValid))
            {
                query.AddFilter(QueryFilterFactory.GetQueryFilter(f));
            }

            return query;
        }

        private static ISearchSource GetFileSource(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist: " + path);
            }

            return new FileListSearchSource()
            {
                FilePaths = File.ReadAllLines(path)
            };
        }

        private static void ValidateSearchPath(string path)
        {
            
            if (!Directory.Exists(path))
            {
                throw new Exception("Search path doesn't exist: " + path);
            }
        }
    }
}
