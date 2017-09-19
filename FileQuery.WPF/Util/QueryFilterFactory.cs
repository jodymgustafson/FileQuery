using System;
using System.Linq;
using FileQuery.Core.Filter;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Wpf.Util
{
    static class QueryFilterFactory
    {
        public static IFileQueryFilter GetQueryFilter(SearchParamItemViewModel f)
        {
            return GetQueryFilter(f.ParamType, f.ParamOperator.Operator, f.ParamValue);
        }

        public static IFileQueryFilter GetQueryFilter(string paramType, FilterOperator paramOp, string paramValue)
        {
            if (paramOp == FilterOperator.In)
            {
                var values = paramValue.Split(',').Select(s => s.Trim()).ToArray();
                if (values.Length > 1)
                {
                    return GetQueryFilter(paramType, values);
                }
            }

            switch (paramType)
            {
                case "Extension": return new FileExtensionFilter(paramValue, paramOp);
                case "Name": return new FileNameFilter(paramValue, paramOp);
                case "Contents": return new FileContentsFilter(paramValue, paramOp);
                case "Modified Date": return new FileDateModifiedFilter(paramValue, paramOp);
                case "Size": return new FileSizeFilter(paramValue, paramOp);
                case "Read Only": return new FileReadOnlyFilter(paramOp == FilterOperator.Equal);
            }

            throw new Exception("Invlaid search parameter type: " + paramType);
        }

        /// <summary>
        /// Gets a query filter for an IN clause
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static IFileQueryFilter GetQueryFilter(string paramType, string[] values)
        {
            switch (paramType)
            {
                case "Extension": return new FileExtensionFilter(values);
                case "Name": return new FileNameFilter(values);
                case "Contents": return new FileContentsFilter(values);
                case "Modified Date": return new FileDateModifiedFilter(values);
                case "Size": return new FileSizeFilter(values);
            }

            throw new Exception("Invlaid search parameter type: " + paramType);
        }
    }
}
