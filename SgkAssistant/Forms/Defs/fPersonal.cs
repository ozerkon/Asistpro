using Models.Common;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FPersonal : RadForm
    {
        private Personal personal ; 
        private int cid = 0; 
        private int index = 0; 
        private string msg = ""; 
        private decimal defaultTih = 0;
        private DateTime defaultIgt = DateTime.Now;
        private DateTime defaultDtr = DateTime.Now;
        private bool willUpdate = false;
        public FPersonal(int cid, Personal personal = null)
        {
            InitializeComponent();
            this.personal =  personal;
            this.cid = cid;
        }
        private void fPersonal_Load(object sender, EventArgs e)
        {
            dtpBirthday.MinDate = new DateTime(1930, 1, 1);
            dtpBirthday.MaxDate = DateTime.Now;
            dtpHireDate.MaxDate = DateTime.Now;
            dtpLeftDate.MaxDate = DateTime.Now;
            if (personal != null)
            {
                texTcno.Text = personal.Tcno; texTcno.Enabled = false;
                texName.Text = personal.Ads;  
                dtpBirthday.Value = personal.Dtr.Date;
                dtpHireDate.Value = personal.Igt.Date;
                if (personal.Active) { cbPersonalLeft.CheckState = CheckState.Unchecked; dtpLeftDate.Enabled = false; }
                else{ cbPersonalLeft.CheckState = CheckState.Checked; dtpLeftDate.Enabled = true; }
                dtpLeftDate.Value = personal.Ict.Date;
                nudTih.Value = personal.Tih;
                cbPersonalLeft.Enabled = true;

                defaultTih = personal.Tih;
                defaultDtr = personal.Dtr;
                defaultIgt = personal.Igt;
                Text = "Personel Bilgilerini Güncelle";
                btnAddPersonal.Text = "Güncelle";
                btnAddPersonal.Image = Resources.update;
                willUpdate = true;
            }
            else
            {
                cbPersonalLeft.CheckState = CheckState.Unchecked; dtpLeftDate.Enabled = false;
                cbPersonalLeft.Enabled = false;

                Text = "Personel Ekle";
                btnAddPersonal.Text = "Ekle";
                btnAddPersonal.Image = Resources.add;
                willUpdate = false;
            }

            Dictionary<string, int> dcCompanies = new Dictionary<string, int>();
            int counter = 0;
            foreach (Company comp in GlobalVars.Companies)
            {
                dcCompanies.Add(comp.CompanyName, comp.Id);
                if (comp.Id == cid) index = counter;
                counter++;
            }
            ddlCompanies.DataSource = dcCompanies;
            ddlCompanies.DisplayMember = "key";
            ddlCompanies.ValueMember = "value";
            ddlCompanies.SelectedIndex = index;
        }
        private void btnAddPersonal_Click(object sender, EventArgs e)
        {
            PersonalChange.HasChanged = false;
            if (!CheckForm()) return;
            DialogResult dialogResult = DialogResult.OK;
            if (willUpdate && nudTih.Value != defaultTih)
            {
                dialogResult = RadMessageBox.Show(
                    $"Personelin 'Önceki Dönemlerden Devreden İzin Gün Sayısı' bilgisi değiştirildi! Devam ederseniz bu personel için eklenmiş izinler silinecektir.\r\nDevam etmek istiyor musunuz?",
                    $"Önceki Dönemlerden Devreden İzin Gün Sayısı değiştirildi", MessageBoxButtons.YesNo,
                    RadMessageIcon.Question);
            }
            else if (willUpdate && defaultDtr.Date != dtpBirthday.Value.Date)
            {
                dialogResult = RadMessageBox.Show(
                    $"Personelin 'Doğum Tarihi' bilgisi değiştirildi! Devam ederseniz bu personel için eklenmiş izinler silinecektir.\r\nDevam etmek istiyor musunuz?",
                    $"Doğum Tarihi değiştirildi", MessageBoxButtons.YesNo,
                    RadMessageIcon.Question);
            }
            else if (willUpdate && defaultIgt.Date != dtpHireDate.Value.Date)
            {
                dialogResult = RadMessageBox.Show(
                    $"Personelin 'İşe Giriş Tarihi' bilgisi değiştirildi! Devam ederseniz bu personel için eklenmiş izinler silinecektir.\r\nDevam etmek istiyor musunuz?",
                    $"İşe Giriş Tarihi değiştirildi", MessageBoxButtons.YesNo,
                    RadMessageIcon.Question);
            }

            if (dialogResult == DialogResult.No)
            {
                lblMessage.Text = "Güncelleme iptal edildi"; return;
            }
            IOC.PersonalService.AddPersonal(personal, out msg);
            lblMessage.Text = msg;
            if (willUpdate)
            {
                defaultTih = personal.Tih;
                defaultDtr = personal.Dtr;
                defaultIgt = personal.Igt;

                IOC.LeaveDataService.DeleteAllLeaves(personal.Id, out msg);
                //IOC.LeavePeriodDataService.DeleteLeavePeriodsByTcno(personal.Tcno, out msg);
            }
            if (msg.Contains("eklendi") || msg.Contains("güncellendi")) personal = null;
            
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            texName.Text = "";
            texTcno.Text = willUpdate? texTcno.Text :  "";
            nudTih.Value = 0;
        }
        private bool CheckForm()
        {
            string msg = "";
            Regex rx = new Regex(@"^[a-zA-Z ğüşıöçĞÜŞİÖÇ]{5,50}$");
            if (texTcno.Text.Trim().Length < 11)
            {
                lblMessage.Text = "Kimlik numarası eksik girildi"; texTcno.Focus(); SetTextBoxError(texTcno); return false;
            }
            else if (!IOC.WinHelpers.IsTcnoValid(texTcno.Text.Trim(), out msg))
            {
                lblMessage.Text = msg; texTcno.Focus(); SetTextBoxError(texTcno); return false;
            }

            if (texName.Text.Trim().Length < 5 )
            {
                lblMessage.Text = "Ad soyad eksik girildi"; texName.Focus(); SetTextBoxError(texName); return false;
            }
            else if (!IOC.WinHelpers.IsNameValid(texName.Text.Trim(), out msg))
            {
                lblMessage.Text = "Ad soyad için geçersiz karakter girildi"; texName.Focus(); SetTextBoxError(texName); return false;
            }
            if(nudTih.Value % 0.5m != 0) { lblMessage.Text = "Önceki dönemlerden devreden izin gün sayısı hatalı girildi!\r\nGirilen değer 0.5'in katları olmalıdır"; nudTih.Focus();  return false; }
            
            personal = personal == null ? new Personal() : personal;
            
            personal.Tcno = texTcno.Text;
            personal.Ads = texName.Text.Trim();
            personal.Cid = cid;
            personal.Dtr = dtpBirthday.Value.Date;
            personal.Igt = dtpHireDate.Value.Date;
            personal.Ict = cbPersonalLeft.CheckState == CheckState.Checked? dtpLeftDate.Value.Date : personal.Ict.Date;
            personal.Tih = nudTih.Value;
            personal.Active = !cbPersonalLeft.Checked;

            decimal maxLeaveDays = IOC.PersonalService.GetMaxLeaveDay(personal);
            if (personal.Tih > maxLeaveDays ) { lblMessage.Text = $"Önceki dönemlerden devreden izin gün sayısı hak edilenden fazla!\r\nSon dönem hariç hak edilenden gün sayısı : {maxLeaveDays} gün"; nudTih.Focus(); return false; }

            lblMessage.Text = "";
            return true;
        }
        private void texTcno_TextChanged(object sender, EventArgs e)
        {
            if (texTcno.Text.Trim() == "")
            {
                lblMessage.Text = String.Empty;
                texTcno.Clear();
            }
        }
        private void texTcno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void SetTextBoxError(object sender)
        {
            RadTextBoxControl t = sender as RadTextBoxControl;
            t.TextBoxElement.BorderColor = Color.MediumVioletRed;
        }
        private void dtpBirthday_ValueChanged(object sender, EventArgs e)
        {
            dtpHireDate.MinDate = dtpBirthday.Value;
        }
        private void dtpHireDate_ValueChanged(object sender, EventArgs e)
        {
            dtpLeftDate.MinDate = dtpHireDate.Value;
        }
        private void cbPersonalLeft_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            dtpLeftDate.Enabled = cbPersonalLeft.CheckState == CheckState.Checked ? true : false;
        }
        private void ddlCompanies_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            cid = ((KeyValuePair<string, int>)ddlCompanies.SelectedItem.DataBoundItem).Value;
        }

        private void SetNudTihMax()
        {
            Personal temp = new Personal() { Dtr = dtpBirthday.Value.Date, Igt = dtpHireDate.Value.Date };
            decimal maxLeaveDays = IOC.PersonalService.GetMaxLeaveDay(temp);
            nudTih.Maximum = maxLeaveDays;
        }

        private void nudTih_Enter(object sender, EventArgs e)
        {
            SetNudTihMax();
        }
    }
}
