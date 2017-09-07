using System;

namespace FileQuery.Core.Filter
{
    /// <summary>
    /// Takes an expression and parses it into a query filter
    /// 
    /// TODO: Make this able to load user defined filters from other assemblies
    /// http://my.execpc.com/~gopalan/dotnet/reflection.html
    /// </summary>
    public class FileQueryFilterFactory
    {
        public static IFileQueryFilter GetFileQueryFilter(string expression)
        {
            string name = "";
            string value = "";
            FilterOperator op = FilterOperator.Equal;
            if (ParseExpression(expression, ref name, ref value, ref op))
            {
                if (op == FilterOperator.In)
                {
                    return GetFileQueryFilter(name, ParseInValues(value), FilterOperator.Equal);
                }
                else
                {
                    return GetFileQueryFilter(name, value, op);
                }
            }
            else
            {
                throw new FileQueryException("Invalid query: Where clause invalid");
            }
        }

        public static IFileQueryFilter GetFileQueryFilter(string name, string value, FilterOperator op)
        {
            switch (name.ToLower())
            {
                case "name":
                    return new FileNameFilter(value, op);
                case "size":
                    return new FileSizeFilter(value, op);
                case "contents":
                    return new FileContentsFilter(value, op);
                case "ext":
                    return new FileExtensionFilter(value, op);
                case "modified":
                    return new FileDateModifiedFilter(value, op);
                case "readonly":
                    return new FileReadOnlyFilter(value);
                default:
                    throw new FileQueryException("Invalid query filter: Invalid filter: " + name);
            }
        }

        /// <summary>
        /// Gets a filter and chains other filters for the IN clause to it
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static IFileQueryFilter GetFileQueryFilter(string name, string[] values, FilterOperator op)
        {
            // When the op is IN chain together all of the values as filters
            IFileQueryFilter firstFilter = GetFileQueryFilter(name, values[0].Trim(), op);
            //if (firstFilter is FileQueryFilter)
            //{
            //    FileQueryFilter lastFilter = (FileQueryFilter)firstFilter;
            //    lastFilter.IsInClauseFilter = true;
            //    // Chain the rest of this filters to the first one
            //    for (int i = 1; i < values.Length; i++)
            //    {
            //        string value = values[i].Trim();
            //        lastFilter.NextFilter = (FileQueryFilter)GetFileQueryFilter(name, value, op);
            //        lastFilter = lastFilter.NextFilter;
            //        lastFilter.IsInClauseFilter = true;
            //    }
            //}
            //else
            //{
            //    throw new FileQueryException("Filter does not support IN clause: " + firstFilter.GetType());
            //}

            return firstFilter;
        }

        private static bool ParseExpression(string expression, ref string name, ref string value, ref FilterOperator op)
        {
            int opSize = 1;

            // Find the operator
            char[] operators = {'=', '<', '>', '!', ' ', '\t'};
            int idxOp = expression.IndexOfAny(operators);
            if (idxOp < 0)
            {
                throw new FileQueryException("Invalid WHERE clause: Missing operator");
            }

            // Strip whitespace
            while (idxOp < expression.Length && char.IsWhiteSpace(expression[idxOp])) idxOp++;

            string sOp = expression.Substring(idxOp);
            // Get operator
            if (sOp.StartsWith("<>") || sOp.StartsWith("!="))
            {
                op = FilterOperator.NotEqual;
                opSize = 2;
            }
            else if (sOp.StartsWith("<="))
            {
                op = FilterOperator.LessThanEqual;
                opSize = 2;
            }
            else if (sOp[0] == '<')
            {
                op = FilterOperator.LessThan;
            }
            else if (sOp.StartsWith(">="))
            {
                op = FilterOperator.GreaterThanEqual;
                opSize = 2;
            }
            else if (sOp[0] == '>')
            {
                op = FilterOperator.GreaterThan;
            }
            else if (sOp[0] == '=')
            {
                op = FilterOperator.Equal;
            }
            else if (sOp.StartsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                op = FilterOperator.In;
                opSize = 2;
            }
            else
            {
                throw new FileQueryException("Invalid WHERE clause: Missing or invalid operator");
            }

            name = expression.Substring(0, idxOp).Trim().ToLower();
            value = expression.Substring(idxOp + opSize).Trim();

            return true;
        }

        private static string[] ParseInValues(string rawValue)
        {
            if (rawValue[0] == '(' && rawValue[rawValue.Length - 1] == ')')
            {
                // Get list of values between parens
                return rawValue.Substring(1, rawValue.Length - 2).Split(',');
            }
            else
            {
                throw new FileQueryException("Invalid IN clause: Missing parens");
            }

            //return new string[] { rawValue };
        }
    }
}
