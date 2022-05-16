﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace MacroBoard
{
   public static class Utils
   {


//-------------------------------------------------------------------------


        public static string concatPathWithFileName(string path, string fileName)
        {
            return path +
                   (path.EndsWith(@"\")? "":@"\")+
                   fileName;
        }





//-------------------------------------------------------------------------

        public static Process? getRecentProcess(string processName, bool exactMatching=false)
        {
            Process[] processes = Process.GetProcesses();
            // TODO: .StartTime acces denied, donc pas de trie par ordre de creation, mais il semble que se soit par defaut par ordre du dernier utilisé
            //IEnumerable<Process> processes = pre_proc.OrderBy(p => p.StartTime); s
            foreach (Process proc in processes)
            {
                if (exactMatching)
                {
                    if (proc.ProcessName.Equals(processName))
                        return proc;
                }
                else
                {
                    if (proc.ProcessName.ToLower().Contains(processName.ToLower()))
                        return proc;
                }
            }
            return null;
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);



 //-------------------------------------------------------------------------

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);


 //-------------------------------------------------------------------------

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);


 //-------------------------------------------------------------------------

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
 
        
 //-------------------------------------------------------------------------

        [DllImport("user32")]
        public static extern void LockWorkStation();


//-------------------------------------------------------------------------

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);







    }
}