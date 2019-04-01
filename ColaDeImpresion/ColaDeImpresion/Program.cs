using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SGECA.ColaDeImpresion
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);



        [STAThread]
        static void Main(string[] args)
        {

            System.Diagnostics.Process me = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcessesByName(me.ProcessName);
            foreach (System.Diagnostics.Process p in myProcesses)
            {
                if (p.Id != me.Id)
                {
                    SwitchToThisWindow(p.MainWindowHandle, true);
                    return;
                }
            }


         


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
            //Application.Run(new Form1());
        }
    }
}
