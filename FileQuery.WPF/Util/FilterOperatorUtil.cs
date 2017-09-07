using System.Collections.Generic;
using FileQuery.Core.Filter;
using FileQuery.Wpf.ViewModels;

namespace FileQuery.Wpf.Util
{
    static class FilterOperatorUtil
    {
        private static readonly Dictionary<string, IEnumerable<FilterOperatorItem>> OperatorsByType = new Dictionary<string, IEnumerable<FilterOperatorItem>>()
        {
            { "Extension", new List<FilterOperatorItem>() {
                new FilterOperatorItem(FilterOperator.Equal, "Equal to"),
                new FilterOperatorItem(FilterOperator.NotEqual, "Not equal to"),
                new FilterOperatorItem(FilterOperator.In, "One of these values") } },
            { "Contents", new List<FilterOperatorItem>() {
                new FilterOperatorItem(FilterOperator.Equal, "Contains text"),
                new FilterOperatorItem(FilterOperator.NotEqual, "Does not contain text"),
                new FilterOperatorItem(FilterOperator.In, "Contains one of these values") } },
            { "Modified Date", new List<FilterOperatorItem>() {
                new FilterOperatorItem(FilterOperator.Equal, "Equal to"),
                new FilterOperatorItem(FilterOperator.NotEqual, "Not equal to"),
                new FilterOperatorItem(FilterOperator.In, "One of these values"),
                new FilterOperatorItem(FilterOperator.LessThan, "Less than"),
                new FilterOperatorItem(FilterOperator.LessThanEqual, "Less than or equal"),
                new FilterOperatorItem(FilterOperator.GreaterThan, "Greater than"),
                new FilterOperatorItem(FilterOperator.GreaterThanEqual, "Greater than or equal") } },
            { "Name", new List<FilterOperatorItem>() {
                new FilterOperatorItem(FilterOperator.Equal, "Equal to"),
                new FilterOperatorItem(FilterOperator.NotEqual, "Not equal to"),
                new FilterOperatorItem(FilterOperator.In, "One of these values") } },
            { "Read Only", new List<FilterOperatorItem>() {
                new FilterOperatorItem(FilterOperator.Equal, "True"),
                new FilterOperatorItem(FilterOperator.NotEqual, "False") } },
            { "Size", new List<FilterOperatorItem>() {
                new FilterOperatorItem(FilterOperator.Equal, "Equal to"),
                new FilterOperatorItem(FilterOperator.NotEqual, "Not equal to"),
                new FilterOperatorItem(FilterOperator.In, "One of these values"),
                new FilterOperatorItem(FilterOperator.LessThan, "Less than"),
                new FilterOperatorItem(FilterOperator.LessThanEqual, "Less than or equal"),
                new FilterOperatorItem(FilterOperator.GreaterThan, "Greater than"),
                new FilterOperatorItem(FilterOperator.GreaterThanEqual, "Greater than or equal") } },
        };

        public static IEnumerable<FilterOperatorItem> GetOperatorsForType(string filterType)
        {
            return OperatorsByType[filterType];
        }
    }
}
