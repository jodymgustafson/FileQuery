using System;
using System.Collections.Generic;

namespace FileQuery.Core.Filter
{
    public class FileSizeFilter : FileQueryFilter
    {
        private List<long> Sizes = new List<long>();

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
        /// <param name="size">Size with an optional modifier such as KB, MB, GB. Default is KB.</param>
        /// <param name="op"></param>
        public FileSizeFilter(string size, FilterOperator op)
            : base(op)
        {
            AddFileSize(size);
        }

        private void AddFileSize(string size)
        {
            // See if the param has a modifier on the end
            if (size.Length > 2 && char.IsLetter(size.Substring(size.Length - 2)[1]))
            {
                // Default to KB
                var byteSize = (long)(Convert.ToDouble(size.Substring(0, size.Length - 2)) * 1024L);

                // Determine the modifier
                string modifier = size.Substring(size.Length - 2).ToUpper();
                switch (modifier)
                {
                    case "KB":
                        // Already computed above
                        break;
                    case "MB":
                        byteSize *= 1024L;
                        break;
                    case "GB":
                        byteSize *= 1024L * 1024L;
                        break;
                    default:
                        throw new FileQueryException("Invalid file size modifier: " + modifier);
                }

                Sizes.Add(byteSize);
            }
            else
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

        private bool TestFileSize(long fileLength, long size)
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
