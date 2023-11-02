using Models.Common;
using SgkAssistant.Forms.Sgk;
using SgkAssistant.Helpers;
using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Forms.Defs
{
    public partial class FUserList : Telerik.WinControls.UI.RadForm
    {
        Users userToBeProcess = new Users();
        GridViewRowInfo row;
        public FUserList()
        {
           InitializeComponent();
        }
        
        private void fUserList_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;

            cbbAdd.Enabled = (Users.ActiveUser.Yetki == 1);
            cbbEdit.Enabled = (Users.ActiveUser.Yetki == 1);
            cbbDelete.Enabled = (Users.ActiveUser.Yetki == 1);

            List();
        }

        private void List()
        {
            string msg = "";
            IOC.WinHelpers.SetUserListRgv(rgvUserList, out msg);
            rgvUserList.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
        }

        private void cbbAdd_Click(object sender, EventArgs e)
        {
            FUser f = new FUser();
            f.ShowDialog();
        }

        private void cbbEdit_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                userToBeProcess = IOC.WinHelpers.GetUserFromRgv(rgvUserList, out msg);
                FUser f = new FUser(userToBeProcess);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        private void cbbDelete_Click(object sender, EventArgs e)
        {
            string msg ;
            try
            {
                row = rgvUserList.CurrentRow;
                userToBeProcess = IOC.WinHelpers.GetUserFromRgv(rgvUserList, out msg);
                bool allowDelete = false; int result = 0;
                var confirmResult = RadMessageBox.Show($"{userToBeProcess.Un} adlı kullanıcı silinecek", "Silme işlemini onayla", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    rgvUserList.ClearSelection();
                }
                else
                {
                    allowDelete = true;
                }
                if (allowDelete == true)
                {
                    result = IOC.UserDataService.DeleteUser(userToBeProcess, out msg);
                    IOC.PkcData.DeleteUserPrm(userToBeProcess.Id, out msg);
                }
                if (result == 1)
                {
                    cblMessage.Text = $"{userToBeProcess.Un} adlı kullanıcı silindi";
                    UserChanged.HasChanged = true;
                    rgvUserList.Rows.Remove(row);
                    rgvUserList.Refresh();
                }
                else
                {
                    cblMessage.Text = $"Veritabanı hatası! Lütfen tekrar deneyiniz";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                cblMessage.Text = $"Silme işlemi başarısız! Hata: {msg}";
            }
        }

        private void cbbExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void cbbCloseMessage_Click(object sender, EventArgs e)
        {
            cblMessage.Text = string.Empty;
        }

        private void rgvUserList_SelectionChanged(object sender, EventArgs e)
        {
            int selectedIndex = rgvUserList.Rows.IndexOf(rgvUserList.CurrentRow);
            if (selectedIndex == 0)
            {
                cbbDelete.Enabled = false;
            }
            else if (selectedIndex > 0 && Users.ActiveUser.Yetki == 1)
            {
                cbbDelete.Enabled = true;
            }
            if (Users.ActiveUser.Id == Convert.ToInt32( rgvUserList.Rows[selectedIndex].Cells[0].Value))
            {
                cbbChangeUser.Enabled = false;
            }
            else
            {
                cbbChangeUser.Enabled = true;
            }
        }

        private void rgvUserList_MouseEnter(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                if (UserChanged.HasChanged == true)
                {
                    IOC.WinHelpers.SetUserListRgv(rgvUserList, out msg);
                    UserChanged.HasChanged = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        private void cbbChangeUser_Click(object sender, EventArgs e)
        {
            string msg;
            Users u;
            u = IOC.WinHelpers.GetUserFromRgv(rgvUserList, out msg);
            FLogin f = new FLogin(u);
            Users.ActiveUser.UserChangeRequest = true;
            if (f.ShowDialog() == DialogResult.OK)
            {
                Changed.HasChanged = true;
                this.DialogResult = DialogResult.Yes;
                Hide();
            }

        }

        private void cblMessage_TextChanged(object sender, EventArgs e)
        {
            if (cblMessage.Text == string.Empty)
            {
                cbreMessage.Visibility = ElementVisibility.Collapsed;
            }
            else
            {
                cbreMessage.Visibility = ElementVisibility.Visible;
            }
            
        }
    }
}
