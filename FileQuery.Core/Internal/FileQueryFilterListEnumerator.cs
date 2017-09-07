using System.Collections;
using System.Collections.Generic;
using FileQuery.Core.Filter;

namespace FileQuery.Core
{
    internal class FileQueryFilterListEnumerator : IEnumerator<IFileQueryFilter>
    {
        //private bool isFirst = true;
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
                //if (isFirst)
                //{
                //    isFirst = false;
                //    if (_filterList.SimpleFileNameFilter != null)
                //    {
                //        return _filterList.SimpleFileNameFilter;
                //    }
                //}
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
            //isFirst = true;
            _enumerator.Reset();
        }
    }
}
