using SgkAssistant.Forms.Common;
using System;
using System.Windows.Forms;

namespace SgkAssistant
{
    static class Program
    {
        public static FSplash SplashForm = null;
        [STAThread]
        static void Main()
        {
            if (Properties.Settings.Default.FirstTimeRunningThisVersion)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.FirstTimeRunningThisVersion = false;
                Properties.Settings.Default.Save();
            }

            Control.CheckForIllegalCrossThreadCalls = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FSplash f = new FSplash();
            if (f.ShowDialog() == DialogResult.OK)
            {
                FBaslangic parentForm = new FBaslangic();
                Forms.FormLoader.MdiMainForm = parentForm;
                Application.Run(parentForm);
                f.Dispose();
            }
            else
            {
                Application.Exit();
            }
        }
        public static bool IsFormOpen(Form nameForm)
        {
            bool isFound = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name.Equals(nameForm.Name))
                {
                    isFound = true;
                }
            }
            return isFound;
        }
     }
}