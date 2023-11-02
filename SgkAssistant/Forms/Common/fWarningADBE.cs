using OpenQA.Selenium.Support;
using System.Windows.Forms;
using Telerik.WinControls;

namespace SgkAssistant.Forms.Defs
{
    public partial class fWarningADBE : Telerik.WinControls.UI.RadForm
    {
        public int errorType { get; set; }
        public fWarningADBE(int _errorType)
        {
            errorType = _errorType;
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/download/Confirmation.aspx?ID=13255");
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            radPanel1.Visible = true;
            string msg = "";
            if(IOC.linksDataService.ExistLink(1, out msg))
            {
                lblMessage.Text = "Access Database Engine Bilgisayarınıza Kuruldu";
                btnContinue.Visible = true;
                btnContinue.Enabled = true;
            }
            else
            {
                lblMessage.Text = "Access Database Engine bulunamadı! Kurulumu kontrol edip tekrar deneyin.";
                btnContinue.Visible = false;
                btnContinue.Enabled = false;
            }
        }

        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void fWarningADBE_Load(object sender, System.EventArgs e)
        {
            if (errorType == 0)
            {
                lblAccessError.Text = "Bilgisayarınızda MS Access yüklü değil!";
            }
            else
            {
                lblAccessError.Text = "Bilgisayarınızda Access Database Engine yüklü değil!";
            }
            lblAccessError.AutoSize = true;
            lblAccessError.Left = (this.ClientSize.Width - lblAccessError.Width) / 2;
            radPanel1.Visible = false;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.microsoft.com/tr-tr/microsoft-365");
        }
    }
}
