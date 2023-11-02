using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FLeave : RadForm
    {
        private Leaves leave = new Leaves();
        private Company company;
        private Personal personal;
        private int index = 0;
        private int cid = 0;
        private string msg = "", toolTipText = "";
        private Dictionary<string, int> dcCompanies = new Dictionary<string, int>();
        private bool isMultiName = false;
        private bool unSavedPersonal = false;
        private bool isUpdate = false;
        private string multiName = "";
        private string filePath = "";
        private decimal timeval = 0;
        private decimal timevalOfDeletedLeave = 0;
        List<Leaves> alvs = new List<Leaves>();
        public FLeave(Company company, Leaves leaves = null, Personal personal = null)
        {
            InitializeComponent();
            leave = leaves;
            this.company = company;
            this.personal = personal;
        }
        private void fLeave_Load(object sender, EventArgs e)
        {
            dtpBirthday.MinDate = new DateTime(1940, 1, 1);
            dtpBirthday.MaxDate = DateTime.Now;
            dtpHireDate.MaxDate = DateTime.Now;
            dtpStartDate.MaxDate = DateTime.Now;
            dtpEndDate.MaxDate = DateTime.Now;
            pnlAddFile.Enabled = false;
            if (personal != null) SetControls(personal.Id);
            if (leave != null)
            {
                bool docref = false;
                if(leave.Docref != null && leave.Docref != "") { docref = true; int slash = leave.Docref.LastIndexOf(@"\") +1 ; leave.Docref = leave.Docref.Substring(slash);  }
                SetControls(leave.Pid);
                dtpStartDate.Value = leave.Startdate;
                dtpEndDate.Value = leave.Enddate;
                cbFileAdd.CheckState = docref ? CheckState.Checked : CheckState.Unchecked;
                rbPaid.CheckState = leave.Paid ? CheckState.Checked : CheckState.Unchecked;
                rbUnpaid.CheckState = !leave.Paid ? CheckState.Checked : CheckState.Unchecked;
                //cbPaid.CheckState = LEAVE.paid ? CheckState.Checked : CheckState.Unchecked;
                lblFileName.Text = !docref ? "" : leave.Docref;
                texComments.Text = !docref ? "" : leave.Notes;
                this.Text = $"İzin Güncelleme";
                this.btnAddLeave.Text = "Güncelle";
                texName.Enabled = false;
                texTcno.Enabled = false;
                isUpdate = true;
                if (leave.Docref != null || leave.Docref != "") pnlAddFile.Enabled = false; 
                else pnlAddFile.Enabled = true; 
            }
            else
            {
                this.Text = $"İzin Ekleme";
                this.btnAddLeave.Text = "Ekle";
                //cbPaid.CheckState =  CheckState.Checked;
                rbPaid.CheckState =  CheckState.Checked;
                texName.Enabled = true;
                texTcno.Enabled = true;
                isUpdate = false;
            }
            dcCompanies = new Dictionary<string, int>();
            int counter = 0;
            foreach (Company comp in GlobalVars.Companies)
            {
                dcCompanies.Add(comp.CompanyName, comp.Id);
                if (comp.Id == company.Id) index = counter;
                counter++;
            }
            ddlCompanies.DataSource = dcCompanies;
            ddlCompanies.DisplayMember = "key";
            ddlCompanies.ValueMember = "value";

            ddlCompanies.SelectedIndex = index;

            GlobalVars.Personals = IOC.PersonalDataService.GetAllPersonals(out msg);
            texTcno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            texTcno.AutoCompleteDataSource = (from x in GlobalVars.Personals select x.Tcno).ToList();

            texName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            texName.AutoCompleteDataSource = (from x in GlobalVars.Personals select x.Ads).ToList();
            LockDates();
            lblTimeval.Text = $"{GetTimeVal()} günlük izin ayarlandı";
        }
        private void LockDates()
        {
            ddlCompanies.Enabled = false;
            dtpBirthday.Enabled = false;
            dtpHireDate.Enabled = false;
        }
        private void UnLockDates()
        {
            ddlCompanies.Enabled = true;
            dtpBirthday.Enabled = true;
            dtpHireDate.Enabled = true;
        }
        private void SetControls(long id)
        {
            Personal p = (from x in GlobalVars.Personals where x.Id == id select x).FirstOrDefault();
            texName.Text = p.Ads;
            texTcno.Text = p.Tcno;
            int counter = 0;
            int compId = (from x in GlobalVars.Personals where x.Tcno == p.Tcno select x.Cid).FirstOrDefault();
            foreach (KeyValuePair<string, int> dcc in dcCompanies)
            {
                if (dcc.Value == compId) { index = counter; break; }
                counter++;
            }
            ddlCompanies.SelectedIndex = index;
            dtpBirthday.Value = p.Dtr;
            dtpHireDate.Value = p.Igt;
        }
        private decimal GetTimeVal()
        {
            decimal timevalInfo = 0; toolTipText = "";
            if (cbPublicHoliday.CheckState == CheckState.Checked)
            {
                timevalInfo = Convert.ToInt32((dtpEndDate.Value.Date - dtpStartDate.Value.Date).TotalDays) + 1;
            }
            else
            {
                for (DateTime day = dtpStartDate.Value.Date; day <= dtpEndDate.Value.Date; day = day.AddDays(1))
                {
                    string dayOfWeek = $"{day:dddd}";
                    if (day.DayOfWeek == DayOfWeek.Sunday) { toolTipText += $"{day:dd.MM.yyyy} {dayOfWeek.PadRight(12) } resmi tatil\r\n"; continue; }
                    List<PublicHolidays> phs = (from x in GlobalVars.PublicHolidays where x.Day.Date == day.Date select x).ToList();
                    if (phs == null || phs.Count == 0)
                    {
                        timevalInfo += 1; toolTipText += $"{day:dd.MM.yyyy} {dayOfWeek.PadRight(12) } 1 gün\r\n";
                    }
                    else if (phs != null && phs.Count ==1  && !phs[0].Ft)
                    {
                        timevalInfo += 0.5m; toolTipText += $"{day:dd.MM.yyyy} {dayOfWeek.PadRight(12) } 0.5 gün {phs[0].Desc}\r\n";
                    }
                    else if (phs != null && phs.Count == 1 && phs[0].Ft)
                    {
                        toolTipText += $"{day:dd.MM.yyyy} {dayOfWeek.PadRight(12) } 0 gün {phs[0].Desc}\r\n";
                    }
                    else if (phs != null && phs.Count > 1 )
                    {
                        bool ft = false;
                        foreach (PublicHolidays ph in phs)
                        {
                            if (ph.Ft) { ft = true; toolTipText += $"{day:dd.MM.yyyy} {dayOfWeek.PadRight(12) } 0 gün {phs[0].Desc}\r\n"; break; }
                        }
                        if (!ft) { timevalInfo += 0.5m; toolTipText += $"{day:dd.MM.yyyy} {dayOfWeek.PadRight(12) } 0.5 gün {phs[0].Desc}\r\n"; }
                    }
                }
            }
            toolTipText += $"Toplam: {timevalInfo} gün";
            lblTimeval.Text = $"{timevalInfo} günlük izin ayarlandı";
            return timevalInfo;
        }
        private void SetAddOrUpdate()
        {
            if (isUpdate)
            {
                timevalOfDeletedLeave = leave.Timeval;
                leave.Trxtype = "2";
                leave.Trxdate = DateTime.Now;
                leave.Startdate = dtpStartDate.Value.Date;
                leave.Enddate = dtpEndDate.Value.Date;
                leave.Paid = rbPaid.CheckState == CheckState.Checked ? true : false;
                leave.Timeval = timeval;
                leave.Timeunit = 1;
                leave.Cd = DateTime.Now;
                leave.Docref = filePath;
                leave.Notes = texComments.Text.Trim() + (!leave.Paid? " (Ücretsiz izin)" : "");
                leave.Ph = cbPublicHoliday.Checked;
                leave.Cu = GlobalVars.ActiveUser;
            }
            else
            {
                leave = new Leaves()
                {
                    Trxtype = "1",
                    Pid = (from x in GlobalVars.Personals where x.Tcno == texTcno.Text.Trim() select x.Id).FirstOrDefault(),
                    Cid = cid,
                    Trxdate = DateTime.Now,
                    Startdate = dtpStartDate.Value.Date,
                    Enddate = dtpEndDate.Value.Date,
                    Paid = rbPaid.CheckState == CheckState.Checked ?  true : false, 
                    Timeval = timeval,
                    Timeunit = 1,
                    Cd = DateTime.Now,
                    Docref = filePath,
                    Notes = texComments.Text.Trim() + (rbUnpaid.CheckState == CheckState.Checked ? " (Ücretsiz izin)" : ""),
                    Ph = cbPublicHoliday.Checked,
                    Cu = GlobalVars.ActiveUser
                };
            }
        }
        private bool IsLeaveExists(Leaves leave)
        {
            msg = "";
            alvs = IOC.LeaveDataService.GetLeavesByPersonalId(leave.Pid, out msg);
            if (alvs == null && msg != "") { lblMessage.Text = $"Bir hata oluştu: {msg}"; return false; }
            else if (alvs != null)
            {
                foreach (Leaves alv in alvs)
                {
                    if (leave.Startdate.Date >= alv.Startdate.Date && leave.Startdate.Date <= alv.Enddate.Date || leave.Enddate.Date >= alv.Startdate.Date && leave.Enddate.Date <= alv.Enddate.Date)
                    {
                        DialogResult confirmResult = RadMessageBox.Show($"Eklemeye çalıştığınız izin {alv.Startdate.Date.ToShortDateString()} - {alv.Enddate.Date.ToShortDateString()} tarihleri arasındaki başka bir izinle çakışıyor. Var olan izin güncellensin mi?", "İzin tarihleri çakışması!", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                        if (confirmResult == DialogResult.Yes)
                        {
                            isUpdate = true;
                            leave.Id = alv.Id;
                            leave.Trxtype = "2";
                            return true;
                        }
                        else
                        {
                            lblMessage.Text = "İşlem iptal edildi";
                            return false;
                        }
                    }
                }
            }return false;
        }
        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.MinDate = dtpStartDate.Value;
            lblTimeval.Text = $"{GetTimeVal()} günlük izin ayarlandı";
        }
        private void dtpBirthday_ValueChanged(object sender, EventArgs e)
        {
            dtpHireDate.MinDate = dtpBirthday.Value;
        }
        private void dtpHireDate_ValueChanged(object sender, EventArgs e)
        {
            dtpStartDate.MinDate = dtpHireDate.Value;
        }
        private bool CheckForm()
        {
            Regex rx = new Regex(@"^[a-zA-Z ğüşıöçĞÜŞİÖÇ]{5,50}$");
            string msg;
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

            if (dtpBirthday.Value > dtpHireDate.Value)
            {
                lblMessage.Text = "İşe giriş tarihi, doğum tarihinden önce!"; return false;
            }
            if (dtpStartDate.Value < dtpHireDate.Value)
            {
                lblMessage.Text = "İzin başlangıç tarihi, işe giriş tarihinden önce!"; return false;
            }
            if (dtpStartDate.Value > dtpEndDate.Value)
            {
                lblMessage.Text = "İzin başlangıç tarihi, bitiş tarihinden ileride!"; return false;
            }

            if (unSavedPersonal)
            {
                RadMessageBox.Show($"{texTcno.Text.Trim()} kimlik numarası personel veri tabanında bulunamadı. Lütfen önce personeli ekleyiniz", "Kayıtlı olmayan personel!", MessageBoxButtons.OK, RadMessageIcon.Info);
                lblMessage.Text = $"{texTcno.Text.Trim()} kimlik numarası kayıtlı değil!";
                return false;
            }

            lblMessage.Text = "";
            return true;
        }
        private void texTcno_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            if (texTcno.Text.Trim().Length == 11 && IOC.WinHelpers.IsTcnoValid(texTcno.Text.Trim(), out msg))
            {
                Personal p = (from x in GlobalVars.Personals where x.Tcno == texTcno.Text.Trim() select x).FirstOrDefault();
                if(p == null)
                {
                    unSavedPersonal = true;
                    UnLockDates();
                }
                else
                {
                    unSavedPersonal = false;
                    SetControls(p.Id);
                }
            }

            if (texTcno.Text.Trim().Length < 11)
            {
                LockDates(); 
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
        private void texName_Leave(object sender, EventArgs e)
        {
            if (texName.Text.Trim().Length < 5) return;
            List<string> tcnos = (from x in GlobalVars.Personals where x.Ads == texName.Text.Trim() select x.Tcno).ToList();
            if (tcnos != null && tcnos.Count == 1)
            {
                Personal p = (from x in GlobalVars.Personals where x.Tcno == tcnos[0] select x).FirstOrDefault();
                SetControls(p.Id);
                isMultiName = false;
                unSavedPersonal = false;
            }
            else if(tcnos != null && tcnos.Count > 1)
            {
                isMultiName = true;
                multiName = (from x in GlobalVars.Personals where x.Tcno == tcnos[0] select x.Ads).FirstOrDefault();
                texTcno.AutoCompleteDataSource = (from x in GlobalVars.Personals where x.Ads == multiName select x.Tcno).ToList();
                lblMessage.Text = $"Birden fazla {multiName} var. Lütfen kimlik numarasını giriniz";
                unSavedPersonal = false;
            }
            else
            {
                unSavedPersonal = true;
                texTcno.AutoCompleteDataSource = null;
            }
        }
        private void texTcno_Leave(object sender, EventArgs e)
        {
            if (isMultiName)
            {
                if (texTcno.Text.Trim().Length == 11)
                {
                    Personal p = (from x in GlobalVars.Personals where x.Tcno == texTcno.Text.Trim() select x).FirstOrDefault();
                    if(p == null) { unSavedPersonal = true; UnLockDates();  return; }
                    SetControls(p.Id);
                    UnLockDates();
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            texName.Text = "";
            texTcno.Text = "";
            lblMessage.Text = "";
        }
        private void texName_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }
        private void cbPublicHoliday_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = $"Personelin izinli olduğu günler, resmi tatil günlerine denk gelirse bu günler de iş günü olarak kabul edilir.\r\nBu günler için personele ödeme yapmanız ya da daha sonra fazladan izin hakkı vermeniz gerekebilir.";
        }
        private void cbFileAdd_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            pnlAddFile.Enabled = cbFileAdd.Checked ? true : false;
        }
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFile.Multiselect = false;
            openFile.RestoreDirectory = true;
            openFile.Filter = "Bütün türler|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.txt" +
                "|Word Belgeleri|*.doc;*.docx" +
                "|Excel Sayfaları|*.xls;*.xlsx" +
                "|PDF Dosyaları|*.pdf" +
                "|Resim Dosyaları|*.jpg;*.jpeg;*.png" +
                "|Metin Dosyaları|*.txt";
            openFile.FileName = string.Empty;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFile.FileName;
                filePath = fileName; 
                if (openFile.SafeFileNames.ToList().Count() > 0)
                {
                    fileName = openFile.SafeFileNames.First();
                }
                lblFileName.Text = fileName;
            }
        }
        private void ddlCompanies_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            cid = ((KeyValuePair<string, int>)ddlCompanies.SelectedItem.DataBoundItem).Value;
        }
        private void btnAddLeave_Click(object sender, EventArgs e)
        {
            AddOrUpdateLeaves(); 
        }
        private void AddOrUpdateLeaves()
        {
            if (!CheckForm()) return;
            decimal usedTimevalOfPersonal = 0; // personelin kaydedilmiş ücretli ve avans olmayan izinlerin toplam gün sayısı
            //decimal kisTotal = 0; // periodlardaki kullanılmayan izinlerin toplamı
            timeval = IOC.PersonalService.SetTimeVal(cbPublicHoliday.Checked, new DateRange() { Sd = dtpStartDate.Value.Date, Ed = dtpEndDate.Value.Date });
            SetAddOrUpdate(); // LEAVE burada oluşturuluyor ve isUpdate değişkeni burada ayarlanıyor, form yeni izin butonundan ya da var olan bir iznin detaylarını görmek için basılan detay butonundan geliyor olablir
            personal = (from x in GlobalVars.Personals where x.Id == leave.Pid select x).FirstOrDefault();
            IOC.PersonalService.SetPersonalLeaves( IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg)); // personelin kayıtlı bütün izinlerini al
            
            List<Leaves> leavesSeperated = IOC.PersonalService.IsLeaveSeperated(leave, out msg);
            IOC.PersonalService.SetPersonalLeavePeriods(personal.Tcno, out msg);
            if(leavesSeperated == null) { lblMessage.Text = $"Hata: {msg}"; return; }
            
            DialogResult dialogResult = DialogResult.OK;
            
            if (leavesSeperated.Count > 1) { dialogResult =  RadMessageBox.Show( 
                    $"Eklemeye çalıştığınız izin {leavesSeperated.Count} farklı çalışma dönemine denk geldiği için birbirini takip eden {leavesSeperated.Count} ayrı izin olarak tanımlanması gerekir", 
                    $"İzin {leavesSeperated.Count} farklı çalışma dönemine denk geliyor!", MessageBoxButtons.OK, 
                    RadMessageIcon.Info, $"İzin Tarihi: {dtpStartDate.Value:dd.MM.yyyy} - {dtpEndDate.Value:dd.MM.yyyy}\r\nDÖNEMLER:\r\n{msg}");
                    lblMessage.Text = "İşlem iptal edildi"; return;
            }

            Leaves existLeave; bool willUpd; string mes;
                        
            if (!isUpdate)
            {
                (existLeave, willUpd, mes) = IOC.PersonalService.IsLeaveExists(leave, out msg);
                if (mes.Contains("iptal") || mes.Contains("hata")) { lblMessage.Text = mes; return; }
                leave.Id = existLeave != null ? existLeave.Id : 0; if (leave.Id > 0) { isUpdate = true; timevalOfDeletedLeave = existLeave.Timeval; }
            }

            if (isUpdate)
            {
                int resultDel = IOC.LeaveDataService.DeleteLeave(leave.Id, out msg);
                if (resultDel == -1) { lblMessage.Text = $"Hata: {msg}"; return; }
                personal.Tih += leave.Paid? timevalOfDeletedLeave : 0;
                IOC.PersonalService.RemoveLeaveFromList(leave.Id);
                IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
            }

            List<Leaves> toBeAdded = IOC.PersonalService.CalculateTimeOut(texTcno.Text.Trim(), texName.Text.Trim(), leave);
            
            if(toBeAdded == null) { 
                lblMessage.Text = "İşlem iptal edildi";
                if (isUpdate) { // işlem iptal edildiğinde silinmiş varsa geri yerine koy
                    IOC.LeaveDataService.AddLeave(leave, out msg);
                    personal.Tih -= leave.Paid ? leave.Timeval : 0;
                    IOC.PersonalService.AddLeaveToList(leave);
                    IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
                }
                return; 
            }
            
            int addRes = 0, updRes =  0;
            
            foreach (Leaves alv in toBeAdded)
            {
                usedTimevalOfPersonal = 0; 
                //kisTotal = (from x in IOC.PersonalService.GetPersonalLeavePeriods() select x.Ki).Sum();
                                
                addRes += IOC.LeaveDataService.AddLeave(alv, out msg); 
                usedTimevalOfPersonal = (from x in IOC.PersonalService.GetPersonalLeaves() where x.Paid select x.Timeval).Sum() + (alv.Paid ? alv.Timeval : 0);
                //if(usedTimevalOfPersonal > kisTotal && alv.Paid) { personal.Tih -= usedTimevalOfPersonal - kisTotal; }
                // eski sistemi dene
                personal.Tih -= alv.Timeval;
                // eski sistemi dene
                IOC.PersonalService.AddLeaveToList(alv);
                IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
                updRes = isUpdate ? 1 : 0;
            }

            if (addRes == 0 && updRes == 0) { lblMessage.Text = "Bu izin zaten eklenmiş"; LeavesChanged.HasChanged = false; LeavesChanged.HasChangedForDialog = false; }
            else if (addRes > 0 && updRes == 0) { lblMessage.Text = $"{personal.Tcno} kimlik numaralı personel için izin eklendi"; LeavesChanged.HasChanged = true; LeavesChanged.HasChangedForDialog = true; }
            else if (addRes > 0 && updRes > 0) { lblMessage.Text = $"{personal.Tcno} kimlik numaralı personelin izni güncellendi"; LeavesChanged.HasChanged = true; LeavesChanged.HasChangedForDialog = true; }
            else if (addRes < 0 || updRes < 0) { lblMessage.Text = $"Hata: {msg}"; LeavesChanged.HasChanged = false; LeavesChanged.HasChangedForDialog = false; }
        }
        private void rbUnpaid_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (rbUnpaid.CheckState == CheckState.Checked)
            {
                cbPublicHoliday.CheckState = CheckState.Checked;
                cbPublicHoliday.Enabled = false;
            }
            else
            {
                cbPublicHoliday.CheckState = CheckState.Unchecked;
                cbPublicHoliday.Enabled = true;
            }
        }
        private void rbUnpaid_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = "Personelin yıllık izin hakkı, 365 gün üzerinden hesaplanmaz.\r\n365'ten ücretsiz izin günlerinin toplamı düşüldükten sonra kalan gün sayısı üzerinden hesaplanır.";
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            lblTimeval.Text = $"{GetTimeVal()} günlük izin ayarlandı";
        }

        private void pbTimevalInfo_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.Offset = new Size(5, 1);
            e.ToolTipText = toolTipText;
        }
    }
}
