using System.Collections.Generic;
using FileQuery.Core.Filter;

namespace FileQuery.Core
{
    /// <summary>
    /// Contains all the information about a search including where to search and the filters to apply
    /// </summary>
    public class Query
    {
        #region Properties

        private List<ISearchSource> _fileSources;

        /// <summary>
        /// Set of paths and result sets to search
        /// </summary>
        public IEnumerable<ISearchSource> FileSources
        {
            get { return _fileSources; }
        }

        private HashSet<string> _excludePaths;

        /// <summary>
        /// Set of paths and result sets to exclude from the search
        /// </summary>
        public IEnumerable<string> ExcludePaths
        {
            get { return _excludePaths; }
        }

        /// <summary>
        /// List of query filters to be applied to the search
        /// </summary>
        internal FileQueryFilterList Filters
        {
            get;
        }

        #endregion Properties

        public Query()
        {
            _fileSources = new List<ISearchSource>();
            Filters = new FileQueryFilterList();
            _excludePaths = new HashSet<string>();
        }

        public Query(IEnumerable<ISearchSource> fileSources, IEnumerable<IFileQueryFilter> filters, IEnumerable<string> excludePaths = null)
            : this()
        {
            _fileSources.AddRange(fileSources);
            Filters.AddFilters(filters);
            if (excludePaths != null)
            {
                _excludePaths = new HashSet<string>(excludePaths);
            }
        }

        public void AddFileSource(ISearchSource src)
        {
            _fileSources.Add(src);
        }

        public void AddFilter(IFileQueryFilter filter)
        {
            Filters.AddFilter(filter);
        }

        public void AddExcludePath(string path)
        {
            _excludePaths.Add(path);
        }
    }
}
