using System.IO;

namespace FileQuery.Core.Filter
{
    public enum FilterOperator { Equal, NotEqual, LessThan, LessThanEqual, GreaterThan, GreaterThanEqual, In }

    /// <summary>
    /// All file filters must implement this interface
    /// </summary>
    public interface IFileQueryFilter
    {
        /// <summary>
        /// Determines whether the given file is accepted by this filter.
        /// </summary>
        bool Accept(FileInfo file);

        /// <summary>
        /// Property to get the name of the filter
        /// </summary>
        string Name { get; }
    }
}
