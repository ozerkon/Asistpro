using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Forms.Defs
{
    public partial class FPersonalList : RadForm
    {
        private Personal personal = new Personal();
        private Company company = new Company();
        private int index = 0;
        private string msg = "";
        private GridDataCellElement hoverCell;
        private GridViewRowInfo row;
        private RadContextMenu perContex;
        private RadMenuItem copyCell, copyRow, copyAllRow, detail, delete, leaves, addLeave;
        public FPersonalList(Company company)
        {
            this.company = company; 
            RadGridLocalizationProvider.CurrentProvider = new Localization();
            RadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            InitializeComponent();
        }
        private void fPersonalList_Load(object sender, EventArgs e)
        {
            btnExport.Enabled = (rgvPersonalList.RowCount > 0) ? true : false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            this.CancelButton = btnClose;
            Dictionary<string, int> dcCompanies = new Dictionary<string, int>();
            int counter = 0;
            dcCompanies.Add("--HEPSİ--", 0);
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

            pnlMessage.Visible = false;
            pnlMessage.Height = 0;
            List();
            rgvPersonalList.ViewRowFormatting += RgvHelpers.GridViews_ViewRowFormatting;
            rgvPersonalList.ViewCellFormatting += RgvHelpers.GridViews_ViewCellFormatting;
            rgvPersonalList.CellFormatting += RgvHelpers.GridViews_CellFormatting;
            perContex   = new RadContextMenu();
            copyCell    = new RadMenuItem("Hücreyi Kopyala", "copyCell");   perContex.Items.Add(copyCell);  copyCell.Click += PerContext_Click;
            copyRow     = new RadMenuItem("Satırı Kopyala", "copyRow");    perContex.Items.Add(copyRow);   copyRow.Click  += PerContext_Click;
            copyAllRow  = new RadMenuItem("Bütün Satırları Kopyala", "copyAllRow"); perContex.Items.Add(copyAllRow); copyAllRow.Click += PerContext_Click;
            detail      = new RadMenuItem("Personel Bilgilerini Göster", "detail");     perContex.Items.Add(detail);    detail.Click += PerContext_Click;
            delete      = new RadMenuItem("Personeli Sil", "delete");     perContex.Items.Add(delete);    delete.Click += PerContext_Click;
            leaves      = new RadMenuItem("İzinlerini Listele", "leaves");     perContex.Items.Add(leaves);    leaves.Click += PerContext_Click;
            addLeave    = new RadMenuItem("İzin Ekle", "addLeave");   perContex.Items.Add(addLeave);  addLeave.Click += PerContext_Click;
        }
        private void PerContext_Click(object sender, EventArgs e)
        {
            if (sender == copyCell)
            {
                Type cellType = rgvPersonalList.CurrentCell.Value != null ? rgvPersonalList.CurrentCell.Value.GetType() : null; if (cellType == null) return;
                string val = "";
                if (cellType.Name == "DateTime")
                {
                    val = Convert.ToDateTime(rgvPersonalList.CurrentCell.Value).ToString("dd.MM.yyyy");
                }
                else if (cellType.Name == "Boolean")
                {
                    val = Convert.ToBoolean(rgvPersonalList.CurrentCell.Value) == true ? "Evet" : "Hayır";
                }
                else
                {
                    val = rgvPersonalList.CurrentCell.Value.ToString();
                }

                Clipboard.SetText(val);
            }
            else if (sender == copyRow)
            {
                string val = "TC Kimlik No\tAdı Soyadı\tDoğum Tarihi\tİşe Giriş Tarihi\tİşten Çıkış Tarihi\tÖnceki Dönemlerden\r\nDevreden İzin Gün Sayısı\r\n";
                GridViewRowInfo row = rgvPersonalList.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;

                foreach (GridViewCellInfo cellInfo in row.Cells)
                {
                    if ((row.Cells.Count == 9 && (cellInfo.ColumnInfo.FieldName == "id" || cellInfo.ColumnInfo.FieldName == "cid" || cellInfo.ColumnInfo.FieldName == "active")) || (row.Cells.Count == 8 && (cellInfo.ColumnInfo.FieldName == "id" || cellInfo.ColumnInfo.FieldName == "tcno"))) continue;

                    Type cellType = cellInfo.Value != null ? cellInfo.Value.GetType() : null;

                    if (cellType == null)
                    {
                        val += "\t";
                    }
                    else if (cellType.Name == "DateTime")
                    {
                        if (cellInfo.ColumnInfo.FieldName == "Ict" && Convert.ToBoolean(row.Cells["Active"].Value) == false)
                        {
                            val += Convert.ToDateTime(cellInfo.Value).ToString("dd.MM.yyyy") + "\t";
                        }
                        else if (cellInfo.ColumnInfo.FieldName == "Ict" && Convert.ToBoolean(row.Cells["Active"].Value) == true)
                        {
                            val += "\t";
                        }
                        else
                        {
                            val += Convert.ToDateTime(cellInfo.Value).ToString("dd.MM.yyyy") + "\t";
                        }
                    }
                    else if (cellType.Name == "Boolean")
                    {
                        val += Convert.ToBoolean(cellInfo.Value) == true ? "Evet" + "\t" : "Hayır" + "\t";
                    }
                    else
                    {
                        val += cellInfo.Value.ToString() + "\t";
                    }

                }
                val = val.Remove(val.Length - 1, 1);
                Clipboard.SetText(val);
            }
            else if (sender == copyAllRow)
            {
                string val = "TC Kimlik No\tAdı Soyadı\tDoğum Tarihi\tİşe Giriş Tarihi\tİşten Çıkış Tarihi\tÖnceki Dönemlerden\r\nDevreden İzin Gün Sayısı\r\n";
                GridViewRowInfo row = rgvPersonalList.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;

                GridViewChildRowCollection satirlar = row.Cells.Count == 9 ? rgvPersonalList.CurrentCell.ViewTemplate.MasterViewInfo.Rows : rgvPersonalList.CurrentCell.RowInfo.ViewInfo.ChildRows;
                foreach (GridViewRowInfo item in satirlar)
                {
                    foreach (GridViewCellInfo cellInfo in item.Cells)
                    {
                        if ((item.Cells.Count == 9 && (cellInfo.ColumnInfo.FieldName == "id" || cellInfo.ColumnInfo.FieldName == "cid" || cellInfo.ColumnInfo.FieldName == "active")) || (item.Cells.Count == 8 && (cellInfo.ColumnInfo.FieldName == "id" || cellInfo.ColumnInfo.FieldName == "tcno"))) continue;

                        Type cellType = cellInfo.Value != null ? cellInfo.Value.GetType() : null;

                        if (cellType == null)
                        {
                            val += "\t";
                        }
                        else if (cellType.Name == "DateTime")
                        {
                            if (cellInfo.ColumnInfo.FieldName == "Ict" && Convert.ToBoolean(item.Cells["Active"].Value) == false)
                            {
                                val += Convert.ToDateTime(cellInfo.Value).ToString("dd.MM.yyyy") + "\t";
                            }
                            else if (cellInfo.ColumnInfo.FieldName == "Ict" && Convert.ToBoolean(item.Cells["Active"].Value) == true)
                            {
                                val += "\t";
                            }
                            else
                            {
                                val += Convert.ToDateTime(cellInfo.Value).ToString("dd.MM.yyyy") + "\t";
                            }
                        }
                        else if (cellType.Name == "Boolean")
                        {
                            val += Convert.ToBoolean(cellInfo.Value) == true ? "Evet" + "\t" : "Hayır" + "\t";
                        }
                        else
                        {
                            val += cellInfo.Value.ToString() + "\t";
                        }
                    }
                    val = val.Remove(val.Length - 1, 1);
                    val += "\r\n";
                }
                Clipboard.SetText(val);

            }
            else if (sender == detail) btnEdit_Click(btnEdit, new EventArgs());
            else if (sender == delete) btnDelete_Click(btnDelete, new EventArgs());
            else if (sender == leaves)
            {
                personal = GetPersonalFromRgv();
                SearchReport.KimlikNo = personal.Tcno;
                GlobalVars.LeavesForDialog = IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg);
                FLeaveList f = new FLeaveList(personal);
                f.ShowDialog();
            }else if (sender == addLeave)
            {
                personal = GetPersonalFromRgv();
                FLeave f = new FLeave(company, null, personal);
                f.ShowDialog();
            }
        }
        private void List()
        {
            bool yuklendi = RgvHelpers.SetPersonalListRgv(rgvPersonalList, out msg);
            if (!yuklendi)
            {
                DialogResult dr = RadMessageBox.Show($"Veri tabanından personel listesi yüklenmesi başarısız oldu ({msg})\r\nTekrar denemek için \"Tekrar Dene\" butonuna basın", "Personel Listesi Yüklenemedi!", MessageBoxButtons.RetryCancel);
                if (dr == DialogResult.Retry)
                {
                    List();
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }

            if (rgvPersonalList.DataSource != null && rgvPersonalList.RowCount > 0)
            {
                rgvPersonalList.Columns[5].DataType = typeof(DateTime);
                rgvPersonalList.Columns[5].FormatString = "{0: dd.MM.yyyy}";
                rgvPersonalList.Columns[6].DataType = typeof(DateTime);
                rgvPersonalList.Columns[6].FormatString = "{0: dd.MM.yyyy}";
                rgvPersonalList.Columns[7].DataType = typeof(DateTime);
                rgvPersonalList.Columns[7].FormatString = "{0: dd.MM.yyyy}";

                rgvPersonalList.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FPersonal f = new FPersonal(company.Id);
            f.ShowDialog();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            personal = GetPersonalFromRgv();
            FPersonal f = new FPersonal(personal.Cid, personal);
            f.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string msg;
            try
            {
                row = rgvPersonalList.CurrentRow;
                personal = GetPersonalFromRgv();

                bool allowDelete = false; int result = 0;
                var confirmResult = RadMessageBox.Show($"{personal.Ads} adlı personel silinecek!", "Silme işlemini onayla", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    rgvPersonalList.ClearSelection(); return;
                }
                else
                {
                    allowDelete = true;
                }
                if (allowDelete == true)
                {
                    result = IOC.PersonalDataService.DeletePersonal(personal.Id, out msg);
                }
                if (result == 1)
                {
                    cbreMessage.Visibility = ElementVisibility.Visible; cbbUndoDelete.Visibility = ElementVisibility.Visible;
                    cblMessage.Text = $"{personal.Ads} adlı personel silindi";
                    Changed.HasChanged = true; Changed.HasChangedForDialogBox = true;
                    rgvPersonalList.Rows.Remove(row);
                    rgvPersonalList.Refresh();
                    cbbUndoDelete.Enabled = true;
                    
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
                
                int result = IOC.PersonalDataService.AddPersonal(personal, out msg);
                if (result == 1)
                {
                    cblMessage.Text = $"{personal.Tcno} adlı personel tekrar eklendi";
                    PersonalChange.HasChanged = true; PersonalChange.HasChangedForDialogBox = true;
                    List();
                    rgvPersonalList.Refresh();
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
            pnlMessage.Visible = false;
            pnlMessage.Height = 0;
        }
        private void rgvPersonalList_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (PersonalChange.HasChangedForDialogBox == true)
                {
                    List();
                    PersonalChange.HasChangedForDialogBox = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Hata: {msg}";
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            FPersonalAdd f = new FPersonalAdd(company.Id);
            f.ShowDialog();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            lblMessage.Visibility  = ElementVisibility.Collapsed;
            string msg = "";
            string title = $"{GlobalVars.IzinComp.CompanyName} PERSONEL LİSTESİ";
            bool result = false;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (rgvPersonalList.RowCount > 0)
                {
                   result = IOC.ExportService.CreateFileForPersonals(title, GlobalVars.Personals, out msg);
                    
                }
                if (result)
                {
                    if(IOC.ExportService.Save($"{GlobalVars.IzinComp.CompanyName} Personel Listesi", out msg))
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
                lblMessage.Text = $"Dosya oluşturulamadı! Hata: {ex.Message}"; btnOpenFile.Visibility = ElementVisibility.Collapsed;
            }
        }
        private void rgvPersonalList_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            btnExport.Enabled = (rgvPersonalList.RowCount > 0) ? true : false;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
        private Personal GetPersonalFromRgv()
        {
            int i = 1;
            Personal personal = new Personal();
            personal.Id = Convert.ToInt32(rgvPersonalList.CurrentRow.Cells[i++].Value);
            personal.Tcno = rgvPersonalList.CurrentRow.Cells[i++].Value.ToString();
            personal.Ads = rgvPersonalList.CurrentRow.Cells[i++].Value.ToString();
            personal.Cid = Convert.ToInt32(rgvPersonalList.CurrentRow.Cells[i++].Value);
            personal.Dtr = DateTime.Parse(rgvPersonalList.CurrentRow.Cells[i++].Value.ToString());
            personal.Igt = DateTime.Parse(rgvPersonalList.CurrentRow.Cells[i++].Value.ToString());
            personal.Ict = DateTime.Parse(rgvPersonalList.CurrentRow.Cells[i++].Value.ToString());
            personal.Tih = Convert.ToDecimal(rgvPersonalList.CurrentRow.Cells[i++].Value.ToString());
            personal.Active = Convert.ToBoolean(rgvPersonalList.CurrentRow.Cells[i++].Value.ToString());
            return personal;
        }
        private void rgvPersonalList_FilterExpressionChanged(object sender, FilterExpressionChangedEventArgs e)
        {
            e.FilterExpression = e.FilterExpression.ToUpper();
        }
        private void cblMessage_TextChanged(object sender, EventArgs e)
        {
            if (cblMessage.Text == string.Empty)
            {
                cbreMessage.Visibility = ElementVisibility.Collapsed;
                pnlMessage.Visible = false;
                pnlMessage.Height = 0;
            }
            else
            {
                cbreMessage.Visibility = ElementVisibility.Visible;
                pnlMessage.Visible = true;
                pnlMessage.Height = 46;
            }
        }
        private void ddlCompanies_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            company.Id = ((KeyValuePair<string, int>)ddlCompanies.SelectedItem.DataBoundItem).Value;
            if (company.Id != 0) GlobalVars.IzinComp = (from x in GlobalVars.Companies where x.Id == company.Id select x).FirstOrDefault();
            else { GlobalVars.IzinComp = new Company() { Id = 0 }; }
            List();
            Text = $"PERSONEL LİSTESİ - {GlobalVars.IzinComp.CompanyName}";
        }
        private void rgvPersonalList_SelectionChanged(object sender, EventArgs e)
        {
            if (rgvPersonalList.SelectedRows.Count == 1 )
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
        private void rgvPersonalList_MouseLeave(object sender, EventArgs e)
        {
            foreach (GridViewRowInfo rowInfo in rgvPersonalList.Rows) { rowInfo.Tag = ""; }
        }
        private void rgvPersonalList_MouseMove(object sender, MouseEventArgs e)
        {
            if (hoverCell != null && hoverCell.RowInfo != null)
            {
                hoverCell.RowInfo.Tag = null;
                hoverCell.RowInfo.InvalidateRow();
            }

            GridDataCellElement dataCellElement = rgvPersonalList.ElementTree.GetElementAtPoint(e.Location) as GridDataCellElement;
            if (dataCellElement != null)
            {
                dataCellElement.RowInfo.Tag = "HoverMeFlag";
                hoverCell = dataCellElement;
                dataCellElement.RowInfo.InvalidateRow();
            }
        }
        private void rgvPersonalList_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if (rgvPersonalList.CurrentCell != null && rgvPersonalList.CurrentCell.RowIndex != -1 )
            {
                GridViewRowInfo row = rgvPersonalList.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;
                if (row == null) return;
                if (row.Index >= 0) e.ContextMenu = perContex.DropDown;
            }
        }
    }
}
