namespace FileQuery.Core.Filter
{
    /// <summary>
    /// Base class for filters that only accept = or <> operators
    /// </summary>
    public abstract class EqualNotEqualFilter : FileQueryFilter
    {
        public EqualNotEqualFilter(FilterOperator op)
            : base(op)
        {
            ValidateOperator();
        }

        protected void ValidateOperator()
        {
            // Check for valid operator
            switch (FilterOperator)
            {
                case FilterOperator.Equal:
                case FilterOperator.NotEqual:
                case FilterOperator.In:
                    return;
                default:
                    throw new FileQueryException($"Invalid '{Name}' filter operator: {FilterOperator}");
            }
        }
    }
}
