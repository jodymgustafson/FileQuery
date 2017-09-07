using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FileQuery.Core.Logging;

namespace FileQuery.Core.Filter
{
    /// <summary>
    /// Filter for testing file name.
    /// The pattern can use star and question mark wildcards.
    /// </summary>
    public class FileNameFilter : EqualNotEqualFilter
    {
        private List<Regex> _regex = new List<Regex>();

        /// <summary>
        /// Used to determine if the filter can be used with directory.GetFiles(searchPattern)
        /// </summary>
        internal bool IsSimpleFileNameFilter
        {
            get;
            private set;
        }

        /// <summary>
        /// When is IsSimpleFileNameFilter is true this is the file name pattern
        /// </summary>
        internal string Pattern
        {
            get;
        }

        /// <summary>
        /// Tests that a file matches the specified pattern
        /// </summary>
        /// <param name="pattern">A simple file search pattern, may contain wildcards</param>
        public FileNameFilter(string pattern)
            : this(pattern, FilterOperator.Equal)
        {
        }

        /// <summary>
        /// Tests that a file matches one of the specified patterns (OR)
        /// </summary>
        /// <param name="pattern">A simple file search pattern, may contain wildcards</param>
        public FileNameFilter(params string[] patterns)
            : base(FilterOperator.In)
        {
            IsSimpleFileNameFilter = false;
            for (var i = 0; i < patterns.Length; i++)
            {
                AddPattern(patterns[i]);
            }
        }

        /// <summary>
        /// Tests that the pattern satisfies the specified operator
        /// </summary>
        /// <param name="pattern">A simple file search pattern, may contain wildcards</param>
        /// <see cref="DirectoryInfo.GetFiles"/>
        /// <param name="op"></param>
        public FileNameFilter(string pattern, FilterOperator op)
            : base(op)
        {
            AddPattern(pattern);

            // Only can use as a simple filter if it's testing equal
            IsSimpleFileNameFilter = (op == FilterOperator.Equal);
            Pattern = pattern;
        }

        public FileNameFilter(Regex regex, FilterOperator op)
            : base(op)
        {
            _regex.Add(regex);
            IsSimpleFileNameFilter = false;
        }

        /// <summary>
        /// Converts a string pattern to a regex and adds it to the list of patterns to check
        /// </summary>
        /// <param name="pattern">Can use star and question mark wildcards</param>
        private void AddPattern(string pattern)
        {
            _regex.Add(new Regex($"(^{pattern.ToLower().Replace(".", @"\.").Replace("*", ".*").Replace("?", ".{1}")}$)"));
        }

        #region IFileQueryFilter Members

        public override bool AcceptFile(FileInfo file)
        {
            bool accept = false;
            foreach (var re in _regex)
            {
                if (Logger.IsDebugEnabled) Logger.Debug("Testing filename: " + file.Name);
                if (re.IsMatch(file.Name.ToLower()))
                {
                    // If one of the pattern matches we are done
                    accept = true;
                    break;
                }
            }
            return (FilterOperator == FilterOperator.NotEqual ? !accept : accept);
        }

        public override string Name
        {
            get { return "name"; }
        }

        #endregion

        public override string ToString()
        {
            return base.ToString() + ": " + Pattern;
        }
    }
}
