using System;

namespace FileQuery.Core.Filter
{
    public class FileReadOnlyFilter : EqualNotEqualFilter
    {
        public bool IsReadOnly
        {
            get;
        }

        public FileReadOnlyFilter(bool isReadOnly)
            : base(FilterOperator.Equal)
        {
            IsReadOnly = isReadOnly;
        }

        public FileReadOnlyFilter(string sIsReadOnly)
            : base(FilterOperator.Equal)
        {
            try
            {
                IsReadOnly = Convert.ToBoolean(sIsReadOnly);
            }
            catch (Exception ex)
            {
                throw new FileQueryException("Error parsing value: " + sIsReadOnly + ": " + ex.Message);
            }
        }

        public override bool AcceptFile(System.IO.FileInfo file)
        {
            bool accept = (file.IsReadOnly == IsReadOnly);
            return (FilterOperator == FilterOperator.NotEqual ?  !accept : accept);
        }

        public override string Name
        {
            get { return "readonly"; }
        }

        public override string ToString()
        {
            return base.ToString() + ": " + IsReadOnly;
        }
    }
}
