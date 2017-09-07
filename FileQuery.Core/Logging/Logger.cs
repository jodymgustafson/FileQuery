namespace FileQuery.Core.Logging
{
    public class Logger
    {
        private static bool debug;
        public static bool IsDebugEnabled
        {
            get { return debug; }
            set { debug = value; }
        }

        public static void Debug(string message)
        {
            if (debug)
            {
                System.Console.WriteLine("DEBUG: " + message);
            }
        }

        public static void Error(string message)
        {
            System.Console.WriteLine("ERROR: " + message);
        }
    }
}
