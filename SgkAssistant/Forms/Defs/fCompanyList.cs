using Models.Common;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Forms.Defs
{
    public partial class FCompanyList : Telerik.WinControls.UI.RadForm
    {
        Company c = new Company();
        List<Company> toUpdate = null;
        GridViewRowInfo row;
        public FCompanyList()
        {
            
            RadGridLocalizationProvider.CurrentProvider = new Localization();
            RadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            InitializeComponent();
        }

        private void fCompanyList_Load(object sender, EventArgs e)
        {
            btnExport.Enabled = (rgvCompanyList.RowCount > 0) ? true : false;
            btnImport.Enabled = (rgvCompanyList.RowCount > 0) ? true : false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            this.CancelButton = btnClose; 
            List();
            rgvCompanyList.ViewRowFormatting += RgvHelpers.GridViews_ViewRowFormatting;
            rgvCompanyList.ViewCellFormatting += RgvHelpers.GridViews_ViewCellFormatting;
            rgvCompanyList.CellFormatting += RgvHelpers.GridViews_CellFormatting;
        }

        private void List()
        {
            string msg = "";
            bool yuklendi = IOC.WinHelpers.SetCompanyListRgv(rgvCompanyList, out msg);
            if (!yuklendi)
            {
                DialogResult dr = RadMessageBox.Show($"Veri tabanından firma listesi yüklenmesi başarısız oldu ({msg})\r\nTekrar denemek için \"Tekrar Dene\" butonuna basın", "Firma Listesi Yüklenemedi!", MessageBoxButtons.RetryCancel);
                if (dr == DialogResult.Retry)
                {
                    List();
                }
                else
                {
                    Application.Exit();
                }

            }
            if (rgvCompanyList.DataSource != null && rgvCompanyList.RowCount > 0)
            {
                rgvCompanyList.Columns[14].DataType = typeof(DateTime);
                rgvCompanyList.Columns[14].FormatString = "{0: dd.MM.yyyy}";
                rgvCompanyList.Columns[15].DataType = typeof(DateTime);
                rgvCompanyList.Columns[15].FormatString = "{0: dd.MM.yyyy}";
                rgvCompanyList.Columns[22].DataType = typeof(DateTime);
                rgvCompanyList.Columns[22].FormatString = "{0: dd.MM.yyyy}";

                rgvCompanyList.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                rgvCompanyList.Columns[0].MaxWidth = 70; rgvCompanyList.Columns[0].MinWidth = 70;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FCompany f = new FCompany();
            f.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            c = GetCompanyFromRgv();
            FCompany f = new FCompany(c);
            f.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string msg = "",  sgscEnc = "";
            try
            {
                row = rgvCompanyList.CurrentRow;
                c = IOC.CompanyDataService.GetCompanyById(Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[23].Value), out msg);
                //if (c.Kkc.Year > DateTime.Now.Year) { lblMessage.Text = $"Aktif durumdaki firma silinemez!"; lblMessage.Visibility = ElementVisibility.Visible; btnOpenFile.Visibility = ElementVisibility.Collapsed; return; }
                bool allowDelete = false; int result = 0;
                var confirmResult = RadMessageBox.Show($"{c.CompanyName} adlı firma silinecek", "Silme işlemini onayla", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    rgvCompanyList.ClearSelection(); return;
                }
                else
                {
                    allowDelete = true;
                }
                if (allowDelete == true)
                {
                    toUpdate = IOC.CompanyDataService.GetCompaniesByFm(c.Id, out msg);
                    sgscEnc = Encrypt.EncryptString(c.Sgsc, Settings.Default.discid);
                    result = IOC.CompanyDataService.DeleteCompany(c, sgscEnc, out msg);
                }
                if (result == 1)
                {
                    cbreMessage.Visibility = ElementVisibility.Visible; cbbUndoDelete.Visibility = ElementVisibility.Visible;
                    cblMessage.Text = $"{c.CompanyName} adlı firma silindi";
                    Changed.HasChanged = true; Changed.HasChangedForDialogBox = true;
                    rgvCompanyList.Rows.Remove(row);
                    rgvCompanyList.Refresh();
                    cbbUndoDelete.Enabled = true;
                    IOC.WinHelpers.ResetSgsc(out msg);
                }
                else
                {
                    cblMessage.Text = $"Veritabanı hatası! Lütfen tekrar deneyiniz";
                    cbbUndoDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                cblMessage.Text = $"Silme işlemi başarısız! Hata: {msg}";
            }
        }

        private void cbbUndoDelete_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                int lastId = IOC.CompanyDataService.GetLastId(out msg);
                c.Fm = lastId + 1;
                int result = IOC.CompanyDataService.AddCompany(c, PackageHelper.Mcc, out msg);
                if (result == 1)
                {
                    cblMessage.Text = $"{c.CompanyName} adlı firma tekrar eklendi";
                    

                    Changed.HasChanged = true; Changed.HasChangedForDialogBox = true;
                    lastId = IOC.CompanyDataService.GetLastId(out msg);
                    foreach (Company comp in toUpdate)
                    {
                        IOC.CompanyDataService.UpdateFm(comp, lastId, out msg);
                    }
                    IOC.WinHelpers.ResetSgsc(out msg);
                    List();
                    rgvCompanyList.Refresh();
                    cbbUndoDelete.Enabled = false;
                    
                }
                else
                {
                    cblMessage.Text = $"Veritabanı hatası, lütfen tekrar deneyiniz";
                    cbbUndoDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                cblMessage.Text = $"Geri alma işlemi başarısız! Hata: {msg}";
            }
        }
        
        private void cbbCloseMessage_Click(object sender, EventArgs e)
        {
            cbreMessage.Visibility = ElementVisibility.Collapsed;
        }

        private void rgvCompanyList_MouseEnter(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                if (Changed.HasChangedForDialogBox == true)
                {
                    List();
                    Changed.HasChangedForDialogBox = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        private void rgvCompanyList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[0].Value);
                if (rgvCompanyList.SelectedRows.Count == 1 && id == 1)
                {
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = false;
                }
                else if (rgvCompanyList.SelectedRows.Count == 1 && id != 1)
                {
                    btnDelete.Enabled = true;
                    btnEdit.Enabled = true;
                }
                else
                {
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                }
            }
            catch (Exception)
            {
                return;
            }
            
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            FCompanyAdd f = new FCompanyAdd();
            f.ShowDialog();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            lblMessage.Visibility  = ElementVisibility.Collapsed;
            string msg = "";
            string title = $"{"FİRMA LİSTESİ"}";
            bool result = false;
            List<Company> lst = new List<Company>();
            lst = GlobalVars.Companies;// IOC.winHelpers.GetAllCompaniesFromRgv(rgvCompanyList, out msg);
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (rgvCompanyList.RowCount > 0)
                {
                   result = IOC.ExportService.CreateFileForCompanies(title, lst, out msg);
                    
                }
                if (result)
                {
                    if(IOC.ExportService.Save("Firma Listesi",out msg))
                    {
                        lblMessage.Visibility = ElementVisibility.Visible;
                        btnOpenFile.Visibility = ElementVisibility.Visible;
                    }
                    else
                    {
                        btnOpenFile.Visibility = ElementVisibility.Collapsed;
                    }
                    
                }
                lblMessage.Text = msg;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Dosya oluşturulamadı! Hata: {ex.Message.ToString()}"; btnOpenFile.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            }
        }

        private void commandBarCompanyList_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            if (rgvCompanyList.RowCount > 0)
            {
                int id = Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[0].Value);
                if (rgvCompanyList.SelectedRows.Count == 1 && id == 1)
                    e.ToolTipText = "1 numaralı firma merkez olmalıdır ve silinemez;\r\nancak detay butonuna basarak bilgilerini değiştirebilirsiniz.";
                btnImport.Enabled = true;
            }
            else if (rgvCompanyList.RowCount == 0)
            {
                e.ToolTipText = "Yeni butona basıp ilk firmayı merkez olarak\r\nekledikten sonra toplu firma ekleme yapabilirsiniz.";
                btnImport.Enabled = false;
            }
        }

        private void rgvCompanyList_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            btnExport.Enabled = (rgvCompanyList.RowCount > 0) ? true : false;
            btnImport.Enabled = (rgvCompanyList.RowCount > 0) ? true : false;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
        private Company GetCompanyFromRgv()
        {
            int i = 1;
            Company comp = new Company();
            comp.CompanyName = rgvCompanyList.CurrentRow.Cells[i++].Value.ToString();
            comp.CompanyId = rgvCompanyList.CurrentRow.Cells[i++].Value.ToString();
            comp.CompanyId2 = rgvCompanyList.CurrentRow.Cells[i++].Value.ToString();
            comp.SystemPassword = rgvCompanyList.CurrentRow.Cells[i++].Value.ToString();
            comp.CompanyPassword = rgvCompanyList.CurrentRow.Cells[i++].Value.ToString();
            comp.Fm = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[i].Value) : 1; i++;
            comp.Gun = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Gp = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Gs = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Sgsc = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Unvan = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Adres = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Sgm = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Kka = DateTime.Parse(rgvCompanyList.CurrentRow.Cells[i++].Value.ToString());
            comp.Kkc = DateTime.Parse(rgvCompanyList.CurrentRow.Cells[i++].Value.ToString());
            comp.Sc1 = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Sc2 = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Sc3 = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Sc4 = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Sc5 = (rgvCompanyList.CurrentRow.Cells[i].Value != null) ? rgvCompanyList.CurrentRow.Cells[i].Value.ToString() : String.Empty; i++;
            comp.Cu = Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[i++].Value);
            comp.Cd = DateTime.Parse(rgvCompanyList.CurrentRow.Cells[i++].Value.ToString());
            comp.Id = Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[i++].Value);
            return comp;
        }
        private void rgvCompanyList_FilterExpressionChanged(object sender, FilterExpressionChangedEventArgs e)
        {
            e.FilterExpression = e.FilterExpression.ToUpper();
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
