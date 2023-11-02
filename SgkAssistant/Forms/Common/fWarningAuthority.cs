using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
namespace SgkAssistant.Forms.Defs
{
    public partial class FWarningAuthority : Telerik.WinControls.UI.RadForm
    {
        public FWarningAuthority()
        {
            InitializeComponent();
        }

        private void fWarningAuthority_Load(object sender, EventArgs e)
        {
            List<RadButton> radButtons = new List<RadButton>() { btnOpenFileProperties, btnReTry, btnCancel };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region fileProperties
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref Shellexecuteinfo lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct Shellexecuteinfo
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SwShow = 5;
        private const uint SeeMaskInvokeidlist = 12;
        public static bool ShowFileProperties(string filename)
        {
            Shellexecuteinfo info = new Shellexecuteinfo();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = filename;
            info.nShow = SwShow;
            info.fMask = SeeMaskInvokeidlist;
            return ShellExecuteEx(ref info);
        }
        #endregion

        private void btnOpenFileProperties_Click(object sender, EventArgs e)
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string path = $@"{appPath}\Asistpro.exe";
            ShowFileProperties(path);
        }

        private void btnReTry_Click(object sender, EventArgs e)
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string path = $@"{appPath}\Asistpro.exe";
            System.Diagnostics.Process.Start(path);
            Application.Exit(); 
        }

        private void btnReTry_ToolTipTextNeeded(object sender, Telerik.WinControls.ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = "<html>Dosya özelliklerinden <strong>Bu Programı Yönetici Olarak Çalıştır</strong>\n seçeneğini işaretlediyseniz, yeniden çalıştırmayı deneyebilirsiniz</html>";
        }
    }
}
