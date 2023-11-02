using Models.Common;
using Models.Domain;
using SgkAssistant.Forms.Defs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO;
using System.Text.RegularExpressions;
namespace SgkAssistant.Helpers
{
    public class SgkAutomations
    {
        public List<SgkHlp> LstHlp { get; set; } = new List<SgkHlp>();
        public List<SgkIgl> LstIgl { get; set; } = new List<SgkIgl>();
        public SgkAutomations()
        {
            Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();

        }
        public bool FromDb { get; set; }
        private void FormatDecimal(RadGridView rgv, int sc, int fc)
        {

            foreach (GridViewRowInfo row in rgv.Rows)
            {
                for (int i = sc; i <= fc; i++)
                {
                    string value = row.Cells[i].Value.ToString();

                    row.Cells[i].Value = Convert.ToDecimal(row.Cells[i].Value) ;
                }
            }
        }

        private void HideColumns(RadGridView rgv)
        {
            foreach (GridViewColumn col in rgv.Columns)
            {
                if (col.Name == "Id" || col.Name == "Cx" || col.Name == "Un"  || col.Name == "Ttl" || col.Name == "Pdfid" || col.Name == "PdfPath")
                {
                    col.IsVisible = false; col.VisibleInColumnChooser = false;
                }else if(col.Name == "Cd")
                {
                    col.IsVisible = false;
                }
                else
                {
                    col.IsVisible = true;
                }

            }
        }
        #region PD
        public void SetRgvPd(RadGridView rgv, bool simpleMode)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 4, 20);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Yil"; rgv.Columns[h++].HeaderText = "Yıl";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Ay"; rgv.Columns[h++].HeaderText = "Ay"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Drm"; rgv.Columns[h++].HeaderText = "Durumu"; // column 3
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Pb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Prim Borcu"; // column 4
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "PbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Prim Borcu\r\nGecikme Zammı"; // column 5
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Ipcb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İdari Para\r\nCezası Borcu"; // column 6
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "IpcbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İdari Para\r\nCezası Borcu\r\nGecikme Zammı"; // column 7
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Ekpb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Eğitime Katkı\r\nPayı Borcu"; // column 8
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "EkpbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Eğitime Katkı\r\nPayı Borcu\r\nGecikme Zammı"; // column 9
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Oivb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Özel İşlem\r\nVergisi Borcu"; // column 10
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "OivbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Özel İşlem\r\nVergisi Borcu\r\nGecikme Zammı"; // column 111
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Ib"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İşsizlik Borcu"; // column 12 
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "IbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İşsizlik Borcu\r\nGecikme Zammı"; // column 13
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Dvb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Damga Vergisi\r\nBorcu"; // column 14
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "DvbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Damga Vergisi\r\nBorcu\r\nGecikme Zammı"; // column 15
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Dpgzb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Donmuş Prim \r\nGecikme Zammı\r\nBorcu"; // column 16
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "DpgzbGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Donmuş Prim \r\nGecikme Zammı Borcu\r\nGecikme Zammı"; // column 17
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Dekpgzb"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Donmuş EKP \r\nGecikme Zammı\r\nBorcu"; // column 18
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Sbt"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Sadece Borçlar\r\nToplamı"; // column 19
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "SbtGz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Gecikme Zamları\r\nToplamı"; // column 20
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h++].HeaderText = "Id"; // column 21
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cx"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 22
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 23
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi"; // column 24

                ChangePdLayout(rgv, simpleMode);
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 60;
                AddSummariesToPd(rgv);
            }
            catch (Exception)
            {

            }
            
        }
        public void ChangePdLayout(RadGridView rgv, bool simpleMode)
        {
            rgv.GroupDescriptors.Clear();
            if (simpleMode)
            {
                foreach (GridViewColumn col in rgv.Columns)
                {
                    if (col.Name == "Cn" || col.Name == "Yil" || col.Name == "Ay" || col.Name == "Sbt" || col.Name == "SbtGz" || col.Name == "Cd")
                    {
                        col.IsVisible = true;
                    }
                    else
                    {
                        col.IsVisible = false;
                    }
                }
                rgv.Columns["Yil"].MaxWidth = 200; 
                rgv.Columns["Ay"].MaxWidth = 200; 
                rgv.Columns["Sbt"].MaxWidth = 250; 
                rgv.Columns["SbtGz"].MaxWidth = 250; 
            }
            else
            {
                HideColumns(rgv);
                rgv.Columns["Cd"].IsVisible = true; 
                rgv.Columns["Drm"].MaxWidth = 100;		
                rgv.Columns["Pb"].MaxWidth = 122;		
                rgv.Columns["PbGz"].MaxWidth = 92;		
                rgv.Columns["Ipcb"].MaxWidth = 82;		
                rgv.Columns["IpcbGz"].MaxWidth = 92;	
                rgv.Columns["Ekpb"].MaxWidth = 82;		
                rgv.Columns["EkpbGz"].MaxWidth = 92;	
                rgv.Columns["Oivb"].MaxWidth = 82;		
                rgv.Columns["OivbGz"].MaxWidth = 92;	
                rgv.Columns["Ib"].MaxWidth = 80;		
                rgv.Columns["IbGz"].MaxWidth = 92;		
                rgv.Columns["Dvb"].MaxWidth = 92;		
                rgv.Columns["DvbGz"].MaxWidth = 92;		
                rgv.Columns["Dpgzb"].MaxWidth = 92;		
                rgv.Columns["DpgzbGz"].MaxWidth = 130;	
                rgv.Columns["Dekpgzb"].MaxWidth = 92;	
                rgv.Columns["Yil"].MaxWidth = 110; 
                rgv.Columns["Ay"].MaxWidth = 60; 
                rgv.Columns["Sbt"].MaxWidth = 92; 
                rgv.Columns["SbtGz"].MaxWidth = 110; 

            }
            
            rgv.Columns["Cn"].BestFit();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("[Cn] Group By [Cn]"));
        }
        public void AddSummariesToPd(RadGridView rgv, string exp ="Cn ASC")
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.MasterTemplate.ShowTotals = true;
            rgv.SummaryRowsBottom.Clear();
            GridViewSummaryItem firmaAdi = new GridViewSummaryItem();
            firmaAdi.Name = "cn"; firmaAdi.Aggregate = GridAggregateFunction.Var; firmaAdi.FormatString = "Toplamlar";
            GridViewSummaryItem yil = new GridViewSummaryItem(); 
            yil.Name = "yil"; yil.Aggregate = GridAggregateFunction.Var; yil.FormatString = "Toplamlar";
            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            switch (exp)
            {
                case "Cn ASC":  case "[Cn] Group By [Cn]":
                    lstSum.Add(yil);
                    break;
                case "Yil ASC": case "[Yil] Group By [Yil]":  case "Ay ASC":  case "[Ay] Group By [Ay]": case "Drm ASC": case "[Drm] Group By [Drm]":
                
                    lstSum.Add(firmaAdi);
                    break;
                default:
                    break;
            }
          

            GridViewSummaryItem pbSum = new GridViewSummaryItem();		pbSum.Name = "Pb";		     pbSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(pbSum);		
            GridViewSummaryItem pbGzSum = new GridViewSummaryItem();	pbGzSum.Name = "PbGz";		 pbGzSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(pbGzSum);		
            GridViewSummaryItem ipcbSum = new GridViewSummaryItem();	ipcbSum.Name = "Ipcb";		 ipcbSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(ipcbSum);		
            GridViewSummaryItem ipcbGzSum = new GridViewSummaryItem();	ipcbGzSum.Name = "IpcbGz";	 ipcbGzSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(ipcbGzSum);		
            GridViewSummaryItem ekpbSum = new GridViewSummaryItem();	ekpbSum.Name = "Ekpb";		 ekpbSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(ekpbSum);		
            GridViewSummaryItem ekpbGzSum = new GridViewSummaryItem();	ekpbGzSum.Name = "EkpbGz";	 ekpbGzSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(ekpbGzSum);		
            GridViewSummaryItem oivbSum = new GridViewSummaryItem();	oivbSum.Name = "Oivb";		 oivbSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(oivbSum);		
            GridViewSummaryItem oivbGzSum = new GridViewSummaryItem();	oivbGzSum.Name = "OivbGz";	 oivbGzSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(oivbGzSum);		
            GridViewSummaryItem ibSum = new GridViewSummaryItem();		ibSum.Name = "Ib";		     ibSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(ibSum);		
            GridViewSummaryItem ibGzSum = new GridViewSummaryItem();	ibGzSum.Name = "IbGz";		 ibGzSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(ibGzSum);		
            GridViewSummaryItem dvbSum = new GridViewSummaryItem();		dvbSum.Name = "Dvb";		 dvbSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(dvbSum);		
            GridViewSummaryItem dvbGzSum = new GridViewSummaryItem();	dvbGzSum.Name = "DvbGz";	 dvbGzSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(dvbGzSum);		
            GridViewSummaryItem dpgzbSum = new GridViewSummaryItem();	dpgzbSum.Name = "Dpgzb";	 dpgzbSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(dpgzbSum);		
            GridViewSummaryItem dpgzbGzSum = new GridViewSummaryItem();	dpgzbGzSum.Name = "DpgzbGz"; dpgzbGzSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(dpgzbGzSum);		
            GridViewSummaryItem dekpgzbSum = new GridViewSummaryItem();	dekpgzbSum.Name = "Dekpgzb"; dekpgzbSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(dekpgzbSum);		
            GridViewSummaryItem sbtSum = new GridViewSummaryItem();		sbtSum.Name = "Sbt";		 sbtSum.Aggregate = GridAggregateFunction.Sum;		 lstSum.Add(sbtSum);		
            GridViewSummaryItem sbtGzSum = new GridViewSummaryItem();	sbtGzSum.Name = "SbtGz";	 sbtGzSum.Aggregate = GridAggregateFunction.Sum;	 lstSum.Add(sbtGzSum);
            foreach (GridViewSummaryItem item in lstSum)
            {
                if(item.Aggregate == GridAggregateFunction.Sum) item.FormatString = "{0:C}";
            }
            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);
            
            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;
           
        }
        #endregion

        #region ET
        public void SetRgvEt(RadGridView rgv)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 5, 5);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Tt"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Tahsilat Tarihi";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Yil"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Dönem Yıl"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Ay"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Dönem Ay"; // column 3
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Bt"; rgv.Columns[h++].HeaderText = "Borç Türü"; // column 4
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Ttr"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Tahsilat Tutarı"; // column 5

                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h++].HeaderText = "Id"; // column 6
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cx"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 7
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 8
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi"; // column 9

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv);
                rgv.Columns["Cd"].IsVisible = true;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 60;
                AddSummariesToEt(rgv);
            }
            catch (Exception)
            {
                
            }
            
        }
        public void AddSummariesToEt(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem ttSum = new GridViewSummaryItem(); ttSum.Name = "Ttr"; ttSum.Aggregate = GridAggregateFunction.Sum; ttSum.FormatString = "Toplam: {0:C}"; lstSum.Add(ttSum);
            
            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;

        }

        public void SetRgvMe(RadGridView rgv)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 2, 2);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Byt"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Bankaya Yatırılma Tarihi";// column 1
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Etr"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "	Emanetteki Tahsilat Tutarı"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Tur"; rgv.Columns[h++].HeaderText = "	Tahsilat Türü"; // column 3

                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h++].HeaderText = "Id"; // column 4
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cx"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 5
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 6
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi"; // column 7

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv);
                rgv.Columns["Cd"].IsVisible = true;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 60;
                AddSummariesToMe(rgv);
            }
            catch (Exception)
            {

            }
            
        }
        public void AddSummariesToMe(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
           
            GridViewSummaryItem ettSum = new GridViewSummaryItem(); ettSum.Name = "Etr"; ettSum.Aggregate = GridAggregateFunction.Sum; ettSum.FormatString = "Toplam {0:C}"; lstSum.Add(ettSum);

            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;

        }

        #endregion

        #region CR
        public void SetRgvCr(RadGridView rgv)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 5, 8);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Kn"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Kart No";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Ty"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Takip Yıl"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Tn"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Takip No"; // column 3
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Bt"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Borç Türü"; // column 4
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Ba"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Borç Aslı"; // column 5
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Gz"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Gecikme Zammı"; // column 6
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Tm"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Takip Masrafı"; // column 7
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Tp"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Toplam"; // column 8
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h++].HeaderText = "Id"; // column 9
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Cx"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 10
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 11
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi";

                rgv.GroupDescriptors.Clear();

                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv); rgv.Columns["Cd"].IsVisible = true;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 30;
                AddSummariesToCr(rgv);
            }
            catch (Exception)
            {

            }
            
        }
        public void AddSummariesToCr(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem baSum = new GridViewSummaryItem(); baSum.Name = "Ba"; baSum.Aggregate = GridAggregateFunction.Sum; baSum.FormatString = "Toplam: {0:C}"; lstSum.Add(baSum);
            GridViewSummaryItem gzSum = new GridViewSummaryItem(); gzSum.Name = "Gz"; gzSum.Aggregate = GridAggregateFunction.Sum; gzSum.FormatString = "Toplam: {0:C}"; lstSum.Add(gzSum);
            GridViewSummaryItem tmSum = new GridViewSummaryItem(); tmSum.Name = "Tm"; tmSum.Aggregate = GridAggregateFunction.Sum; tmSum.FormatString = "Toplam: {0:C}"; lstSum.Add(tmSum);
            GridViewSummaryItem tpSum = new GridViewSummaryItem(); tpSum.Name = "Tp"; tpSum.Aggregate = GridAggregateFunction.Sum; tpSum.FormatString = "Toplam: {0:C}"; lstSum.Add(tpSum);


            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;

        }
        #endregion

        #region AAAB
        public void SetRgv6661(RadGridView rgv)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 4, 4);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Yil"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Yıl";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Ay"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Ay"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Fgs"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "Faydalanılan Gün Sayısı"; // column 3
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Dt"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Destek Tutarı"; // column 4
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Tt"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Tahsilat Tarihi"; // column 5
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h++].HeaderText = "Id"; // column 9
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Cx"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 10
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 11
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi";

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv); rgv.Columns["Cd"].IsVisible = true;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 30;
                AddSummariesTo6661(rgv);
            }
            catch (Exception)
            {

            }
            
        }
        public void AddSummariesTo6661(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem baSum = new GridViewSummaryItem(); baSum.Name = "Fgs"; baSum.Aggregate = GridAggregateFunction.Sum; baSum.FormatString = "Toplam: {0:D}"; lstSum.Add(baSum);
            GridViewSummaryItem gzSum = new GridViewSummaryItem(); gzSum.Name = "Dt"; gzSum.Aggregate = GridAggregateFunction.Sum; gzSum.FormatString = "Toplam: {0:C}"; lstSum.Add(gzSum);

            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;
        }
        #endregion

        #region IGL
        public void SetRgvIgl(RadGridView rgv)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 5, 6);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Tc"; rgv.Columns[h++].HeaderText = "Kimlik No";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Ads"; rgv.Columns[h++].HeaderText = "Ad Soyad"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Gc"; rgv.Columns[h++].HeaderText = "Giriş-Çıkış"; // column 3
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Tr"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Tarih"; // column 4
                rgv.Columns[h].DataType = typeof(Decimal); rgv.Columns[h].Name = "Stn"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İstisna"; // column 5
                rgv.Columns[h].DataType = typeof(Decimal); rgv.Columns[h].Name = "Ipc"; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İdari Para Cezası"; // column 6
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Isl"; rgv.Columns[h++].HeaderText = "İşlem"; // column 7
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Ist"; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "İşlem Tarihi"; // column 8
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Isa";   rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h++].HeaderText = "İşlem Saati"; // column 9
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h++].HeaderText = "Id"; // column 9
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Cx"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 10
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 11
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi";

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv); rgv.Columns["Cd"].IsVisible = true;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 30;
                AddSummariesToIgl(rgv);
            }
            catch (Exception)
            {
            }
            
        }
        public void AddSummariesToIgl(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem stnSum = new GridViewSummaryItem(); stnSum.Name = "Stn"; stnSum.Aggregate = GridAggregateFunction.Sum; stnSum.FormatString = "Toplam: {0:C}"; lstSum.Add(stnSum);
            GridViewSummaryItem ipcSum = new GridViewSummaryItem(); ipcSum.Name = "Ipc"; ipcSum.Aggregate = GridAggregateFunction.Sum; ipcSum.FormatString = "Toplam: {0:C}"; lstSum.Add(ipcSum);

            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;

        }
        #endregion

        #region HL
        public void SetRgvHl(RadGridView rgv, bool printing = false)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
                if (!FromDb) FormatDecimal(rgv, 8, 8);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string);   rgv.Columns[h].Name = "Cn"; rgv.Columns[h].MinWidth = 280; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0 
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].Name = "Tya"; rgv.Columns[h].MinWidth = 100; rgv.Columns[h].FormatString = "{0: yyyy/MM}"; rgv.Columns[h++].HeaderText = "Tahakkuk Yıl / Ay";// column 1
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].Name = "Hya"; rgv.Columns[h].MinWidth = 100; rgv.Columns[h].FormatString = "{0: yyyy/MM}"; rgv.Columns[h++].HeaderText = "Hizmet Yıl / Ay"; // column 2
                rgv.Columns[h].DataType = typeof(string);   rgv.Columns[h].Name = "Bt";  rgv.Columns[h].MinWidth = 100; rgv.Columns[h++].HeaderText = "Belge Türü"; // column 3
                rgv.Columns[h].DataType = typeof(string);   rgv.Columns[h].Name = "Bm";  rgv.Columns[h].MinWidth = 100; rgv.Columns[h++].HeaderText = "Belge Mahiyeti"; // column 4
                rgv.Columns[h].DataType = typeof(string);   rgv.Columns[h].Name = "Kn";  rgv.Columns[h].MinWidth = 100; rgv.Columns[h++].HeaderText = "Kanun No"; // column 5
                rgv.Columns[h].DataType = typeof(int);      rgv.Columns[h].Name = "Tcs"; rgv.Columns[h].MinWidth = 120; rgv.Columns[h].FormatString = "{0:D}"; rgv.Columns[h++].HeaderText = "Toplam Çalışan Sayısı"; // column 6
                rgv.Columns[h].DataType = typeof(int);      rgv.Columns[h].Name = "Tgs"; rgv.Columns[h].MinWidth = 120; rgv.Columns[h].FormatString = "{0:D}"; rgv.Columns[h++].HeaderText = "Toplam Gün Sayısı"; // column 7
                rgv.Columns[h].DataType = typeof(decimal);  rgv.Columns[h].Name = "Tpt"; rgv.Columns[h].MinWidth = 80; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Toplam Pek Tutar"; // column 8

                if (!printing)
                {
                    GridViewCommandColumn pdfColumn = WinHelpers.AddCommandColumnToRgv("PDF Dosyası");
                    rgv.Columns.Add(pdfColumn);
                    GridViewCommandColumn detailsColumn = WinHelpers.AddCommandColumnToRgv("Dosya İçeriği", "openHL", "İçeriği Göster", "İçeriği Göster");
                    rgv.Columns.Add(detailsColumn);
                }

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv);
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 40;
                AddSummariesToHl(rgv);
                rgv.Columns["PdfPath"].IsVisible = false;
                rgv.Columns["PdfPath"].VisibleInColumnChooser = false;
            }
            catch (Exception)
            {

            }
            
        }
        public void AddSummariesToHl(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem tcsSum = new GridViewSummaryItem(); tcsSum.Name = "Tcs"; tcsSum.Aggregate = GridAggregateFunction.Sum; tcsSum.FormatString = "Toplam: {0:D}"; lstSum.Add(tcsSum);
            GridViewSummaryItem tgsSum = new GridViewSummaryItem(); tgsSum.Name = "Tgs"; tgsSum.Aggregate = GridAggregateFunction.Sum; tgsSum.FormatString = "Toplam: {0:D}"; lstSum.Add(tgsSum);
            GridViewSummaryItem tptSum = new GridViewSummaryItem(); tptSum.Name = "Tpt"; tptSum.Aggregate = GridAggregateFunction.Sum; tptSum.FormatString = "Toplam: {0:C}"; lstSum.Add(tptSum);

            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;

        }
        #endregion

        #region HLP
        public void SetRgvHlp(RadGridView rgv)
        {
            try
            {
                if (!FromDb) FormatDecimal(rgv, 3, 4);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h].MinWidth = 280; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].Name = "Tcn";  rgv.Columns[h].MinWidth = 86; rgv.Columns[h++].HeaderText = "Kimlik No";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Ads"; rgv.Columns[h].MinWidth = 120; rgv.Columns[h++].HeaderText = "Adı Soyadı"; // column 2
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Utl"; rgv.Columns[h].MinWidth = 72; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Ücret"; // column 3
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Itl"; rgv.Columns[h].MinWidth = 72; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İkramiye"; // column 4
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Gun"; rgv.Columns[h].MaxWidth = 110; rgv.Columns[h].MinWidth = 50; rgv.Columns[h++].HeaderText = "Gün"; // column 5
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Ucg"; rgv.Columns[h].MaxWidth = 110; rgv.Columns[h].MinWidth = 50; rgv.Columns[h++].HeaderText = "Uzaktan\r\nÇalışma Günü"; // column 6
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "EGun"; rgv.Columns[h].MaxWidth = 110; rgv.Columns[h].MinWidth = 50; rgv.Columns[h++].HeaderText = "Eksik\r\nGün"; // column 7
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "GGun"; rgv.Columns[h].MaxWidth = 120; rgv.Columns[h].MinWidth = 60; rgv.Columns[h++].HeaderText = "Giriş\r\nGünü"; // column 8
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "CGun"; rgv.Columns[h].MaxWidth = 120; rgv.Columns[h].MinWidth = 60; rgv.Columns[h++].HeaderText = "Çıkış\r\nGünü"; // column 9
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Icn"; rgv.Columns[h].MaxWidth = 120; rgv.Columns[h].MinWidth = 60; rgv.Columns[h++].HeaderText = "İşten Çıkış\r\nNedeni"; // column 10
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Egn"; rgv.Columns[h].MaxWidth = 120; rgv.Columns[h].MinWidth = 60; rgv.Columns[h++].HeaderText = "Eksik Gün\r\nNedeni"; // column 11
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Mk"; rgv.Columns[h].MaxWidth = 150; rgv.Columns[h].MinWidth = 70; rgv.Columns[h++].HeaderText = "Meslek\r\nKodu"; // column 12
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].Name = "Ya"; rgv.Columns[h].MaxWidth = 100; rgv.Columns[h].MinWidth = 72; rgv.Columns[h].FormatString = "{0: yyyy/MM}"; rgv.Columns[h++].HeaderText = "Yıl / Ay"; // column 13
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Bm"; rgv.Columns[h].MaxWidth = 72; rgv.Columns[h].MinWidth = 72; rgv.Columns[h++].HeaderText = "Belge\r\nMahiyeti"; // column 14
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Bt"; rgv.Columns[h].MaxWidth = 72; rgv.Columns[h].MinWidth = 72; rgv.Columns[h++].HeaderText = "Belge\r\nTürü"; // column 15
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Kk"; rgv.Columns[h].MaxWidth = 72; rgv.Columns[h].MinWidth = 72; rgv.Columns[h++].HeaderText = "Kanun\r\nKodu"; // column 16
                rgv.Columns[22].DataType = typeof(DateTime); rgv.Columns[22].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[22].Name = "Cd"; rgv.Columns[22].MaxWidth = 100; rgv.Columns[22].MinWidth = 72; rgv.Columns[22].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[22].HeaderText = "Sorgulama\r\nTarihi"; // column 17

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv);
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 40;
                AddSummariesToHlp(rgv);
            }
            catch (Exception)
            {

            }
            
        }
        public void AddSummariesToHlp(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();
            GridViewSummaryItem adsSum  = new GridViewSummaryItem(); adsSum.Name    = "Ads";    adsSum.Aggregate =  GridAggregateFunction.Sum; adsSum.FormatString  = "Toplamlar"; lstSum.Add(adsSum); 
            GridViewSummaryItem utlSum  = new GridViewSummaryItem(); utlSum.Name    = "Utl";    utlSum.Aggregate =  GridAggregateFunction.Sum; utlSum.FormatString  = "{0:C}"; lstSum.Add(utlSum);
            GridViewSummaryItem itlSum  = new GridViewSummaryItem(); itlSum.Name    = "Itl";    itlSum.Aggregate =  GridAggregateFunction.Sum; itlSum.FormatString  = "{0:C}"; lstSum.Add(itlSum);
            GridViewSummaryItem gunSum  = new GridViewSummaryItem(); gunSum.Name    = "Gun";    gunSum.Aggregate =  GridAggregateFunction.Sum; gunSum.FormatString  = "{0}"; lstSum.Add(gunSum);
            GridViewSummaryItem eGunSum = new GridViewSummaryItem(); eGunSum.Name   = "EGun";   eGunSum.Aggregate = GridAggregateFunction.Sum; eGunSum.FormatString = "{0}"; lstSum.Add(eGunSum);

            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;

        }
        #endregion

        #region THKK
        public void SetRgvThkk(RadGridView rgv)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
                if (!FromDb) FormatDecimal(rgv, 8, 8);
                int h = 0;
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Cn"; rgv.Columns[h].MinWidth = 280; rgv.Columns[h++].HeaderText = "Firma Adı"; // column 0 
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].Name = "Tya"; rgv.Columns[h].MinWidth = 90; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h].FormatString = "{0: yyyy/MM}"; rgv.Columns[h++].HeaderText = "Tahakkuk\r\nYıl / Ay";// column 1
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Bm"; rgv.Columns[h].MinWidth = 90; rgv.Columns[h].MaxWidth = 90; rgv.Columns[h++].HeaderText = "Belge\r\nMahiyeti"; // column 2
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "Sgm"; rgv.Columns[h].MinWidth = 100; rgv.Columns[h++].HeaderText = "Sosyal Güvenlik\r\n Merkezi"; // column 3
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Tp"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Toplam Prim\r\nÖdemesi"; // column 4
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Ip"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "İşsizlik Primi\r\nÖdemesi"; // column 5
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn14857"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "14857 Sayılı Kanun\r\nPrim İndirimi"; // column 6
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn15921"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "15921 Sayılı Kanun\r\nPrim İndirimi"; // column 7
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn6645"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "6645 Sayılı Kanun\r\nPrim İndirimi"; // column 8
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn15510"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "15510 Sayılı Kanun\r\nPrim İndirimi"; // column 9
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn2828"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "2828 Sayılı Kanun\r\nPrim İndirimi"; // column 10
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn6111"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "6111 Sayılı Kanun\r\nPrim İndirimi"; // column 11
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn17103"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "17103 Sayılı Kanun\r\nPrim İndirimi"; // column 12
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn17103i"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "17103 Sayılı Kanun\r\nİşsizlik İndirimi"; // column 13
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn27103"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "27103 Sayılı Kanun\r\nPrim İndirimi"; // column 14
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn27103i"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "27103 Sayılı Kanun\r\nİşsizlik İndirimi"; // column 15
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn37103"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "37103 Sayılı Kanun\r\nPrim İndirimi"; // column 16
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn37103i"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "37103 Sayılı Kanun\r\nİşsizlik İndirimi"; // column 17
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn7252"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "7252 Sayılı Kanun\r\nPrim İndirimi"; // column 18
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn17256"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "17256 Sayılı Kanun\r\nPrim İndirimi"; // column 19
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn7316"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "7316 Sayılı Kanun\r\nPrim İndirimi"; // column 20
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn7319"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "7319 Sayılı Kanun\r\nPrim İndirimi"; // column 21
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn5510"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "5510 Sayılı Kanun\r\nPrim İndirimi"; // column 22
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn4857"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "4857 Sayılı Kanun\r\nPrim İndirimi"; // column 23
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn159210"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "159210 Sayılı Kanun\r\nPrim İndirimi"; // column 24
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Kn3294"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "3294 Sayılı Kanun\r\nPrim İndirimi"; // column 25
                rgv.Columns[h].DataType = typeof(decimal); rgv.Columns[h].Name = "Odenecek"; rgv.Columns[h].MinWidth = 110; rgv.Columns[h].FormatString = "{0:C}"; rgv.Columns[h++].HeaderText = "Ödenecek\r\nNet Tutar"; // column 26
                rgv.Columns[h].DataType = typeof(string); rgv.Columns[h].Name = "PdfPath"; rgv.Columns[h++].HeaderText = "PDF dosya yolu"; // column 27
                rgv.Columns[h].DataType = typeof(bool); rgv.Columns[h].Name = "Onayli"; rgv.Columns[h].MinWidth = 50; rgv.Columns[h].MaxWidth = 50; rgv.Columns[h++].HeaderText = "Onaylı"; // column 28
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Id"; rgv.Columns[h].MinWidth = 80; rgv.Columns[h].FormatString = "{0:D}"; rgv.Columns[h++].HeaderText = "Id"; // column 29
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Cx"; rgv.Columns[h].MinWidth = 80; rgv.Columns[h].FormatString = "{0:D}"; rgv.Columns[h++].HeaderText = "Firma Id"; // column 30
                rgv.Columns[h].DataType = typeof(int); rgv.Columns[h].Name = "Un"; rgv.Columns[h].MinWidth = 80; rgv.Columns[h].FormatString = "{0:D}"; rgv.Columns[h++].HeaderText = "Ekleyen"; // column 31
                rgv.Columns[h].DataType = typeof(DateTime); rgv.Columns[h].TextAlignment = ContentAlignment.MiddleCenter; rgv.Columns[h].Name = "Cd"; rgv.Columns[h].MinWidth = 80; rgv.Columns[h].MaxWidth = 80; rgv.Columns[h].FormatString = "{0: dd.MM.yyyy}"; rgv.Columns[h++].HeaderText = "Sorgulama\r\nTarihi";// column 32


                GridViewCommandColumn pdfColumn = WinHelpers.AddCommandColumnToRgv("PDF Dosyası");
                pdfColumn.MaxWidth = 120; pdfColumn.MinWidth = 120;
                rgv.Columns.Add(pdfColumn);

                rgv.GroupDescriptors.Clear();
                rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
                HideColumns(rgv);
                rgv.Columns["Cd"].IsVisible = true;
                rgv.Columns["Cd"].VisibleInColumnChooser = true;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.GridViewElement.TableElement.TableHeaderHeight = 40;
                AddSummariesToThkk(rgv);
                rgv.Columns["PdfPath"].IsVisible = false;
                rgv.Columns["PdfPath"].VisibleInColumnChooser = false;
            }
            catch (Exception)
            {

            }
        }
        public void AddSummariesToThkk(RadGridView rgv)
        {
            rgv.MasterTemplate.AutoExpandGroups = true;
            rgv.GroupDescriptors.Clear();
            rgv.GroupDescriptors.Add(new GridGroupByExpression("Cn Group By Cn"));
            rgv.SummaryRowsBottom.Clear();

            rgv.MasterTemplate.ShowTotals = true;

            List<GridViewSummaryItem> lstSum = new List<GridViewSummaryItem>();

            CustomSummaryItem tptSum = new CustomSummaryItem(); tptSum.Name = "Tp"; tptSum.Aggregate = GridAggregateFunction.Sum;  tptSum.FormatString = "{0:C}"; lstSum.Add(tptSum);
            CustomSummaryItem ipSum = new CustomSummaryItem(); ipSum.Name = "Ip"; ipSum.Aggregate = GridAggregateFunction.Sum; ipSum.FormatString = "{0:C}"; lstSum.Add(ipSum);
            CustomSummaryItem kn14857Sum = new CustomSummaryItem(); kn14857Sum.Name = "Kn14857"; kn14857Sum.Aggregate = GridAggregateFunction.Sum; kn14857Sum.FormatString = "{0:C}"; lstSum.Add(kn14857Sum);
            CustomSummaryItem kn15921Sum = new CustomSummaryItem(); kn15921Sum.Name = "Kn15921"; kn15921Sum.Aggregate = GridAggregateFunction.Sum; kn15921Sum.FormatString = "{0:C}"; lstSum.Add(kn15921Sum);
            CustomSummaryItem kn6645Sum = new CustomSummaryItem(); kn6645Sum.Name = "Kn6645"; kn6645Sum.Aggregate = GridAggregateFunction.Sum; kn6645Sum.FormatString = "{0:C}"; lstSum.Add(kn6645Sum);
            CustomSummaryItem kn15510Sum = new CustomSummaryItem(); kn15510Sum.Name = "Kn15510"; kn15510Sum.Aggregate = GridAggregateFunction.Sum; kn15510Sum.FormatString = "{0:C}"; lstSum.Add(kn15510Sum);
            CustomSummaryItem kn2828Sum = new CustomSummaryItem(); kn2828Sum.Name = "Kn2828"; kn2828Sum.Aggregate = GridAggregateFunction.Sum; kn2828Sum.FormatString = "{0:C}"; lstSum.Add(kn2828Sum);
            CustomSummaryItem kn6111Sum = new CustomSummaryItem(); kn6111Sum.Name = "Kn6111"; kn6111Sum.Aggregate = GridAggregateFunction.Sum; kn6111Sum.FormatString = "{0:C}"; lstSum.Add(kn6111Sum);
            CustomSummaryItem kn17103Sum = new CustomSummaryItem(); kn17103Sum.Name = "Kn17103"; kn17103Sum.Aggregate = GridAggregateFunction.Sum; kn17103Sum.FormatString = "{0:C}"; lstSum.Add(kn17103Sum);
            CustomSummaryItem kn17103ISum = new CustomSummaryItem(); kn17103ISum.Name = "Kn17103i"; kn17103ISum.Aggregate = GridAggregateFunction.Sum; kn17103ISum.FormatString = "{0:C}"; lstSum.Add(kn17103ISum);
            CustomSummaryItem kn27103Sum = new CustomSummaryItem(); kn27103Sum.Name = "Kn27103"; kn27103Sum.Aggregate = GridAggregateFunction.Sum; kn27103Sum.FormatString = "{0:C}"; lstSum.Add(kn27103Sum);
            CustomSummaryItem kn27103ISum = new CustomSummaryItem(); kn27103ISum.Name = "Kn27103i"; kn27103ISum.Aggregate = GridAggregateFunction.Sum; kn27103ISum.FormatString = "{0:C}"; lstSum.Add(kn27103ISum);
            CustomSummaryItem kn37103Sum = new CustomSummaryItem(); kn37103Sum.Name = "Kn37103"; kn37103Sum.Aggregate = GridAggregateFunction.Sum; kn37103Sum.FormatString = "{0:C}"; lstSum.Add(kn37103Sum);
            CustomSummaryItem kn37103ISum = new CustomSummaryItem(); kn37103ISum.Name = "Kn37103i"; kn37103ISum.Aggregate = GridAggregateFunction.Sum; kn37103ISum.FormatString = "{0:C}"; lstSum.Add(kn37103ISum);
            CustomSummaryItem kn7252Sum = new CustomSummaryItem(); kn7252Sum.Name = "Kn7252"; kn7252Sum.Aggregate = GridAggregateFunction.Sum; kn7252Sum.FormatString = "{0:C}"; lstSum.Add(kn7252Sum);
            CustomSummaryItem kn17256Sum = new CustomSummaryItem(); kn17256Sum.Name = "Kn17256"; kn17256Sum.Aggregate = GridAggregateFunction.Sum; kn17256Sum.FormatString = "{0:C}"; lstSum.Add(kn17256Sum);
            CustomSummaryItem kn7316Sum = new CustomSummaryItem(); kn7316Sum.Name = "Kn7316"; kn7316Sum.Aggregate = GridAggregateFunction.Sum; kn7316Sum.FormatString = "{0:C}"; lstSum.Add(kn7316Sum);
            CustomSummaryItem kn7319Sum = new CustomSummaryItem(); kn7319Sum.Name = "Kn7319"; kn7319Sum.Aggregate = GridAggregateFunction.Sum; kn7319Sum.FormatString = "{0:C}"; lstSum.Add(kn7319Sum);
            CustomSummaryItem kn5510Sum = new CustomSummaryItem(); kn5510Sum.Name = "Kn5510"; kn5510Sum.Aggregate = GridAggregateFunction.Sum; kn5510Sum.FormatString = "{0:C}"; lstSum.Add(kn5510Sum);
            CustomSummaryItem kn4857Sum = new CustomSummaryItem(); kn4857Sum.Name = "Kn4857"; kn4857Sum.Aggregate = GridAggregateFunction.Sum; kn4857Sum.FormatString = "{0:C}"; lstSum.Add(kn4857Sum);
            CustomSummaryItem kn159210Sum = new CustomSummaryItem(); kn159210Sum.Name = "Kn159210"; kn159210Sum.Aggregate = GridAggregateFunction.Sum; kn159210Sum.FormatString = "{0:C}"; lstSum.Add(kn159210Sum);
            CustomSummaryItem kn3294Sum = new CustomSummaryItem(); kn3294Sum.Name = "Kn3294"; kn3294Sum.Aggregate = GridAggregateFunction.Sum; kn3294Sum.FormatString = "{0:C}"; lstSum.Add(kn3294Sum);
            CustomSummaryItem odeSum = new CustomSummaryItem(); odeSum.Name = "Odenecek"; odeSum.Aggregate = GridAggregateFunction.Sum; odeSum.FormatString = "{0:C}"; lstSum.Add(odeSum);

            GridViewSummaryRowItem sumTopRow = new GridViewSummaryRowItem();
            sumTopRow.AddRange(lstSum);
            rgv.SummaryRowsBottom.Add(sumTopRow);

            rgv.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed;
            rgv.MasterView.SummaryRows[0].IsPinned = true;
            rgv.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;
        }

        public void HideThkkColumn(RadGridView rgv, List<SgkThkk> lst)
        {
            if ((from x in lst select x.Kn14857).Sum() == 0) rgv.Columns["Kn14857"].IsVisible = false;
            if ((from x in lst select x.Kn15921).Sum() == 0) rgv.Columns["Kn15921"].IsVisible = false;
            if ((from x in lst select x.Kn6645).Sum() == 0) rgv.Columns["Kn6645"].IsVisible = false;
            if ((from x in lst select x.Kn15510).Sum() == 0) rgv.Columns["Kn15510"].IsVisible = false;
            if ((from x in lst select x.Kn2828).Sum() == 0) rgv.Columns["Kn2828"].IsVisible = false;
            if ((from x in lst select x.Kn6111).Sum() == 0) rgv.Columns["Kn6111"].IsVisible = false;
            if ((from x in lst select x.Kn17103).Sum() == 0) rgv.Columns["Kn17103"].IsVisible = false;
            if ((from x in lst select x.Kn17103I).Sum() == 0) rgv.Columns["Kn17103i"].IsVisible = false;
            if ((from x in lst select x.Kn27103).Sum() == 0) rgv.Columns["Kn27103"].IsVisible = false;
            if ((from x in lst select x.Kn27103I).Sum() == 0) rgv.Columns["Kn27103i"].IsVisible = false;
            if ((from x in lst select x.Kn37103).Sum() == 0) rgv.Columns["Kn37103"].IsVisible = false;
            if ((from x in lst select x.Kn37103I).Sum() == 0) rgv.Columns["Kn37103i"].IsVisible = false;
            if ((from x in lst select x.Kn7252).Sum() == 0) rgv.Columns["Kn7252"].IsVisible = false;
            if ((from x in lst select x.Kn17256).Sum() == 0) rgv.Columns["Kn17256"].IsVisible = false;
            if ((from x in lst select x.Kn7316).Sum() == 0) rgv.Columns["Kn7316"].IsVisible = false;
            if ((from x in lst select x.Kn7319).Sum() == 0) rgv.Columns["Kn7319"].IsVisible = false;
            if ((from x in lst select x.Kn5510).Sum() == 0) rgv.Columns["Kn5510"].IsVisible = false;
            if ((from x in lst select x.Kn4857).Sum() == 0) rgv.Columns["Kn4857"].IsVisible = false;
            if ((from x in lst select x.Kn159210).Sum() == 0) rgv.Columns["Kn159210"].IsVisible = false;
            if ((from x in lst select x.Kn3294).Sum() == 0) rgv.Columns["Kn3294"].IsVisible = false;
        }
        #endregion

        #region HizmetListesiHatalar
        public void AnalyzeHl()
        {
            string msg = "";
            if (LstHlp == null || LstHlp.Count == 0) return;
            List<int> cxs = (from x in LstHlp select x.Cx).Distinct().ToList();
            DateTime tr1 = (from x in LstHlp select x.Ya).Min();
            DateTime tr2 = (from x in LstHlp select x.Ya).Max();
            tr2 = new DateTime(tr2.Year, tr2.Month, DateTime.DaysInMonth(tr2.Year, tr2.Month));
            LstIgl = IOC.SgkDataService.GetAllIgl(cxs, tr1, tr2, out msg);

            RadMessageBox.Show($"Hizmet listesi analizinin doğru sonuç verebilmesi için {tr1:dd.MM.yyyy} - {tr2:dd.MM.yyyyy} tarihleri arasındaki işe giriş/işten çıkış hareketlerinin veri tabanında kayıtlı olduğundan emin olun.", "İŞE GİRİŞ İŞTEN ÇIKIŞ LİSTESİ!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, "* SGK'nın bu hizmeti hafta içi mesai saatleri içinde (08:00-12:00 13:00-17:00) çalışmamaktadır.\r\n* SGK sistemindeki işe giriş işten çıkış kayıtları 07.06.2014 tarihinden önceki işlemleri içermez.");
            string hata = IOC.SgkAutomations.HizmetListesiAnalizi(LstHlp, LstIgl);
            FHlReport f = new FHlReport(hata);
            f.ShowDialog();
        }
        public string HizmetListesiAnalizi(List<SgkHlp> lstHlp, List<SgkIgl> lstIgl)
        {
            string basDonem = CheckUpVars.StartDate.ToString("yyyy/MM", CultureInfo.InvariantCulture);
            string bitDonem = CheckUpVars.EndDate.ToString("yyyy/MM", CultureInfo.InvariantCulture);
            string rapor = $"<html><p><span style=\"font-size:14\"><strong>{basDonem} - {bitDonem} &nbsp;DÖNEM &nbsp;ARALIĞINDAKİ &nbsp;HİZMET &nbsp;LİSTESİ &nbsp;ANALİZİ</strong></span></p>";
            List<SgkHlp> lstIptal = (from x in lstHlp where x.Bm == "IPTAL" select x).ToList();
            List<SgkHlp> lstSil = new List<SgkHlp>();
            lstSil.AddRange(lstIptal);
            lstHlp = lstHlp.Except(lstIptal).ToList();

            foreach (SgkHlp ipt in lstIptal)
            {
                foreach (SgkHlp ele in lstHlp)
                {
                    if (ele.Cn == ipt.Cn && ele.Tcno == ipt.Tcno && ele.Utl == ipt.Utl && ele.Itl == ipt.Itl && ele.Gun == ipt.Gun && ele.EGun == ipt.EGun && ele.CGun == ipt.CGun && ele.Icn == ipt.Icn && ele.Egn == ipt.Egn && ele.Ya == ipt.Ya && ele.Bt == ipt.Bt && ele.Kk == ipt.Kk && ele.Cx == ipt.Cx && (ele.Bm == "ASIL" || ele.Bm == "EK"))
                    {
                        lstSil.Add(ele);
                    }
                }
            }

            lstHlp = lstHlp.Except(lstSil).ToList();

            List<int> cxs = (from x in lstHlp orderby x.Cx ascending select x.Cx).Distinct().ToList();
            foreach (int cx in cxs)
            {

                List<DateTime> donemler = (from x in lstHlp where x.Cx == cx orderby x.Ya ascending select x.Ya).Distinct().ToList();
                string cn = (from x in lstHlp where x.Cx == cx select x.Cn).FirstOrDefault();
                rapor += $"<p><span style=\"font-size:12\"><strong>{cn}</strong></p><ul><li>";
                for (int i = 0; i < donemler.Count ; i++)
                {
                    string eksikPers = "", fazlaPers = "", girissizPers = "", iglCikissizPers = "", hlCikissizPers = "", iglGirissizPers = "", hlGirissizPers = "", hldeVarGirisDegil = "" , hldeVarCikisDegil = "";

                    List<SgkHlp> ilk = (from x in lstHlp where x.Cx == cx && x.Ya == donemler[i] select x).ToList();
                    List<SgkHlp> son = donemler.Count > 1 && i < donemler.Count - 1 ? (from x in lstHlp where x.Cx == cx && x.Ya == donemler[i + 1] select x).ToList() : null;
                    List<SgkIgl> igl = FilterIgl(lstIgl, cx, donemler[i]);

                    string oncekiDonem = donemler[i].ToString("yyyy/MM", CultureInfo.InvariantCulture);
                    string sonrakiDonem = donemler[i].AddMonths(1).ToString("yyyy/MM", CultureInfo.InvariantCulture);

                    List<string> olmamasiGerekenler = new List<string>();
                    List<string> eksikOlanlar = new List<string>();
                    List<string> girisiOlmayanlar = new List<string>();
                    List<string> igldeCikisiOlmayanlar = new List<string>();
                    List<string> hldeCikisiOlmayanlar = new List<string>();
                    List<string> igldGirisiOlmayanlar = new List<string>();
                    List<string> hldeGirisiOlmayanlar = new List<string>();
                    List<string> hldeVarAmaGirisBelirtilmemis = new List<string>();
                    List<string> hldeVarAmaCikisBelirtilmemis = new List<string>();

                    if (son != null) { (olmamasiGerekenler, eksikOlanlar, girisiOlmayanlar) = HlFarkBul(ilk, son); }
                    (igldeCikisiOlmayanlar, hldeCikisiOlmayanlar, igldGirisiOlmayanlar, hldeGirisiOlmayanlar, hldeVarAmaGirisBelirtilmemis, hldeVarAmaCikisBelirtilmemis) = IseGirisCikisKarsilastir(ilk, igl);

                    foreach (var item in eksikOlanlar)
                    {
                        eksikPers += $"{item}, ";
                    }
                    foreach (var item in olmamasiGerekenler)
                    {
                        fazlaPers += $"{item}, ";
                    }
                    foreach (var item in girisiOlmayanlar)
                    {
                        girissizPers += $"{item}, ";
                    }
                    foreach (var item in igldeCikisiOlmayanlar)
                    {
                        iglCikissizPers += $"{item}, ";
                    }
                    foreach (var item in hldeCikisiOlmayanlar)
                    {
                        hlCikissizPers += $"{item}, ";
                    }
                    foreach (var item in igldGirisiOlmayanlar)
                    {
                        iglGirissizPers += $"{item}, ";
                    }
                    foreach (var item in hldeGirisiOlmayanlar)
                    {
                        hlGirissizPers += $"{item}, ";
                    }
                    foreach (var item in hldeVarAmaGirisBelirtilmemis)
                    {
                        hldeVarGirisDegil += $"{item}, ";
                    }
                    foreach (var item in hldeVarAmaCikisBelirtilmemis)
                    {
                        hldeVarCikisDegil += $"{item}, ";
                    }

                    rapor += $"<li><strong>{oncekiDonem}</strong><ul>";

                    if (eksikPers != "" || fazlaPers != "" || girissizPers != "" || iglCikissizPers != "" || hlCikissizPers != "" || iglGirissizPers != "" || hlGirissizPers != "" || hldeVarGirisDegil != "" || hldeVarCikisDegil != "")
                    {
                        if (eksikPers != "")
                        {
                            rapor += eksikPers.Length > 13 ? $"<li><span style=\"color:red\">EKSİK PERSONELLER:</span> {eksikPers} sicil numaralı personeller {sonrakiDonem} döneminde bulunması gerekmekte olup Hizmet Listesinde bulunmamaktadır. <em>{sonrakiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">EKSİK PERSONELLER:</span> {eksikPers} sicil numaralı personel {sonrakiDonem} döneminde bulunması gerekmekte olup Hizmet Listesinde bulunmamaktadır. <em>{sonrakiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                        if (fazlaPers != "")
                        {
                            rapor += fazlaPers.Length > 13 ? $"<li><span style=\"color:red\">FAZLA PERSONELLER:</span> {fazlaPers} sicil numaralı personeller {oncekiDonem} döneminde işten çıktıkları ve tekrar işe dönmedikleri halde {sonrakiDonem} dönemi Hizmet Listesinde bulunmaktadır. <em>{sonrakiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">FAZLA PERSONELLER:</span> {fazlaPers} sicil numaralı personel {oncekiDonem} döneminde işten çıktığı ve tekrar işe dönmediği {sonrakiDonem} dönemi Hizmet Listesinde bulunmaktadır. <em>{sonrakiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                        if (girissizPers != "")
                        {
                            rapor += girissizPers.Length > 13 ? $"<li><span style=\"color:red\">GİRİŞ KAYDI OLMAYAN PERSONELLER:</span> {girissizPers} sicil numaralı personeller {oncekiDonem} dönemi listesinde olmadıkları ve {sonrakiDonem} döneminde de giriş kayıtları olmadıkları halde {sonrakiDonem} Hizmet Listesinde bulunmaktadır. <em>{sonrakiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">GİRİŞ KAYDI OLMAYAN PERSONELLER:</span> {girissizPers} sicil numaralı personel {oncekiDonem} dönemi listesinde olmadığı ve {sonrakiDonem} döneminde de işe giriş kaydı olmadığı halde {sonrakiDonem} dönemi Hizmet Listesinde bulunmaktadır. <em>{sonrakiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                        if (iglCikissizPers != "")
                        {
                            rapor += iglCikissizPers.Length > 13 ? $"<li><span style=\"color:red\">İŞTEN ÇIKIŞ BİLDİRİMİ OLMAYAN PERSONELLER:</span> {iglCikissizPers} sicil numaralı personeller {oncekiDonem} dönemi Hizmet Listesinde işten çıktı olarak göründükleri halde, işten çıkış bildirgeleri verilmemiş görünmektedir</li>" : $"<li><span style=\"color:red\">ÇIKIŞ BİLDİRİMİ OLMAYAN PERSONELLER:</span> {iglCikissizPers} sicil numaralı personel {oncekiDonem} dönemi Hizmet Listesinde işten çıktı olarak göründüğü halde, işten çıkış bildirgesi verilmemiş görünmektedir</li>";
                        }
                        if (iglGirissizPers != "")
                        {
                            rapor += iglGirissizPers.Length > 13 ? $"<li><span style=\"color:red\">İŞE GİRİŞ BİLDİRİMİ OLMAYAN PERSONELLER:</span> {iglGirissizPers} sicil numaralı personeller {oncekiDonem} dönemi Hizmet Listesinde işe girdi olarak göründükleri halde, işe giriş bildirgeleri verilmemiş görünmektedir</li>" : $"<li><span style=\"color:red\">GİRİŞ BİLDİRİMİ OLMAYAN PERSONELLER:</span> {iglGirissizPers} sicil numaralı personel {oncekiDonem} dönemi Hizmet Listesinde işe girdi olarak göründüğü halde, işe giriş bildirgesi verilmemiş görünmektedir</li>";
                        }
                        if (hlCikissizPers != "")
                        {
                            rapor += hlCikissizPers.Length > 13 ? $"<li><span style=\"color:red\">İŞTEN ÇIKIŞ BİLDİRGESİ HİZMET LİSTESİNDE BELİRTİLMEYEN PERSONELLER:</span> {hlCikissizPers} sicil numaralı personeller {oncekiDonem} döneminde işten çıktıkları halde {oncekiDonem} dönemi Hizmet Listesinde hiçbir şekilde görünmemektedirler <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">İŞTEN ÇIKIŞ BİLDİRGESİ HİZMET LİSTESİNDE BELİRTİLMEYEN PERSONELLER:</span> {hlCikissizPers} sicil numaralı personel {oncekiDonem} döneminde işten çıktığı halde {oncekiDonem} dönemi Hizmet Listesinde hiçbir şekilde görünmemektedir <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                        if (hlGirissizPers != "")
                        {
                            rapor += hlGirissizPers.Length > 13 ? $"<li><span style=\"color:red\">İŞE GİRİŞ BİLDİRGESİ HİZMET LİSTESİNDE BELİRTİLMEYEN PERSONELLER:</span> {hlGirissizPers} sicil numaralı personeller {oncekiDonem} döneminde işe girdikleri halde {oncekiDonem} dönemi Hizmet Listesinde görünmemektedirler <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">İŞE GİRİŞ BİLDİRGESİ HİZMET LİSTESİNDE BELİRTİLMEYEN PERSONELLER:</span> {hlGirissizPers} sicil numaralı personel {oncekiDonem} döneminde işe girdiği halde {oncekiDonem} dönemi Hizmet Listesinde görünmemektedir <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                        if (hldeVarCikisDegil != "")
                        {
                            rapor += hldeVarCikisDegil.Length > 13 ? $"<li><span style=\"color:red\">İŞTEN ÇIKIŞ BİLDİRGESİ OLUP HİZMET LİSTESİNDE ÇALIŞIYOR OLARAK GÖRÜNEN PERSONELLER:</span> {hldeVarCikisDegil} sicil numaralı personellerin {oncekiDonem} döneminde işten çıkış bildirgeleri olduğu halde {oncekiDonem} dönemi Hizmet Listesinde çalışıyor olarak görünmektedirler <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">İŞTEN ÇIKIŞ BİLDİRGESİ OLUP HİZMET LİSTESİNDE ÇALIŞIYOR OLARAK GÖRÜNEN PERSONELLER:</span> {hldeVarCikisDegil} sicil numaralı personelin {oncekiDonem} döneminde işten çıkış bildirgesi olduğu halde {oncekiDonem} dönemi Hizmet Listesinde çalışıyor olarak görünmektedir <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                        if (hldeVarGirisDegil != "")
                        {
                            rapor += hldeVarGirisDegil.Length > 13 ? $"<li><span style=\"color:red\">İŞE GİRİŞ BİLDİRGESİ OLUP HİZMET LİSTESİNDE ÇALIŞIYOR OLARAK GÖRÜNENEN PERSONELLER:</span> {hldeVarGirisDegil} sicil numaralı personellerin {oncekiDonem} döneminde işe giriş bildirgeleri olduğu halde {oncekiDonem} dönemi Hizmet Listesinde yeni giriş yapmış değil zaten çalışan olarak görünmektedirler <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>" : $"<li><span style=\"color:red\">İŞE GİRİŞ BİLDİRGESİ OLUP HİZMET LİSTESİNDE ÇALIŞIYOR OLARAK GÖRÜNENEN PERSONELLER:</span> {hldeVarGirisDegil} sicil numaralı personel {oncekiDonem} döneminde işe giriş bildirgesi olduğu halde {oncekiDonem} dönemi Hizmet Listesinde yeni giriş yapmış değil zaten çalışan olarak görünmektedir <em>{oncekiDonem} Hizmet Listenizi kontrol ediniz</em></li>";
                        }
                    }
                    else
                    {
                        rapor += $"<ul><li>Herhangi bir sorun bulunmadı</li></ul></li>";
                    }
                    rapor += $"</ul></li>";
                }

                rapor += "</ul>";
            }
            rapor += "</html>";
            return rapor;
        }
        public (List<string>, List<string>, List<string>) HlFarkBul(List<SgkHlp> ilk, List<SgkHlp> son)
        {
            List<string> gecenAyCikanlar = new List<string>();
            List<string> olmamasiGerekenler = new List<string>();
            List<string> olmasiGerekenler = new List<string>();
            List<string> eksikOlanlar = new List<string>();
            List<string> girisiOlmayanlar = new List<string>();
            foreach (SgkHlp ele in ilk)
            {
                if (ele.Icn != 0) // çıkış var
                {
                    bool ayniAydaGeriDonmusMu = GeriDonmusMu(ilk, ele.Tcno, ele.CGun);
                    if (!ayniAydaGeriDonmusMu)
                    {
                        gecenAyCikanlar.Add(ele.Tcno);
                    }
                }
                else
                {
                    olmasiGerekenler.Add(ele.Tcno);
                }
            }

            if (gecenAyCikanlar.Count != 0) // geçen ay çıktığı halde sonraki ayda olanlar
            {
                olmamasiGerekenler = (from x in son where gecenAyCikanlar.Contains(x.Tcno) && x.GGun == 0 select x.Tcno).ToList();
            }

            foreach (string og in olmasiGerekenler)
            {
                bool varMi = false;
                foreach (SgkHlp ele in son)
                {
                    if(ele.Tcno == og) { varMi = true; break; }
                }
                if (!varMi) { eksikOlanlar.Add(og); }
            }
            
            bool gecenAydaVarmi;
            foreach (SgkHlp ele in son) // girişi olmadığı halde listede olanlar
            {
                gecenAydaVarmi = false;
                if (ele.GGun == 0)
                {
                    foreach (SgkHlp elex in ilk)
                    {
                        if(elex.Tcno == ele.Tcno) { gecenAydaVarmi = true; break; }
                    }
                    if (!gecenAydaVarmi) { girisiOlmayanlar.Add(ele.Tcno); }
                }
                
            }

            return (olmamasiGerekenler, eksikOlanlar, girisiOlmayanlar);
        }
        public bool GeriDonmusMu(List<SgkHlp> lst, string tcno, int cGun) {

            List<SgkHlp> ele = (from x in lst where (x.Tcno == tcno && x.GGun >= cGun) select x).ToList();
            if (ele == null || ele.Count == 0) { return false; }
            else
            {
                if (ele[ele.Count - 1].GGun > 0) return true; // geri dönmüş
            }
            return false;
        }
        public List<SgkIgl> FilterIgl(List<SgkIgl> lstIgl, int cx, DateTime ilkDonem)
        {
            DateTime sonDonem = ilkDonem.AddMonths(1);
            List<SgkIgl> igl = (from x in lstIgl where x.Cx == cx && x.Tr >= ilkDonem && x.Tr < sonDonem orderby x.Tc, x.Gc, x.Ist descending, x.Isa descending select x).ToList();
            List<SgkIgl> excludeList = new List<SgkIgl>();
            //foreach (SgkIgl item in igl)
            //{
            //    Console.WriteLine($"{item.tc} \t {item.ads} \t {item.gc} \t {item.tr} \t {item.isl} \t {item.ist} \t {item.isa}");
            //}
            for (int i = 0; i < igl.Count; i++)
            {
                if (igl[i].Isl == "Güncelleme")
                {
                    for (int j = i+1; j < igl.Count; j++)
                    {
                        if (igl[j].Isl == "Güncelleme") { excludeList.Add(igl[j]); }
                        if (igl[j].Isl == "Kayıt") { excludeList.Add(igl[j]); i = j ; break; }
                    }
                }
                else if (igl[i].Isl == "Silme")
                {
                    excludeList.Add(igl[i]);
                    for (int j = i + 1; j < igl.Count; j++)
                    {
                        if (igl[j].Isl == "Güncelleme") { excludeList.Add(igl[j]); }
                        if (igl[j].Isl == "Kayıt") { excludeList.Add(igl[j]); i = j ; break; }
                    }
                }

            }

            igl = igl.Except(excludeList).ToList();
            
            return igl;
        }
        public (List<string>, List<string>, List<string>, List<string>, List<string>, List<string>) IseGirisCikisKarsilastir(List<SgkHlp> hlp, List<SgkIgl> igl)
        {
            List<string> igldeGirisiOlmayanlar = new List<string>();
            List<string> igldeCikisiOlmayanlar = new List<string>();
            List<string> hldeGirisiOlmayanlar = new List<string>();
            List<string> hldeCikisiOlmayanlar = new List<string>();
            List<string> hldeVarAmaGirisBelirtilmemis = new List<string>();
            List<string> hldeVarAmaCikisBelirtilmemis = new List<string>();

            List<SgkHlp> hlpG = (from x in hlp where x.GGun > 0  select x).ToList();
            List<SgkHlp> hlpC = (from x in hlp where x.CGun > 0  select x).ToList();
            List<SgkIgl> iglC = (from x in igl where x.Gc == "Çıkış" select x).ToList();
            List<SgkIgl> iglG = (from x in igl where x.Gc == "Giriş" select x).ToList();

            foreach (SgkIgl per in iglC)
            {
                bool hldeCikisVarmi = false, hldeVarmi = false;
                foreach (SgkHlp ele in hlpC)
                {
                    string cgunS = ele.CGun < 1000 ? $"0{ele.CGun}".Substring(0, 2) : ele.CGun.ToString().Substring(0, 2);
                    int cgunD = Convert.ToInt32(cgunS);
                    DateTime cGun = new DateTime(ele.Ya.Year, ele.Ya.Month, cgunD);
                    if (per.Tc == ele.Tcno)
                    {
                        if (per.Tr == cGun) { hldeCikisVarmi = true; hldeVarmi = true; break; }
                        else { hldeVarmi = true; break; }
                    }
                }
                if (!hldeVarmi) { hldeCikisiOlmayanlar.Add(per.Tc); }
                else if (!hldeCikisVarmi) { hldeVarAmaCikisBelirtilmemis.Add(per.Tc); }
            }
            foreach (SgkIgl per in iglG) 
            {
                bool hldeGirisVarmi = false, hldeVarmi = false; ;
                foreach (SgkHlp ele in hlpG)
                {
                    string ggunS = ele.GGun < 1000 ? $"0{ele.GGun}".Substring(0, 2) : ele.GGun.ToString().Substring(0, 2);
                    int ggunD = Convert.ToInt32(ggunS);
                    DateTime gGun = new DateTime(ele.Ya.Year, ele.Ya.Month, ggunD);
                    if (per.Tc == ele.Tcno)
                    {
                        if (per.Tr == gGun) { hldeGirisVarmi = true; hldeVarmi = true; break; }
                        else { hldeVarmi = true; break; }
                    }
                    
                }
                if (!hldeVarmi) { hldeGirisiOlmayanlar.Add(per.Tc); }
                else if (!hldeGirisVarmi) { hldeVarAmaGirisBelirtilmemis.Add(per.Tc); }
            }
            foreach (SgkHlp ele in hlpC)
            { 
                string cgunS = ele.CGun < 1000 ? $"0{ele.CGun}".Substring(0, 2) : ele.CGun.ToString().Substring(0, 2);
                int cgunD = Convert.ToInt32(cgunS);
                DateTime cGun = new DateTime(ele.Ya.Year, ele.Ya.Month, cgunD);
                
                bool cikisVarmi = false;
                foreach (SgkIgl per in iglC)
                {
                    if (per.Tc == ele.Tcno && per.Tr == cGun) { cikisVarmi = true; break; }
                }
                if (!cikisVarmi) { igldeCikisiOlmayanlar.Add(ele.Tcno); }
            }
            foreach (SgkHlp ele in hlpG)
            {
                string ggunS = ele.GGun < 1000 ? $"0{ele.GGun}".Substring(0, 2) : ele.GGun.ToString().Substring(0, 2);
                int ggunD = Convert.ToInt32(ggunS);
                DateTime gGun = new DateTime(ele.Ya.Year, ele.Ya.Month, ggunD);

                bool girisVarmi = false;
                foreach (SgkIgl per in iglG)
                {
                    if (per.Tc == ele.Tcno && per.Tr == gGun) { girisVarmi = true; break; }
                }
                if (!girisVarmi) { igldeGirisiOlmayanlar.Add(ele.Tcno); }
            }

            return (igldeCikisiOlmayanlar, hldeCikisiOlmayanlar, igldeGirisiOlmayanlar, hldeGirisiOlmayanlar, hldeVarAmaGirisBelirtilmemis, hldeVarAmaCikisBelirtilmemis);
        }
        #endregion
    }
    public class CustomSummaryItem : GridViewSummaryItem
    {
        public CustomSummaryItem()
            : base()
        { }
        public decimal Subtruct { get; set; } = 0;
        public override object Evaluate(IHierarchicalRow row)
        {
            //if (Name == "tya") return base.Evaluate(row);
            decimal totalPos = 0;
            foreach (GridViewRowInfo childRow in row.ChildRows)
            {
                if ((childRow is GridViewGroupRowInfo) == false)
                {

                    string bm = childRow.Cells["Bm"].Value.ToString();

                    if (bm != "İPTAL")
                    {
                        totalPos += Convert.ToDecimal(childRow.Cells[this.Name].Value);
                    }
                    else
                    {
                        Subtruct += Convert.ToDecimal(childRow.Cells[this.Name].Value);
                    }
                }

            }
            if (row is MasterGridViewTemplate)
            {
                return Convert.ToDecimal(base.Evaluate(row)) - Subtruct;
            }
            return totalPos;
        }
    }
}
