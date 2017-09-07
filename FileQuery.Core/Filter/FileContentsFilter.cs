using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FileQuery.Core.Logging;

namespace FileQuery.Core.Filter
{
    /// <summary>
    /// A filter that looks at the contents of a file for the specified pattern
    /// 
    /// Multiple contents filters are chained together so a file only has to be
    /// accessed once then each filter's Accept method is called for each line
    /// of text in the file.
    /// 
    /// Tests:
    /// select from c:/dev/filequery where name=*.cs and content=Test
    /// select from c:/dev/filequery where name=*.cs and content=re(.*Test.*)
    /// </summary>
    public class FileContentsFilter : EqualNotEqualFilter
    {
        private List<Regex> _regex = new List<Regex>();

        public FileContentsFilter(string pattern)
            : this(pattern, FilterOperator.Equal)
        { }

        public FileContentsFilter(params string[] patterns)
            : base(FilterOperator.In)
        {
            foreach (var p in patterns)
            {
                AddPattern(p);
            }
        }

        /// <summary>
        /// Tests that a file contains the specified pattern
        /// </summary>
        /// <param name="pattern">A simple search pattern, may contain wildcards</param>
        /// <param name="op"></param>
        public FileContentsFilter(string pattern, FilterOperator op)
            : base(op)
        {
            AddPattern(pattern);
        }

        public FileContentsFilter(Regex regex, FilterOperator op)
            : base(op)
        {
            _regex.Add(regex);
        }

        private void AddPattern(string pattern)
        {
            var re = new Regex($"({pattern.Replace(".", @"\.").Replace("*", ".*").Replace("?", ".{1}")})", RegexOptions.IgnoreCase);
            _regex.Add(re);
        }

        #region IFileQueryFilter Members

        public override bool AcceptFile(System.IO.FileInfo file)
        {
            // Set initial state of acceptance
            // If checking "contains" assume not accepted
            // If checking "not contains" assume accepted
            bool accepted = (FilterOperator == FilterOperator.NotEqual);

            try
            {
                if (Logger.IsDebugEnabled) Logger.Debug("Testing contents of: " + file);
                using (StreamReader reader = file.OpenText())
                {
                    while (reader.Peek() >= 0)
                    {
                        // Get a line of text
                        string text = reader.ReadLine();

                        if (FilterOperator == FilterOperator.NotEqual)
                        {
                            // If checking for "not contains" and found the text, the filter failed
                            if (MatchText(text))
                            {
                                accepted = false;
                                break;
                            }
                        }
                        else if (FilterOperator != FilterOperator.NotEqual)
                        {
                            // If checking for "contains" and found the text, the filter succeeded
                            if (MatchText(text))
                            {
                                accepted = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (IOException ioe)
            {
                throw new FileQueryException(ioe.Message);
            }

            return accepted;
        }

        /// <summary>
        /// Tests if the specified line of text matches the filter.
        /// </summary>
        /// <param name="text">A line of text</param>
        /// <returns>True if the text matches the filter</returns>
        protected bool MatchText(string text)
        {
            foreach (var re in _regex)
            {
                if (re.IsMatch(text))
                {
                    // Found it no need to continue
                    return true;
                }
            }
            //else if (text.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
            //{
            //    // Found it using simple text match
            //    matched = true;
            //}

            return false;
        }

        public override string Name
        {
            get { return "contents"; }
        }

        #endregion
    }
}
