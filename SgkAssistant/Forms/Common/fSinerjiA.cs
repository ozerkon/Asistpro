using System;
using System.Windows.Forms;
using Telerik.WinControls;

namespace SgkAssistant.Forms.Defs
{
    public partial class FSinerjiA : Telerik.WinControls.UI.RadForm
    {
        public FSinerjiA()
        {
            InitializeComponent();
            btnClose.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
            btnClose.ButtonElement.BorderElement.ForeColor = System.Drawing.Color.FromArgb(120, 148, 186);
            btnClose.ButtonElement.ShowBorder = true;
            btnClose.ForeColor = System.Drawing.Color.FromArgb(21, 66, 139);
            btnClose.ElementTree.Control.Cursor = Cursors.Hand;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sinerjia.net/");
        }
    }
}
