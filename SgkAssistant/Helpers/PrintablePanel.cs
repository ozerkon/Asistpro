using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace SgkAssistant.Helpers
{
    public class PrintablePanel : RadPanel, IPrintable
    {
        Bitmap bmp;
        public PrintablePanel(Bitmap bitmap)
        {
            bmp = bitmap;
        }

        public int BeginPrint(RadPrintDocument sender, PrintEventArgs args)
        {
            return 1;
        }

        public bool EndPrint(RadPrintDocument sender, PrintEventArgs args)
        {
            return true;
        }

        public Form GetSettingsDialog(RadPrintDocument document)
        {
            return new PrintSettingsDialog(document);
        }
        public bool PrintPage(int pageNumber, RadPrintDocument sender, PrintPageEventArgs args)
        {
            args.Graphics.DrawImage(bmp, Point.Empty);
            return false;
        }
        public void Print()
        {
            RadPrintDocument doc = this.CreatePrintDocument();
            doc.Print();
        }

        public void PrintPreview()
        {
            RadPrintDocument doc = this.CreatePrintDocument();
            RadPrintPreviewDialog dialog = new RadPrintPreviewDialog(doc);

            dialog.ThemeName = this.ThemeName;
            dialog.ShowDialog();
        }
        private RadPrintDocument CreatePrintDocument()
        {
            RadPrintDocument doc = new RadPrintDocument();
            doc.AssociatedObject = this;
            return doc;
        }
    }
}
