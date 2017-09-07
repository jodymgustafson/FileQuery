using System.Collections;
using System.Collections.Generic;
using FileQuery.Core.Filter;

namespace FileQuery.Core
{
    /// <summary>
    /// Manages the list of query filters.
    /// Puts filters in order for optimal performance.
    /// Tightly coupled to FileQuery.
    /// </summary>
    internal class FileQueryFilterList : IEnumerable<IFileQueryFilter>
    {
        private LinkedList<IFileQueryFilter> _filterList = new LinkedList<IFileQueryFilter>();

        public IEnumerable<IFileQueryFilter> Filters
        {
            get { return _filterList; }
        }

        /// <summary>
        /// A simple file name filter that can be applied directly to get the
        /// files in the processor.
        /// </summary>
        public FileNameFilter SimpleFileNameFilter
        {
            get; set;
        }

        public FileQueryFilterList() { }

        public FileQueryFilterList(IEnumerable<IFileQueryFilter> filters)
        {
            AddFilters(filters);
        }

        public void AddFilters(IEnumerable<IFileQueryFilter> filters)
        {
            foreach (var f in filters)
            {
                AddFilter(f);
            }
        }

        /// <summary>
        /// Adds a filter
        /// </summary>
        /// <param name="filter"></param>
        public void AddFilter(IFileQueryFilter filter)
        {
            // The first simple file name filter can be used in DirectoryInfo.GetFiles()
            if (SimpleFileNameFilter == null && (filter is FileNameFilter && ((FileNameFilter)filter).IsSimpleFileNameFilter))
            {
                SimpleFileNameFilter = (FileNameFilter)filter;
            }

            if (filter is FileContentsFilter)
            {
                // File contents filters are the most labor intensive so add them to the end of the list
                _filterList.AddLast(filter);
            }
            else
            {
                // Other filters go to the front of the list
                _filterList.AddFirst(filter);
            }
        }

        public IEnumerator<IFileQueryFilter> GetEnumerator()
        {
            return new FileQueryFilterListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new FileQueryFilterListEnumerator(this);
        }
    }
}
