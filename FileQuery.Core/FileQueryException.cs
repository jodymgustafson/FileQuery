using System;

namespace FileQuery.Core
{
    public class FileQueryException : Exception
    {
        public FileQueryException(string msg)
            : base(msg)
        {
        }
    }
}
