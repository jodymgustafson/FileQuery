using System;
using System.Collections.Generic;
using FileQuery.Core.Logging;

namespace FileQuery.Core.Filter
{
    public class FileExtensionFilter : FileNameFilter
    {
        private List<string> _extensions = new List<string>();

        /// <summary>
        /// Tests that a file has the specified extension
        /// </summary>
        /// <param name="extension">file extension (e.g. 'exe')</param>
        public FileExtensionFilter(string extension)
            : this(extension, FilterOperator.Equal)
        { }

        /// <summary>
        /// Tests that a file has one of the specified extenstions
        /// </summary>
        /// <param name="extensions">file extension (e.g. 'exe')</param>
        public FileExtensionFilter(params string[] extensions)
            : this(extensions[0], FilterOperator.In)
        {
            for (var i = 1; i < extensions.Length; i++)
            {
                _extensions.Add(NormalizeExtension(extensions[i]));
            }
        }

        /// <summary>
        /// Tests that the pattern satisfies the specified operator
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="op"></param>
        public FileExtensionFilter(string extension, FilterOperator op)
            : base("*" + NormalizeExtension(extension), op)
        {
            _extensions.Add(NormalizeExtension(extension));
        }

        public override string Name
        {
            get { return "ext"; }
        }

        private static string NormalizeExtension(string ext)
        {
            return ext.StartsWith(".") ? ext : "." + ext;
        }

        #region IFileQueryFilter Members

        public override bool AcceptFile(System.IO.FileInfo file)
        {
            bool accept = false;
            foreach (var ext in _extensions)
            {
                if (Logger.IsDebugEnabled) Logger.Debug("Testing extension: " + ext);
                if (file.Extension.Equals(ext, StringComparison.OrdinalIgnoreCase))
                {
                    // If one of the pattern matches we are done
                    accept = true;
                    break;
                }
            }
            return (FilterOperator == FilterOperator.NotEqual ? !accept : accept);
        }

        #endregion
    }
}
