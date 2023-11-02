using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Common
{

    public partial class FReportDetails : Telerik.WinControls.UI.RadForm
    {
        List<SourceIgb> sourceIgb = new List<SourceIgb>();
        List<SourceSpvudk> sourceSpvudk = new List<SourceSpvudk>();
        List<SourceSraod> sourceSraod = new List<SourceSraod>();
        
        public FReportDetails(List<SourceIgb> sourceIgBs, List<SourceSpvudk> sourceSpvudKs, List<SourceSraod> sourceSraoDs)
        {
            PrintDialogsLocalizationProvider.CurrentProvider = new MyPrintDialogsLocalizationProvider();
            ColorDialogLocalizationProvider.CurrentProvider = new CustomColorDialogLocalizationProvider();
            InitializeComponent();
            sourceIgb = sourceIgBs;
            sourceSpvudk = sourceSpvudKs;
            sourceSraod = sourceSraoDs;
        }

        private void fReportDetails_Load(object sender, EventArgs e)
        {
            SetView();
        }

        private void SetView()
        {
            List<CommandBarButton> radButtons = new List<CommandBarButton>() { btnOpenFile, btnHideMessage };
            foreach (CommandBarButton btn in radButtons)
            {
                btn.DrawBorder = true;
                btn.BorderColor = Color.FromArgb(120, 148, 186);
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            cbreMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            lblMessage.Text = string.Empty;
            rgvIGB.DataSource = sourceIgb;
            rgvSRAOD.DataSource = sourceSraod;
            rgvSPVUDK.DataSource = sourceSpvudk;
            rgvIGB.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvSPVUDK.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvSRAOD.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvIGB.TableElement.RowHeight = 24;
            rgvSPVUDK.TableElement.RowHeight = 24;
            rgvSRAOD.TableElement.RowHeight = 24;
            Text = $"{DetailsNameTc.Name} ({DetailsNameTc.Tcno}) adlı personelin rapor ayrıntıları.";
            labelPersonel.Text = $"{DetailsNameTc.Name} ({DetailsNameTc.Tcno})";
            if (rgvIGB.RowCount > 0)
            {
                pnlSRAOD.Dispose();
                pnlIGB.Height = rgvIGB.Height + 16;
                pnlSPVUDK.Height = rgvSPVUDK.Height + 16;
                Height = pnlButtons.Height + pnlIGB.Height + pnlSPVUDK.Height + 40;
                if (rgvSPVUDK.RowCount == 0)
                {
                    pnlSPVUDK.Dispose();
                    Height = pnlButtons.Height + pnlIGB.Height + 50;
                }
            }
            else
            {
                pnlIGB.Dispose();
                pnlSPVUDK.Dispose();
                pnlSRAOD.Height = rgvSRAOD.Height + 16;
                Height = pnlButtons.Height + pnlSRAOD.Height + 50;
            }
            MinimumSize = new Size(816, Height);
            MaximumSize = new Size(816, Height);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnOpenFile.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            string msg = "";
            string title = $"{DetailsNameTc.Name}";
            bool result = false;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (rgvIGB.RowCount > 0)
                {
                    if (rgvSPVUDK.RowCount > 0)
                    {
                        result = IOC.ExportService.CreateFileForReportDetails(title, sourceIgb, sourceSpvudk, null, out msg);
                    }
                    else
                    {
                       result =  IOC.ExportService.CreateFileForReportDetails(title, sourceIgb, null, null, out msg);
                    }
                }
                else
                {
                    result = IOC.ExportService.CreateFileForReportDetails(title, null, null, sourceSraod, out msg);
                }
                if (result)
                {
                    IOC.ExportService.Save(DetailsNameTc.Name, out msg);
                    btnOpenFile.Visibility = msg.Contains("iptal")? Telerik.WinControls.ElementVisibility.Collapsed :  Telerik.WinControls.ElementVisibility.Visible;
                }
                lblMessage.Text = msg;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Dosya oluşturulamadı! Hata: {ex.Message.ToString()}"; btnOpenFile.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            DrawToBitmap(bmp, new Rectangle(Point.Empty, Size));

            Rectangle cloneRect = new Rectangle(0, 80, Width, Height - 81);
            System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
            Bitmap cloneBitmap = bmp.Clone(cloneRect, format);
            PrintablePanel printablePanel = new PrintablePanel(cloneBitmap);
            printablePanel.PrintPreview();

        }
        
        private void btnHideMessage_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            cbreMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }

        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblMessage.Text == string.Empty)
            {
                cbreMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            }
            else
            {
                cbreMessage.Visibility = Telerik.WinControls.ElementVisibility.Visible; 
            }
        }
    }
}
