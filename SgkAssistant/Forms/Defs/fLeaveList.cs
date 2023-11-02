using Models.Common;
using Models.Domain;
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
    public partial class FLeaveList : RadForm
    {
        private Personal personal = new Personal();
        private Company company = new Company();
        private DateRange dateRange = new DateRange();
        private Leaves leave;
        private string msg = "";
        GridViewRowInfo row;
        private GridDataCellElement hoverCell;
        public FLeaveList(Personal personal, DateRange dateRange = null)
        {
            this.personal = personal;
            this.dateRange = dateRange;
            RadGridLocalizationProvider.CurrentProvider = new Localization();
            RadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            InitializeComponent();
        }

        private void fLeaveList_Load(object sender, EventArgs e)
        {
            btnExport.Enabled = (rgvLeaveList.RowCount > 0) ? true : false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            this.CancelButton = btnClose;
            this.Text =  $"{personal.Ads} adlı personelin izin listesi";
            pnlMessage.Visible = false;
            pnlMessage.Height = 0;
            List();
            rgvLeaveList.ViewRowFormatting += RgvHelpers.GridViews_ViewRowFormatting;
            rgvLeaveList.ViewCellFormatting += RgvHelpers.GridViews_ViewCellFormatting;
            rgvLeaveList.CellFormatting += RgvHelpers.GridViews_CellFormatting;
        }
        private void List()
        {
            bool yuklendi = RgvHelpers.SetLeaveListRgvForPersonal(rgvLeaveList, out msg);
            if (!yuklendi)
            {
                DialogResult dr = RadMessageBox.Show($"Veri tabanından personel izin listesi yüklenmesi başarısız oldu ({msg})\r\nTekrar denemek için \"Tekrar Dene\" butonuna basın", "Personel Listesi Yüklenemedi!", MessageBoxButtons.RetryCancel);
                if (dr == DialogResult.Retry)
                {
                    List();
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }

            if (rgvLeaveList.DataSource != null && rgvLeaveList.RowCount > 0)
            {
                rgvLeaveList.Columns[4].DataType = typeof(DateTime);
                rgvLeaveList.Columns[4].FormatString = "{0: dd.MM.yyyy}";
                rgvLeaveList.Columns[5].DataType = typeof(DateTime);
                rgvLeaveList.Columns[5].FormatString = "{0: dd.MM.yyyy}";
                rgvLeaveList.Columns[7].DataType = typeof(decimal);
                rgvLeaveList.Columns[7].FormatString = "{0:0.0}";
                rgvLeaveList.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FLeave f = new FLeave(company, null, personal);
            f.ShowDialog();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            leave = GetLeaveFromRgv(rgvLeaveList.CurrentRow);
            FLeave f = new FLeave(company, leave);
            f.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string msg; 
            try
            {
                row = rgvLeaveList.CurrentRow;
                leave = GetLeaveFromRgv(rgvLeaveList.CurrentRow);

                bool allowDelete = false; int result = 0;
                var confirmResult = RadMessageBox.Show($@"{personal.Ads} adlı personelin {leave.Startdate:dd.MM.yyyy} - {leave.Enddate:dd.MM.yyyy} arasındaki {leave.Timeval} günlük izni silinecek!", "Silme işlemini onayla", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    rgvLeaveList.ClearSelection(); return;
                }
                else
                {
                    allowDelete = true;
                }
                if (allowDelete == true)
                {
                    result = IOC.LeaveDataService.DeleteLeave(leave.Id, out msg);
                }
                if (result == 1)
                {
                    cbreMessage.Visibility = ElementVisibility.Visible; cbbUndoDelete.Visibility = ElementVisibility.Visible;
                    cblMessage.Text = $"{personal.Ads} adlı personelin {leave.Startdate:dd.MM.yyyy}-{leave.Enddate:dd.MM.yyyy} arasındaki {leave.Timeval} günlük izni silindi";
                    if (leave.Paid) { personal.Tih += leave.Timeval; }
                    LeavesChanged.HasChanged = true;
                    PersonalChange.HasChanged = true;
                    LeavesChanged.HasChangedForDialog = true;
                    IOC.PersonalService.RemoveLeaveFromList(leave.Id);
                    IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
                    rgvLeaveList.Rows.Remove(row);
                    rgvLeaveList.Refresh();
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
            string msg;
            try
            {
                int result = IOC.LeaveDataService.AddLeave(leave, out msg);
                if (result == 1)
                {
                    cblMessage.Text = $"{personal.Ads} adlı personelin {leave.Startdate:dd.MM.yyyy}-{leave.Enddate:dd.MM.yyyy} arasındaki {leave.Timeval} günlük izni tekrar eklendi";
                    if (leave.Paid) { personal.Tih -= leave.Timeval; }
                    LeavesChanged.HasChanged = true;
                    PersonalChange.HasChanged = true;
                    LeavesChanged.HasChangedForDialog = true;
                    IOC.PersonalService.AddLeaveToList(leave);
                    IOC.PersonalService.UpdatePersonalAfterLeavesAdd(personal, out msg);
                    List();
                    rgvLeaveList.Refresh();
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
        private void rgvLeaveList_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (LeavesChanged.HasChangedForDialog == true)
                {
                    GlobalVars.LeavesForDialog = IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg);
                    List();
                    LeavesChanged.HasChangedForDialog = false;
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
            FLeavesAdd f = new FLeavesAdd(personal);
            f.ShowDialog();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            lblMessage.Visibility  = ElementVisibility.Collapsed;
            string msg = "";
            string title = $"{personal.Ads}, İZİN LİSTESİ";
            bool result = false;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                List<Leaves> lst = new List<Leaves>();
                foreach (GridViewRowInfo row in rgvLeaveList.Rows)
                {
                    leave = GetLeaveFromRgv(row);
                    lst.Add(leave);
                }
                if (rgvLeaveList.RowCount > 0)
                {
                   result = IOC.ExportService.CreateFileForLeaves(title, lst, out msg);
                    
                }
                if (result)
                {
                    if(IOC.ExportService.Save($"{personal.Ads} İzin Listesi", out msg))
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
        private void rgvLeaveList_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            btnExport.Enabled = (rgvLeaveList.RowCount > 0) ? true : false;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
        private Leaves GetLeaveFromRgv(GridViewRowInfo row)
        {
            long i = 0;
            Leaves alv = new Leaves();
            i = Convert.ToInt64(row.Cells[0].Value);
            alv = (from x in GlobalVars.LeavesForDialog where x.Id == i select x).FirstOrDefault();
            return alv;
        }
        private void rgvLeaveList_FilterExpressionChanged(object sender, FilterExpressionChangedEventArgs e)
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
        private void rgvLeaveList_CellFormatting(object sender, CellFormattingEventArgs e)
        {
                if (e.Column.Index == 11 && (e.Row.Cells[10].Value == null || e.Row.Cells[10].Value.ToString() == "")) 
                    e.CellElement.Enabled = false;
                else e.CellElement.Enabled = true;
            
                if (e.CellElement.ColumnInfo.Name == "Timeval")
                {
                    e.CellElement.Padding = new Padding(0, 0, 20, 0);
                }

            if (e.CellElement is GridDataCellElement && e.CellElement.RowInfo.Tag != null && e.CellElement.RowInfo.Tag.ToString() == "HoverMeFlag")
            {
                e.CellElement.BackColor = Color.Red;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.DrawFill = true;
            }
            //else if (e.Row.IsSelected || (e.CellElement.RowInfo.Tag != null && e.CellElement.RowInfo.Tag.ToString() == "SelectMeFlag"))
            //{
            //    e.CellElement.DrawFill = true;
            //    e.CellElement.ForeColor = Color.White;
            //    e.CellElement.GradientStyle = GradientStyles.Solid;
            //    e.CellElement.BackColor = Color.Black;
            //    e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
            //    e.CellElement.BorderLeftColor = Color.White;
            //    e.CellElement.BorderRightColor = Color.White;
            //}
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BorderBoxStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BorderRightColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BorderLeftColorProperty, ValueResetFlags.Local);
                e.CellElement.ForeColor = Color.Black;
            }
        }
        private void rgvLeaveList_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            GridViewRowInfo row = rgvLeaveList.CurrentRow;
            string location = row.Cells[9].Value != null ? row.Cells[9].Value.ToString() : "";
            if (location != "") Process.Start(location);
        }
        private void rgvLeaveList_SelectionChanged(object sender, EventArgs e)
        {
            if (rgvLeaveList.SelectedRows.Count == 1)
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

        private void rgvLeaveList_MouseMove(object sender, MouseEventArgs e)
        {
            if (hoverCell != null && hoverCell.RowInfo != null)
            {
                hoverCell.RowInfo.Tag = null;
                hoverCell.RowInfo.InvalidateRow();
            }

            GridDataCellElement dataCellElement = rgvLeaveList.ElementTree.GetElementAtPoint(e.Location) as GridDataCellElement;
            if (dataCellElement != null && dataCellElement.RowInfo != null)
            {
                dataCellElement.RowInfo.Tag = "HoverMeFlag";
                hoverCell = dataCellElement;
                dataCellElement.RowInfo.InvalidateRow();
            }
        }

        private void rgvLeaveList_MouseLeave(object sender, EventArgs e)
        {
            foreach (GridViewRowInfo rowInfo in rgvLeaveList.Rows) { rowInfo.Tag = ""; }
        }
    }
}
