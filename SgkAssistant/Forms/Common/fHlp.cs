using Models.Common;
using Models.Domain;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FHlp : RadForm
    {
        public List<SgkHlp> LstHlp { get; set; } = new List<SgkHlp>();
        public List<SgkIgl> LstIgl { get; set; } = new List<SgkIgl>();
        public FHlp(List<SgkHlp> lstHlp, List<SgkIgl> lstIgl)
        {
            LstHlp = lstHlp;
            LstIgl = lstIgl;
            InitializeComponent();
        }
        string msg;
        private void fHl_Load(object sender, EventArgs e)
        {
            rgvMossip.DataSource = LstHlp;
            IOC.SgkAutomations.SetRgvHlp(rgvMossip);
            pnlMessage.Visible = false;
            RgvHelpers.HLP_Colorize(rgvMossip);
        }
        private void rgvMossip_ViewCellFormatting(object sender, CellFormattingEventArgs e)
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
                e.CellElement.TextAlignment = e.Column.FieldName == "Ads" ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleRight;
                e.CellElement.Padding = e.Column.FieldName == "Ads" ? new Padding(20, 0, 0, 0) : new Padding(0, 0, 20, 0);
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
        private void rgvMossip_ViewRowFormatting(object sender, RowFormattingEventArgs e)
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
        }

        private void rgvMossip_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            string cin = e.CellElement.ColumnInfo.Name;
            if (cin == "Cn" || cin == "Ads" || cin == "Mk" || cin == "Bm" || cin == "Bt" || cin == "Kk") e.CellElement.Padding = new Padding(20, 0, 0, 0);
            else if (cin == "Utl" || cin == "Itl" || cin == "Gun" || cin == "Ucg" || cin == "EGun" || cin == "GGun" || cin == "CGun" || cin == "Icn" || cin == "Egn") e.CellElement.Padding = new Padding(0, 0, 20, 0);
        }

        private void btnAnalyzeHL_Click(object sender, EventArgs e)
        {
            IOC.SgkAutomations.LstHlp = LstHlp;
            IOC.SgkAutomations.AnalyzeHl();
        }

        private void btnPrintHLP_Click(object sender, EventArgs e)
        {
            if (!PrepareForPrint()) { return; }

            RadPrintDocument document = new RadPrintDocument();
            document.DefaultPageSettings.Landscape = true;
            document.DefaultPageSettings.PrinterSettings.Copies = 2;
            document.AssociatedObject = rgvMossip; 
            GridPrintStyle style = new GridPrintStyle();
            int gridWidth = 0;
            //int i = 0;
            Margins margins = document.DefaultPageSettings.Margins;
            int documentWidth = document.DefaultPageSettings.Bounds.Width;
            documentWidth -= margins.Left + margins.Right;

            if (gridWidth < documentWidth)
            {
                style.CellFont = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Italic);
                document.DefaultPageSettings.Landscape = false;
            }
            else
            {
                style.CellFont = new Font(FontFamily.GenericSansSerif, 6.0F);
                document.DefaultPageSettings.Landscape = true;
            }

            style.FitWidthMode = PrintFitWidthMode.FitPageWidth;
            style.PrintGrouping = true;
            style.PrintSummaries = true;
            style.PrintHeaderOnEachPage = true;
            style.PrintHiddenColumns = false;
            rgvMossip.PrintStyle = style;

            document.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
            document.AssociatedObject = rgvMossip;

            RadPrintPreviewDialog dialog = new RadPrintPreviewDialog();
            dialog.Document = document;
            dialog.ShowDialog();
            IOC.SgkAutomations.SetRgvHlp(rgvMossip);
        }

        private bool PrepareForPrint()
        {
            if (LstHlp.Count == 0) { lblMessage.Text = "Yazdırılacak hizmet listesi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }

            rgvMossip.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
            rgvMossip.BestFitColumns();
            return true;
        }

        private void btnExcelHLP_Click(object sender, EventArgs e)
        {
            
            IOC.ExportService.CreateFile(out msg);
            bool goXlsHlp = false;
            string title;
            GlobalVars.LstHlp = LstHlp; 
            if (LstHlp != null && LstHlp.Count > 0) { title = "HİZMET LİSTESİ"; IOC.ExportService.CreateXlsHesap(title, 6, out msg); goXlsHlp = true; }
            if (!goXlsHlp) { lblMessage.Text = "Excel'e aktarılacak bir tablo bulunamadı."; return; }
            title = "HİZMET LİSTESİ";
            if (IOC.ExportService.Save(title, out msg))
            {
                lblMessage.Text = msg; pnlMessage.Visible = true;
            }
            else
            {
                lblMessage.Text = msg;
                pnlMessage.Visible = true;
                btnOpenFile.Visible = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
    }
}
