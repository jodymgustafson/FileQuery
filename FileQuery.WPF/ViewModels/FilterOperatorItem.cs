using FileQuery.Core.Filter;

namespace FileQuery.Wpf.ViewModels
{
    /// <summary>
    /// A 2-tuple for selecting filter operators in a combobox
    /// </summary>
    class FilterOperatorItem
    {
        public FilterOperatorItem(FilterOperator op, string label)
        {
            Operator = op;
            Label = label;
        }

        public FilterOperator Operator { get; }
        public string Label { get; }
    }
}
