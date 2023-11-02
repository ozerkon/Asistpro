using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Models.Common;
using System;
using System.Linq;
using System.Text;
using UglyToad.PdfPig.Content;

namespace Models.Domain
{
    public class SgkAssistantBase
    {
        public int Id { get; set; }
        public int Cx  { get; set; }
        public int Un { get; set; } = GlobalVars.ActiveUser;
        public DateTime Cd { get; set; }

        public decimal SetPoints(string source)
        {

            source = source.Trim();
            int pointPos = source.IndexOf(".");
            int commaPos = source.IndexOf(",");
            int negativePos = source.Contains("-") ? source.IndexOf("-") : -1;

            //Console.WriteLine($"\nsource={source} pointPos={pointPos} commaPos={commaPos} negativePos={negativePos}");

            int negativeMult = negativePos >= 0 ? -1 : 1;
            string decimalPart = "";
            string integerPart = "";
            if (commaPos > 0 && commaPos > pointPos && pointPos > 0) // xx.xxx,yy
            {
                decimalPart = source.Substring(commaPos + 1);
                integerPart = source.Substring(0, commaPos).Replace(".", "");
            }
            else if (commaPos > 0 && commaPos > pointPos && pointPos == -1) // xxx,yy
            {
                decimalPart = source.Substring(commaPos + 1);
                integerPart = source.Substring(0, commaPos);
            }
            else if (pointPos > 0 && pointPos > commaPos && commaPos > 0) // xx,xxx.yy
            {
                decimalPart = source.Substring(pointPos + 1);
                integerPart = source.Substring(0, pointPos).Replace(",", "");
            }
            else if (pointPos > 0 && pointPos > commaPos && commaPos == -1) // xxx.yy
            {
                decimalPart = source.Substring(pointPos + 1);
                integerPart = source.Substring(0, pointPos);
            }
            else if (commaPos == pointPos && commaPos == -1) //  xxxx 
            {
                decimalPart = "0";
                integerPart = source;
            }
            else if (commaPos == 0 && pointPos == -1) // ,xx
            {
                decimalPart = source.Substring(commaPos + 1);
                integerPart = "0";
            }
            else if (pointPos == 0 && commaPos == -1) // .xx
            {
                decimalPart = source.Substring(pointPos + 1);
                integerPart = "0";
            }
            //Console.WriteLine($"decimalPart={decimalPart} integerPart={integerPart} ");
            integerPart = integerPart.Replace("-", "");
            if (integerPart.Equals(""))
            {
                integerPart = "0";
            }
            return negativeMult * ((100 * Convert.ToInt32(integerPart) + Convert.ToInt32(decimalPart)) / 100m);
        }
        public static string ReadPdfFile(string path, out string msg)
        {
            StringBuilder processed = new StringBuilder();
            msg = "";
            int step = 1000;
            try
            {
                PdfReader reader = new PdfReader(path);  //new PdfReader(InputFileFullPath(filePdf));
                                                         //   SimpleTextExtractionStrategy strategy;
                step = 2000;
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    step = 2000 + i;
                    String text = PdfTextExtractor.GetTextFromPage(reader, i);
                    step = 20000 + i;
                    processed.Append(text + "\n");
                }



                //var pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(path));
                //StringBuilder processed = new StringBuilder();

                //string text = "";

                //for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
                //{
                //    ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                //    PdfPage page = pdfDocument.GetPage(i);
                //    text = PdfTextExtractor.GetTextFromPage(page, strategy);
                //    processed.Append(text + "\n");
                //}
                return processed.ToString();
            }
            catch (Exception ex)
            {
                msg = $"Hata {step} : "+ ex.Message;
                return null;
            }
        }
        public static string ReadPdfByPdfPig(string path, out string msg)
        {
            msg = ""; string text = "";
            try
            {
                using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(path))
                {
                    StringBuilder builder = new StringBuilder();

                    foreach (Page page in document.GetPages())
                    {
                        var wordsList = page.GetWords().GroupBy(x => x.BoundingBox.Bottom);

                        foreach (var word in wordsList)
                        {
                            foreach (var item in word)
                            {
                                builder.Append(item.Text + " ");
                            }
                            builder.Append("\n");
                            builder.Replace(" \n", "\n");
                        }
                        builder.Append("\n");
                    }
                    text = builder.ToString();
                }
                return text;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
       
    }
}



