using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using System.Runtime.InteropServices;

namespace MacroBoard
{
    internal class TEST : Block
    {
        public TEST(string Name, string LogoUrl, string info)
        {
            base.Name = Name;
            base.LogoUrl = LogoUrl;
            base.info = info;
        }

        /*[DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count);

        [DllImport("user32.dll")]
        static extern int DeferWindowPos(int hWinPosInfo, int hWnd, int hWndInsertAfter, int x, int y,
        int cx, int cy, int uFlags);

        BeginDeferWindowPos
        
        while (true)
            {
                Thread.Sleep(2000);

                const int nChar = 256;
                StringBuilder ss = new StringBuilder(nChar);

                //Run GetForeGroundWindows and get active window informations
                //assign them into handle pointer variable
                IntPtr handle = IntPtr.Zero;
                handle = GetForegroundWindow();

                if (GetWindowText(handle, ss, nChar) > 0)
                {
                    SendKeys.SendWait(ss.ToString());
                }
                else SendKeys.SendWait("Nothing");

            }*/


        public override void Execute()
        {
            /*
            Process.Start(@"C:\WINDOWS\system32\notepad.exe");
            Process.Start("shutdown.exe");
            Thread.Sleep(1000);
            SendKeys.SendWait("PAPA");
            SendKeys.SendWait("^({ESC}E)");
            */


            /*Process.Start("powershell.exe");

            Process[] pname = Process.GetProcessesByName("powershell");
            if (pname.Length == 0)
                MessageBox.Show("nothing");
            else
                MessageBox.Show("run");*/

            //Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "www.google.com");
            //Thread.Sleep(500);
            /*SendKeys.SendWait("ipconfig");
            SendKeys.SendWait("{ENTER}");*/

            //ProcessStartInfo processStartInfo

            //ContainerControl.MousePosition



        }
    }
}
