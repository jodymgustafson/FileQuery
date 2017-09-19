using System;

namespace FileQuery.Core.Filter
{
    public class FileQueryFilterFactory
    {
        /// <summary>
        /// Builds a filter instance
        /// </summary>
        /// <param name="filterType">Name of the filter</param>
        /// <param name="value">Value of the filter</param>
        /// <param name="op">Filter operator</param>
        /// <returns></returns>
        public static IFileQueryFilter GetFileQueryFilter(FilterType filterType, FilterOperator op, string value)
        {
            switch (filterType)
            {
                case FilterType.Name:
                    return new FileNameFilter(value, op);
                case FilterType.Size:
                    return new FileSizeFilter(value, op);
                case FilterType.Contents:
                    return new FileContentsFilter(value, op);
                case FilterType.Extension:
                    return new FileExtensionFilter(value, op);
                case FilterType.ModifiedDate:
                    return new FileDateModifiedFilter(value, op);
                case FilterType.ReadOnly:
                    return new FileReadOnlyFilter(value);
                default:
                    throw new FileQueryException("Invalid query filter: " + filterType);
            }
        }

        /// <summary>
        /// Gets a filter that uses an IN clause
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IFileQueryFilter GetFileQueryFilter(FilterType filterType, string[] values)
        {
            switch (filterType)
            {
                case FilterType.Name:
                    return new FileNameFilter(values);
                case FilterType.Size:
                    return new FileSizeFilter(values);
                case FilterType.Contents:
                    return new FileContentsFilter(values);
                case FilterType.Extension:
                    return new FileExtensionFilter(values);
                case FilterType.ModifiedDate:
                    return new FileDateModifiedFilter(values);
                default:
                    throw new FileQueryException("Invalid query filter for IN clause: " + filterType);
            }
        }
    }
}
