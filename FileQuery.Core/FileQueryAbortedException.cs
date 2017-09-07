namespace FileQuery.Core
{
    /// <summary>
    /// This class is merely to mark query aborted exceptions
    /// </summary>
    public class FileQueryAbortedException : FileQueryException
    {
        public FileQueryAbortedException(string msg)
            : base(msg)
        {
        }
    }
}
