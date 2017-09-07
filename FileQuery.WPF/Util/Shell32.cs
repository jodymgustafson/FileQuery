using System;
using System.Runtime.InteropServices;

namespace FileQuery.Wpf.Util
{
    class Shell32
    {
        [Serializable]
        public struct ShellExecuteInfo
        {
            public int Size;
            public uint Mask;
            public IntPtr hwnd;
            public string Verb;
            public string File;
            public string Parameters;
            public string Directory;
            public uint Show;
            public IntPtr InstApp;
            public IntPtr IDList;
            public string Class;
            public IntPtr hkeyClass;
            public uint HotKey;
            public IntPtr Icon;
            public IntPtr Monitor;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        extern public static bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);
        public const uint SW_NORMAL = 1;

        /// <summary>
        /// Shows the "Open With" dialog to let the user select a program to open the file with
        /// </summary>
        /// <param name="file"></param>
        public static void OpenAs(string file)
        {
            ShellExecuteInfo sei = new ShellExecuteInfo();
            sei.Size = Marshal.SizeOf(sei);
            sei.Verb = "openas";
            sei.File = file;
            sei.Show = SW_NORMAL;
            if (!ShellExecuteEx(ref sei))
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }
    }
}
