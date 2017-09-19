using System.Collections;
using System.Collections.Generic;
using FileQuery.Core.Filter;

namespace FileQuery.Core
{
    /// <summary>
    /// Enumerator over the items in a FileQueryFilterList
    /// </summary>
    internal class FileQueryFilterListEnumerator : IEnumerator<IFileQueryFilter>
    {
        private FileQueryFilterList _filterList;
        private IEnumerator<IFileQueryFilter> _enumerator;

        public FileQueryFilterListEnumerator(FileQueryFilterList filterList)
        {
            _filterList = filterList;
            _enumerator = _filterList.Filters.GetEnumerator();
        }

        public IFileQueryFilter Current
        {
            get
            {
                return _enumerator.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }
    }
}
