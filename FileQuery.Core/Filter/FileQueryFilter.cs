using System.IO;

namespace FileQuery.Core.Filter
{
    /// <summary>
    /// Abstract base class for all filters
    /// </summary>
    public abstract class FileQueryFilter : IFileQueryFilter
    {
        public FilterOperator FilterOperator
        {
            get;
        }

        protected FileQueryFilter(FilterOperator op)
        {
            FilterOperator = op;
        }

        public abstract string Name
        {
            get;
        }

        public abstract bool AcceptFile(FileInfo file);

        /// <summary>
        /// Determines whether the given file is accepted by this filter.
        /// </summary>
        public bool Accept(FileInfo file)
        {
            bool accept = AcceptFile(file);
            return accept;
        }

        public override string ToString()
        {
            return this.GetType().ToString() + "; op:" + FilterOperator;
        }
    }
}
