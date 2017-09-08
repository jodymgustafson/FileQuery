using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileQuery.Core.Filter
{
    public class FileSizeFilter : FileQueryFilter
    {
        protected List<long> Sizes = new List<long>();

        /// <summary>
        /// Tests that a file has the specified size
        /// </summary>
        /// <param name="size">Size in bytes</param>
        public FileSizeFilter(long size)
            : this(size, FilterOperator.Equal)
        { }

        /// <summary>
        /// Tests a file's size
        /// </summary>
        /// <param name="size">Size in bytes</param>
        /// <param name="op"></param>
        public FileSizeFilter(long size, FilterOperator op)
            : base(op)
        {
            Sizes.Add(size);
        }

        /// <summary>
        /// Tests that a file has the specified size
        /// </summary>
        /// <param name="size">Size with an optional modifier such as KB, MB, GB. Default is KB.</param>
        public FileSizeFilter(string size)
            : this(size, FilterOperator.Equal)
        { }

        public FileSizeFilter(params string[] sizes)
            : this(sizes[0], FilterOperator.In)
        {
            for (var i = 1; i < sizes.Length; i++)
            {
                AddFileSize(sizes[i]);
            }
        }

        /// <summary>
        /// Tests a file's size
        /// </summary>
        /// <param name="size">Size with an optional modifier such as KB, MB, GB. Default is KB. The "B" is optional.</param>
        /// <param name="op"></param>
        public FileSizeFilter(string size, FilterOperator op)
            : base(op)
        {
            AddFileSize(size);
        }

        Regex reFileSize = new Regex(@"((?:\d*\.)?\d+)([kmg]*[b]?)");

        private void AddFileSize(string size)
        {
            var match = reFileSize.Match(size.ToLower());
            if (!match.Success)
            {
                throw new FileQueryException("Invalid file size: " + size);
            }

            // See if the param has a modifier on the end
            if (string.IsNullOrEmpty(match.Groups[2].Value))
            {
                // Just a number, default to KB
                try
                {
                    Sizes.Add((long)(Convert.ToDouble(size) * 1024L));
                }
                catch (Exception)
                {
                    throw new FileQueryException("Invalid file size: " + size);
                }
            }
            else
            {
                // Default size is KB
                var decSize = Convert.ToDouble(match.Groups[1].Value) * 1024L;

                // Determine the modifier
                char modifier = match.Groups[2].Value[0];
                switch (modifier)
                {
                    case 'k':
                        // already computed
                        break;
                    case 'm':
                        decSize = decSize * 1024L;
                        break;
                    case 'g':
                        decSize = decSize * 1024L * 1024L;
                        break;
                    default:
                        throw new FileQueryException("Invalid file size modifier: " + modifier);
                }

                Sizes.Add((long)decSize);
            }
        }

        public override string Name
        {
            get { return "size"; }
        }

        #region IFileQueryFilter Members

        public override bool AcceptFile(System.IO.FileInfo file)
        {
            foreach (var size in Sizes)
            {
                if (TestFileSize(file.Length, size))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool TestFileSize(long fileLength, long size)
        {
            switch (FilterOperator)
            {
                case FilterOperator.Equal:
                case FilterOperator.In:
                    return fileLength == size;
                case FilterOperator.GreaterThan:
                    return fileLength > size;
                case FilterOperator.GreaterThanEqual:
                    return fileLength >= size;
                case FilterOperator.LessThan:
                    return fileLength < size;
                case FilterOperator.LessThanEqual:
                    return fileLength <= size;
                case FilterOperator.NotEqual:
                    return fileLength != size;
                default:
                    throw new FileQueryException("Invalid size filter operator: " + FilterOperator);
            }
        }

        #endregion

        public override string ToString()
        {
            return base.ToString() + ": " + string.Join(",", Sizes);
        }
    }
}
