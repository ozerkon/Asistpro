using Models.Common;
using Models.Domain;
using SgkAssistant.Forms.Defs;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Helpers
{
    public static class RgvHelpers
    {
        public static string Mesaj = "";
        public static RadGridView RgvLeaves;
        public static void GridViews_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is GridDataCellElement && e.CellElement.RowInfo.Tag != null && e.CellElement.RowInfo.Tag.ToString() == "HoverMeFlag")
            {
                e.CellElement.BackColor = Color.Red;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.DrawFill = true;
            }
            else if (e.Row.IsSelected)
            {
                e.CellElement.DrawFill = true;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.BackColor = Color.Black;
                e.CellElement.ForeColor = Color.White;
                e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.CellElement.BorderLeftColor = Color.White;
                e.CellElement.BorderRightColor = Color.White;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BorderBoxStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BorderRightColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BorderLeftColorProperty, ValueResetFlags.Local);
            }
            if (e.CellElement.ColumnInfo.Name == "Ads")
            {
                e.CellElement.Padding = new Padding(5, 0, 0, 0);
            }
            else if (e.CellElement.ColumnInfo.Name == "Tih")
            {
                e.CellElement.Padding = new Padding(0, 0, 100, 0);
            }
        }
        public static void GridViews_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement is GridSummaryRowElement)
            {
                e.RowElement.RowInfo.Height = 30;
            }
            if (e.RowElement is GridFilterRowElement)
            {
                e.RowElement.RowInfo.Height = 34;
                e.RowElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.RowElement.BorderWidth = 2;
                e.RowElement.BorderColor = Color.Red;
            }

            if (e.RowElement.IsSelected)
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = Color.Black; 
                e.RowElement.ForeColor = Color.White; 
            }
            else
            {
                e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            }
        }
        public static void GridViews_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is GridFilterCellElement)
            {
                e.CellElement.DrawBorder = true;
                e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.CellElement.BorderWidth = 2;
                e.CellElement.BorderColor = Color.Red;
                Font font = new Font(e.CellElement.Font.Name, 9, FontStyle.Bold);
                e.CellElement.Font = font;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;

            }
            if (e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.DrawBorder = true;
                e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.CellElement.BorderLeftWidth = 0;
                e.CellElement.BorderRightWidth = 0;
                e.CellElement.BorderBottomWidth = 1;
                e.CellElement.BorderTopWidth = 1;
                e.CellElement.BorderTopColor = Color.Black;
                Font font = new Font(e.CellElement.Font.Name, 9, FontStyle.Bold);

                e.CellElement.Font = font;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            if (e.CellElement.RowInfo is GridViewGroupRowInfo)
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = Color.Aquamarine;
                Font font = new Font(e.CellElement.Font.Name, 9, FontStyle.Bold);
                e.CellElement.Font = font;
                e.CellElement.TextAlignment = ContentAlignment.MiddleLeft;
                e.CellElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            }
        }
        public static bool SetPersonalListRgv(RadGridView source, out string msg)
        {
            try
            {
                GlobalVars.Personals = GlobalVars.IzinComp.Id == 0 ? IOC.PersonalDataService.GetAllPersonals(out msg) : IOC.PersonalDataService.GetPersonalsByCompany(GlobalVars.IzinComp.Id, out msg);
                if (GlobalVars.Personals == null)
                {
                    return false;
                }
                source.Columns.Clear();
                source.DataSource = GlobalVars.Personals;
                int i = 0;
                source.Columns[i++].HeaderText = "Id";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "TC Kimlik No";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleLeft; source.Columns[i++].HeaderText = "Adı Soyadı";
                source.Columns[i++].HeaderText = "Firma Id";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "Doğum Tarihi";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "İşe Giriş Tarihi";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "İşten Çıkış Tarihi";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleRight; source.Columns[i++].HeaderText = "Önceki Dönemlerden\r\nDevreden İzin Gün Sayısı";
                source.Columns[i++].HeaderText = "İşten Ayrıldı";

                source.Columns[0].VisibleInColumnChooser = false;
                source.Columns[0].IsVisible = false;
                source.Columns[3].VisibleInColumnChooser = false;
                source.Columns[3].IsVisible = false;
                source.Columns[8].VisibleInColumnChooser = false;
                source.Columns[8].IsVisible = false;

                GridViewTextBoxColumn textBoxColumn = new GridViewTextBoxColumn();
                textBoxColumn.Name = "Cn";
                textBoxColumn.HeaderText = "Çalıştığı Firma";
                textBoxColumn.FieldName = "Cn";
                textBoxColumn.TextAlignment = ContentAlignment.MiddleLeft;
                textBoxColumn.MaxWidth = 450;
                textBoxColumn.MinWidth = 250;
                source.MasterTemplate.Columns.Add(textBoxColumn);

                foreach (var item in source.Rows)
                {
                    int aydi = Convert.ToInt32(item.Cells[3].Value); 
                    item.Cells["Cn"].Value = (from x in GlobalVars.Companies where x.Id == aydi select x.CompanyName).First();
                }
                GridViewDataColumn columnToMove;
                columnToMove = source.Columns[9];
                source.Columns.RemoveAt(9);
                source.Columns.Insert(0, columnToMove);

                source.GroupDescriptors.Clear();
                source.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                source.MasterTemplate.AutoExpandGroups = true;
                source.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return false;
            }
            return true;
        }
        public static bool SetLeaveListRgv(RadGridView source, out string msg)
        {
            source.DataSource = null;
            source.Columns.Clear();
            source.MasterTemplate.Templates.Clear();
            msg = "";
            try
            {
                if (GlobalVars.PersonalsForLeave == null) return false;

                source.DataSource = GlobalVars.PersonalsForLeave;
                int i = 0;

                source.Columns[i++].HeaderText = "Id";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "TC Kimlik No";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleLeft; source.Columns[i++].HeaderText = "Adı Soyadı";
                source.Columns[i++].HeaderText = "Çalıştığı Firma";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "Doğum Tarihi";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "İşe Giriş Tarihi";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter; source.Columns[i++].HeaderText = "İşten Çıkış Tarihi";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleRight; source.Columns[i++].HeaderText = "Önceki Dönemlerden\r\nDevreden İzin Gün Sayısı";
                source.Columns[i++].HeaderText = "İşten Ayrıldı";

                source.Columns[0].VisibleInColumnChooser = false;
                source.Columns[0].IsVisible = false;
                source.Columns[3].VisibleInColumnChooser = false;
                source.Columns[3].IsVisible = false;
                source.Columns[8].VisibleInColumnChooser = false;
                source.Columns[8].IsVisible = false;
                source.Columns[4].DataType = typeof(DateTime);
                source.Columns[4].FormatString = "{0: dd.MM.yyyy}";
                source.Columns[5].DataType = typeof(DateTime);
                source.Columns[5].FormatString = "{0: dd.MM.yyyy}";
                source.Columns[6].DataType = typeof(DateTime);
                source.Columns[6].FormatString = "{0: dd.MM.yyyy}";
                source.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                #region periods
                GridViewTemplate periodTemplate = new GridViewTemplate(); 
                periodTemplate.AllowAddNewRow = false;
                periodTemplate.AllowDeleteRow = false;
                periodTemplate.AllowEditRow = false;
                GlobalVars.LeavePeriods.Sort((a, b) => a.Startdate.CompareTo(b.Startdate));
                periodTemplate.DataSource = GlobalVars.LeavePeriods;

                int k = 0;
                periodTemplate.Columns[k].Name = "Id"; periodTemplate.Columns[k].IsVisible = false; periodTemplate.Columns[k].VisibleInColumnChooser = false; periodTemplate.Columns[k++].HeaderText = "Id";
                periodTemplate.Columns[k].Name = "Tcno"; periodTemplate.Columns[k].IsVisible = false; periodTemplate.Columns[k].VisibleInColumnChooser = false; periodTemplate.Columns[k++].HeaderText = "TC Kimlik No";
                periodTemplate.Columns[k].Name = "Period"; periodTemplate.Columns[k].TextAlignment = ContentAlignment.MiddleLeft; periodTemplate.Columns[k].MinWidth = 80; periodTemplate.Columns[k++].HeaderText = "Dönem";

                periodTemplate.Columns[k].Name = "Start";
                periodTemplate.Columns[k].TextAlignment = ContentAlignment.MiddleCenter;
                periodTemplate.Columns[k].MinWidth = 110;
                periodTemplate.Columns[k].FormatInfo = new System.Globalization.CultureInfo("tr-TR");
                periodTemplate.Columns[k].FormatString = "{0:dd.MM.yyyy}";
                periodTemplate.Columns[k++].HeaderText = "Başlangıç Tarihi";

                periodTemplate.Columns[k].Name = "End";
                periodTemplate.Columns[k].TextAlignment = ContentAlignment.MiddleCenter;
                periodTemplate.Columns[k].MinWidth = 110;
                periodTemplate.Columns[k].FormatInfo = new System.Globalization.CultureInfo("tr-TR");
                periodTemplate.Columns[k].FormatString = "{0:dd.MM.yyyy}";
                periodTemplate.Columns[k++].HeaderText = "Bitiş Tarihi";

                periodTemplate.Columns[k].Name = "His";
                periodTemplate.Columns[k].TextAlignment = ContentAlignment.MiddleCenter;
                periodTemplate.Columns[k].MinWidth = 110;
                periodTemplate.Columns[k].FormatString = "{0:0.0}";
                periodTemplate.Columns[k++].HeaderText = "Hak Edilen İzin Süresi";

                periodTemplate.Columns[k].Name = "Ki";
                periodTemplate.Columns[k].TextAlignment = ContentAlignment.MiddleCenter;
                periodTemplate.Columns[k].MinWidth = 110;
                periodTemplate.Columns[k].FormatString = "{0:0.0}";
                periodTemplate.Columns[k++].HeaderText = "Kullanılan İzin Süresi";

                periodTemplate.Columns[k].Name = "Srk";
                periodTemplate.Columns[k].TextAlignment = ContentAlignment.MiddleCenter;
                periodTemplate.Columns[k].MinWidth = 110;
                periodTemplate.Columns[k].FormatString = "{0:0.0}";
                periodTemplate.Columns[k++].HeaderText = "Sarkan İzin Süresi";

                periodTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                
                source.MasterTemplate.Templates.Add(periodTemplate);

                GridViewRelation relation = new GridViewRelation(source.MasterTemplate);
                relation.ChildTemplate = periodTemplate;
                relation.RelationName = "PersonalPeriods";
                relation.ParentColumnNames.Add("tcno");
                relation.ChildColumnNames.Add("tcno");
                source.Relations.Add(relation);

                #endregion
                AddSummariesToLeaves(source);
                foreach (GridViewRowInfo item in RgvLeaves.Rows) { item.IsExpanded = true; }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return false;
            }
            return true;
        }
        public static void AddSummariesToLeaves(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            //rgv.GroupDescriptors.Clear();
            //rgv.GroupDescriptors.Add(new GridGroupByExpression("cn Group By cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem hisSum = new GridViewSummaryItem(); hisSum.Name = "His"; hisSum.Aggregate = GridAggregateFunction.Sum; hisSum.FormatString = "Toplam: {0:0.0} gün"; lstSum.Add(hisSum);
            GridViewSummaryItem kiSum  = new GridViewSummaryItem(); kiSum.Name  = "Ki";  kiSum.Aggregate  = GridAggregateFunction.Sum; kiSum.FormatString  = "Toplam: {0:0.0} gün"; lstSum.Add(kiSum);
            GridViewSummaryItem srkSum = new GridViewSummaryItem(); srkSum.Name = "Srk"; srkSum.Aggregate = GridAggregateFunction.Sum; srkSum.FormatString = "Toplam: {0:0.0} gün"; lstSum.Add(srkSum);

            GridViewSummaryRowItem sumBottomRow = new GridViewSummaryRowItem();
            sumBottomRow.AddRange(lstSum);
            rgv.MasterTemplate.Templates[0].SummaryRowsBottom.Add(sumBottomRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
        }
        public static bool SetLeaveListRgvForPersonal(RadGridView source, out string msg)
        {
            source.DataSource = null;
            source.Columns.Clear();
            msg = "";
            try
            {
                List<LeavesForShow> leaveForShows = new List<LeavesForShow>();
                List<Personal> personals = new List<Personal>();
                List<long> pids = new List<long>();
                List<string> tcnos = new List<string>();
                pids = (from x in GlobalVars.LeavesForDialog select x.Pid).Distinct().ToList();
                personals = (from x in GlobalVars.Personals where pids.Contains(x.Id) select x).ToList();

                foreach (Leaves alv in GlobalVars.LeavesForDialog)
                {
                    LeavesForShow leavesFor = new LeavesForShow()
                    {
                        Id = alv.Id,
                        Tcno = (from x in GlobalVars.Personals where x.Id == alv.Pid select x.Tcno).FirstOrDefault(),
                        Ads = (from x in GlobalVars.Personals where x.Id == alv.Pid select x.Ads).FirstOrDefault(),
                        Comp = (from x in GlobalVars.Companies where x.Id == alv.Cid select x.CompanyName).FirstOrDefault(),
                        Startdate = alv.Startdate,
                        Enddate = alv.Enddate,
                        Paid = alv.Paid,
                        Timeval = alv.Timeval,
                        Ph = alv.Ph,
                        Notes = alv.Notes,
                        Docref = alv.Docref
                    };
                    leaveForShows.Add(leavesFor); // tabOrders.Sort((a, b) => a.Index.CompareTo(b.Index));
                }

                source.AllowAddNewRow = false;
                source.AllowDeleteRow = false;
                source.AllowEditRow = false;
                leaveForShows.Sort((a, b) => a.Startdate.CompareTo(b.Startdate));
                source.DataSource = leaveForShows;
                int i = 0;
                source.Columns[i].Name = "Id"; source.Columns[i].VisibleInColumnChooser = false; source.Columns[i].IsVisible = false; source.Columns[i++].HeaderText = "Id";
                source.Columns[i].Name = "Tcno"; source.Columns[i].VisibleInColumnChooser = false; source.Columns[i].IsVisible = false; source.Columns[i].MinWidth = 94; source.Columns[i].MaxWidth = 94; source.Columns[i++].HeaderText = "TC Kimlik No";
                source.Columns[i].Name = "Ads"; source.Columns[i].VisibleInColumnChooser = false; source.Columns[i].IsVisible = false; source.Columns[i].MinWidth = 80; source.Columns[i].MaxWidth = 280; source.Columns[i++].HeaderText = "Adı Soyadı";
                source.Columns[i].MinWidth = 200; source.Columns[i].VisibleInColumnChooser = false; source.Columns[i].IsVisible = false; source.Columns[i].MaxWidth = 400; source.Columns[i++].HeaderText = "Çalıştığı Firma";

                source.Columns[i].Name = "Startdate";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter;
                source.Columns[i].MinWidth = 110; source.Columns[i].MaxWidth = 110;
                source.Columns[i].FormatInfo = new System.Globalization.CultureInfo("tr-TR");
                source.Columns[i].FormatString = "{0:dd.MM.yyyy}";
                source.Columns[i++].HeaderText = "Başlangıç Tarihi";

                source.Columns[i].Name = "Enddate";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleCenter;
                source.Columns[i].MinWidth = 110; source.Columns[i].MaxWidth = 110;
                source.Columns[i].FormatInfo = new System.Globalization.CultureInfo("tr-TR");
                source.Columns[i].FormatString = "{0:dd.MM.yyyy}";
                source.Columns[i++].HeaderText = "Bitiş Tarihi";

                source.Columns[i].Name = "Paid";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleRight;
                source.Columns[i].MinWidth = 80; source.Columns[i].MaxWidth = 80; source.Columns[i++].HeaderText = "Ücretli İzin";

                source.Columns[i].Name = "Timeval";
                source.Columns[i].TextAlignment = ContentAlignment.MiddleRight;
                source.Columns[i].MinWidth = 80; source.Columns[i].MaxWidth = 80; source.Columns[i++].HeaderText = "İzin Süresi";

                source.Columns[i].Name = "Ph"; source.Columns[i].MinWidth = 100; source.Columns[i].MaxWidth = 100; source.Columns[i++].HeaderText = "Resmi Tatiller\r\nİş Günü Sayılsın";
                source.Columns[i].Name = "Notes"; source.Columns[i].MinWidth = 150; source.Columns[i].MaxWidth = 500; source.Columns[i++].HeaderText = "Açıklamalar";
                source.Columns[i].Name = "Docref"; source.Columns[i].MinWidth = 150; source.Columns[i].MaxWidth = 500; source.Columns[i++].HeaderText = "Dosya Eki";

                GridViewCommandColumn fileCol = WinHelpers.AddCommandColumnToRgv("Dosya Ekini Aç");
                fileCol.Name = "file";
                fileCol.Image = Resources.open_in_browser;
                fileCol.TextImageRelation = TextImageRelation.ImageBeforeText;
                fileCol.MinWidth = 100;
                fileCol.MaxWidth = 100;
                if (source.Columns.Count == 11) source.Columns.Add(fileCol);
                source.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return false;
            }
            return true;
        }

        public static void CompanyContext_Click(object sender, EventArgs e)
        {
            if (Users.ActiveUser.Yetki == 0)
            {
                RadMessageBox.Show("Bu işlem için yetkiniz yok", "Yetkisiz kullanıcı!", MessageBoxButtons.OK); return;
            }
            RadMenuItem item = sender as RadMenuItem;
            if (item.Tag.ToString() == "bilgi")
            {
                FCompany f = new FCompany(GlobalVars.CurrentCompany);
                f.ShowDialog();
            }
            else if (item.Tag.ToString() == "liste")
            {
                FCompanyList f = new FCompanyList();
                f.ShowDialog();
            }
        }

        public static void LeaveContext_Click(object sender,  EventArgs e)
        {
            string msg = "";
            RadMenuItem buton = sender as RadMenuItem;
            if (buton.Tag.ToString() == "copyCell")
            {
                Type cellType = RgvLeaves.CurrentCell.Value != null ? RgvLeaves.CurrentCell.Value.GetType() : null; if (cellType == null) return;
                string val = "";
                if (cellType.Name == "DateTime")
                {
                    val = Convert.ToDateTime(RgvLeaves.CurrentCell.Value).ToString("dd.MM.yyyy");
                }
                else if (cellType.Name == "Boolean")
                {
                    val = Convert.ToBoolean(RgvLeaves.CurrentCell.Value) == true ? "Evet" : "Hayır";
                }
                else
                {
                    val = RgvLeaves.CurrentCell.Value.ToString();
                }

                Clipboard.SetText(val);
            }
            else if (buton.Tag.ToString() == "copyRow")
            {
                string val = "";
                GridViewRowInfo row = RgvLeaves.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;
                val = row.Cells.Count == 9 ? "TC Kimlik No\tAdı Soyadı\tDoğum Tarihi\tİşe Giriş Tarihi\tİşten Çıkış Tarihi\tKullanılmamış İzin Süresi\r\n" : "Dönem\tBaşlangıç Tarihi\tBitiş Tarihi\tHak Edilen İzin Süresi\tKullanılan İzin Süresi\tSarkan İzin Süresi\r\n";

                foreach (GridViewCellInfo cellInfo in row.Cells)
                {
                    if ((row.Cells.Count == 9 && (cellInfo.ColumnInfo.FieldName == "Id" || cellInfo.ColumnInfo.FieldName == "Cid" || cellInfo.ColumnInfo.FieldName == "Active")) || (row.Cells.Count == 8 && (cellInfo.ColumnInfo.FieldName == "Id" || cellInfo.ColumnInfo.FieldName == "Tcno"))) continue;

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
            else if (buton.Tag.ToString() == "copyAllRow")
            {
                string val = "";
                GridViewRowInfo row = RgvLeaves.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;
                val = row.Cells.Count == 9 ? "TC Kimlik No\tAdı Soyadı\tDoğum Tarihi\tİşe Giriş Tarihi\tİşten Çıkış Tarihi\tKullanılmamış İzin Süresi\r\n" : "Dönem\tBaşlangıç Tarihi\tBitiş Tarihi\tHak Edilen İzin Süresi\tKullanılan İzin Süresi\tSarkan İzin Süresi\r\n";
                GridViewChildRowCollection satirlar = row.Cells.Count == 9 ? RgvLeaves.CurrentCell.ViewTemplate.MasterViewInfo.Rows : RgvLeaves.CurrentCell.RowInfo.ViewInfo.ChildRows;
                foreach (GridViewRowInfo item in satirlar)
                {
                    foreach (GridViewCellInfo cellInfo in item.Cells)
                    {
                        if ((item.Cells.Count == 9 && (cellInfo.ColumnInfo.FieldName == "Id" || cellInfo.ColumnInfo.FieldName == "Cid" || cellInfo.ColumnInfo.FieldName == "Active")) || (item.Cells.Count == 8 && (cellInfo.ColumnInfo.FieldName == "Id" || cellInfo.ColumnInfo.FieldName == "tcno"))) continue;

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
            else if (buton.Tag.ToString() == "edit")
            {
                GridViewRowInfo row = RgvLeaves.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;
                Personal personal = (from x in GlobalVars.Personals where x.Tcno == row.Cells["Tcno"].Value.ToString() select x).FirstOrDefault();
                FPersonal f = new FPersonal(personal.Cid, personal);
                f.ShowDialog();
            }
            else if (buton.Tag.ToString() == "showPeriod")
            {
                GridViewRowInfo row = RgvLeaves.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;
                DateTime sd = Convert.ToDateTime(row.Cells[3].Value).Date;
                DateTime ed = Convert.ToDateTime(row.Cells[4].Value).Date;
                Personal personal = (from x in GlobalVars.Personals where x.Tcno == row.Cells["Tcno"].Value.ToString() select x).FirstOrDefault();
                GlobalVars.LeavesForDialog = IOC.LeaveDataService.GetLeavesByPidAndDate(personal.Id, sd, ed, out msg);
                if (GlobalVars.LeavesForDialog == null) { Mesaj = $"Hata: {msg}"; return; }

                FLeaveList f = new FLeaveList(personal, new DateRange() { Sd = sd, Ed = ed });
                f.ShowDialog(); 
            }
            else if (buton.Tag.ToString() == "showAll")
            {
                GridViewRowInfo row = RgvLeaves.CurrentRow;
                Personal personal = (from x in GlobalVars.Personals where x.Tcno == row.Cells["Tcno"].Value.ToString() select x).FirstOrDefault();

                GlobalVars.LeavesForDialog = IOC.LeaveDataService.GetLeavesByPersonalId(personal.Id, out msg);
                if (GlobalVars.LeavesForDialog == null) { Mesaj = $"Hata: {msg}"; return; }

                FLeaveList f = new FLeaveList(personal);
                f.ShowDialog();
            }
            else if (buton.Tag.ToString() == "expand")
            {
                foreach (GridViewRowInfo item in RgvLeaves.Rows) { item.IsExpanded = true; }
            }
            else if (buton.Tag.ToString() == "collapse")
            {
                foreach (GridViewRowInfo item in RgvLeaves.Rows) { item.IsExpanded = false; }
            }
        }

        public static void GridViews_CurrentRowChanging(object sender, CurrentRowChangingEventArgs e)
        {
            if (e.NewRow is GridViewSearchRowInfo) e.Cancel = true;
        }

        public static void HLP_Colorize(RadGridView source)
        {
            ConditionalFormattingObject cfCalisan = new ConditionalFormattingObject("calisan", ConditionTypes.Equal, "0", "", true);
            cfCalisan.RowBackColor = Settings.Default.hlCalisanColor;
            source.Columns["GGun"].ConditionalFormattingObjectList.Add(cfCalisan);

            ConditionalFormattingObject cfoGiris = new ConditionalFormattingObject("giris", ConditionTypes.Greater, "0", "", true);
            cfoGiris.RowBackColor = Settings.Default.gGunColor;
            source.Columns["GGun"].ConditionalFormattingObjectList.Add(cfoGiris);

            ConditionalFormattingObject cfoCikis = new ConditionalFormattingObject("cikis", ConditionTypes.Greater, "0", "", true);
            cfoCikis.RowBackColor = Settings.Default.cGunColor;
            source.Columns["CGun"].ConditionalFormattingObjectList.Add(cfoCikis);
        }
    }
    public class CustomSearchRow : GridViewSearchRowInfo
    {
        public CustomSearchRow(GridViewInfo viewInfo) : base(viewInfo)
        {
        }

        protected override bool MatchesSearchCriteria(string searchCriteria, GridViewRowInfo row, GridViewColumn col)
        {
            if (col == null) return false;
            searchCriteria = searchCriteria.ToUpper();
            return (row.Cells[col.Name].Value + "").Contains(searchCriteria);
        }
    }
}
