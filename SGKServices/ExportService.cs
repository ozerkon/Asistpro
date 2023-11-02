using Models.Common;
using Models.Domain;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SGKServices
{
    public class ExportService
	{
        public IWorkbook Workbook { get; set; }
        public string FileName { get; set; }
        ISheet sheet; //,  yesNoSheet;

        public ExportService()
        {
        }
        
        public bool CreateFile(out string msg)
        {
            msg = "";
            try
            {
                 Workbook = new XSSFWorkbook();
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        private ICellStyle GetLinkCellStyle()
        {
            ICellStyle style = Workbook.CreateCellStyle();
            IFont font = Workbook.CreateFont();
            font.Underline = FontUnderlineType.Single;
            font.Color = HSSFColor.Red.Index;
            font.IsBold = true;
            font.IsItalic = true;
            font.FontHeightInPoints = 11;
            style.SetFont(font);
            style.Alignment = HorizontalAlignment.Right;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.LeftBorderColor = 0;
            style.TopBorderColor = 0;
            style.RightBorderColor = 0;
            style.BottomBorderColor = 0;
            return style;
        }
        private ICellStyle GetNumberCellStyle()
        {
            // Veri hücreleri için stil oluştur
            ICellStyle numberCellStyle = Workbook.CreateCellStyle();
            IFont numberCellFont = Workbook.CreateFont();
            numberCellFont.IsBold = false;
            numberCellFont.FontHeightInPoints = 11;
            numberCellStyle.SetFont(numberCellFont);
            numberCellStyle.Alignment = HorizontalAlignment.Right;
            numberCellStyle.BorderLeft = BorderStyle.Thin;
            numberCellStyle.BorderTop = BorderStyle.Thin;
            numberCellStyle.BorderRight = BorderStyle.Thin;
            numberCellStyle.BorderBottom = BorderStyle.Thin;
            numberCellStyle.LeftBorderColor = 0;
            numberCellStyle.TopBorderColor = 0;
            numberCellStyle.RightBorderColor = 0;
            numberCellStyle.BottomBorderColor = 0;
            return numberCellStyle;
        }
        private ICellStyle GetTitleCellStyle()
        {
            IFont titleFont = Workbook.CreateFont();
            titleFont.IsBold = true;
            titleFont.FontHeightInPoints = 16;
            ICellStyle bhs = Workbook.CreateCellStyle();
            bhs.SetFont(titleFont);
            bhs.Alignment = HorizontalAlignment.Center;
            bhs.VerticalAlignment = VerticalAlignment.Center;
            return bhs;
        }
        private ICellStyle GetTopBorderCellStyle()
        {
            // Veri hücreleri için stil oluştur
            ICellStyle topBorderCellStyle = Workbook.CreateCellStyle();
            IFont dataCellFont = Workbook.CreateFont();
            dataCellFont.IsBold = false;
            dataCellFont.FontHeightInPoints = 11;
            topBorderCellStyle.SetFont(dataCellFont);
            topBorderCellStyle.Alignment = HorizontalAlignment.Left;
            topBorderCellStyle.BorderLeft = BorderStyle.None;
            topBorderCellStyle.BorderTop = BorderStyle.Thin;
            topBorderCellStyle.BorderRight = BorderStyle.None;
            topBorderCellStyle.BorderBottom = BorderStyle.None;
            topBorderCellStyle.LeftBorderColor = 0;
            topBorderCellStyle.TopBorderColor = 0;
            topBorderCellStyle.RightBorderColor = 0;
            topBorderCellStyle.BottomBorderColor = 0;
            return topBorderCellStyle;
        }
        private ICellStyle GetPercentageStyle()
        {
            // Veri hücreleri için stil oluştur
            ICellStyle dataCellStyle = Workbook.CreateCellStyle();
            IFont dataCellFont = Workbook.CreateFont();
            XSSFDataFormat dataFormat = (XSSFDataFormat)Workbook.CreateDataFormat();
            dataCellFont.IsBold = false;
            dataCellFont.FontHeightInPoints = 11;
            dataCellStyle.SetFont(dataCellFont);
            dataCellStyle.Alignment = HorizontalAlignment.Left;
            dataCellStyle.BorderLeft = BorderStyle.Thin;
            dataCellStyle.BorderTop = BorderStyle.Thin;
            dataCellStyle.BorderRight = BorderStyle.Thin;
            dataCellStyle.BorderBottom = BorderStyle.Thin;
            dataCellStyle.LeftBorderColor = 0;
            dataCellStyle.TopBorderColor = 0;
            dataCellStyle.RightBorderColor = 0;
            dataCellStyle.BottomBorderColor = 0;
            dataCellStyle.DataFormat = dataFormat.GetFormat("0.00\\%");
            return dataCellStyle;
        }
        private IFont SetNumberFont()
        {
            IFont redFont = Workbook.CreateFont();
            redFont.Color = HSSFColor.Red.Index;
            redFont.IsBold = true;
            return redFont;
        }
        private ICellStyle GetItalicStyle()
        {
            // Veri hücreleri için stil oluştur
            ICellStyle dataCellStyle = Workbook.CreateCellStyle();
            IFont dataCellFont = Workbook.CreateFont();
            dataCellFont.IsBold = false;
            dataCellFont.IsItalic = true;
            dataCellFont.FontHeightInPoints = 11;
            dataCellStyle.SetFont(dataCellFont);
            dataCellStyle.Alignment = HorizontalAlignment.Left;
            dataCellStyle.BorderLeft = BorderStyle.None;
            dataCellStyle.BorderTop = BorderStyle.None;
            dataCellStyle.BorderRight = BorderStyle.None;
            dataCellStyle.BorderBottom = BorderStyle.None;
            dataCellStyle.LeftBorderColor = 0;
            dataCellStyle.TopBorderColor = 0;
            dataCellStyle.RightBorderColor = 0;
            dataCellStyle.BottomBorderColor = 0;
            return dataCellStyle;
        }
        static IRichTextString Renklendir(string value, IFont font)
        {
            var richString = new XSSFRichTextString(value);
            for (int i = 0; i < value.Length; i++)
            {
                if (Char.IsDigit(value[i]))
                {
                    var j = i + 1;
                    while (j < value.Length && Char.IsDigit(value[j])) j++;
                    richString.ApplyFont(i, j, font);
                    i = j;
                }
            }
            return richString;
        }
        private ICellStyle GetCurrencyCellStyle(bool red = false, bool summary = false)
        {
            // Veri hücreleri için stil oluştur
            ICellStyle currencyCellStyle = Workbook.CreateCellStyle();
            IFont currencyCellFont = Workbook.CreateFont();
            currencyCellFont.Color = red? HSSFColor.Red.Index : HSSFColor.Black.Index; 
            currencyCellFont.IsBold = false;
            currencyCellFont.IsItalic = summary ? true : false;
            currencyCellFont.IsBold = summary ? true : false;
            currencyCellFont.FontHeightInPoints = 11;
            currencyCellStyle.SetFont(currencyCellFont);
            currencyCellStyle.DataFormat = 8;
            currencyCellStyle.Alignment = HorizontalAlignment.Right;
            currencyCellStyle.BorderLeft = BorderStyle.Thin;
            currencyCellStyle.BorderTop = BorderStyle.Thin;
            currencyCellStyle.BorderRight = BorderStyle.Thin;
            currencyCellStyle.BorderBottom = BorderStyle.Thin;
            currencyCellStyle.LeftBorderColor = 0;
            currencyCellStyle.TopBorderColor = 0;
            currencyCellStyle.RightBorderColor = 0;
            currencyCellStyle.BottomBorderColor = 0;
            return currencyCellStyle;
        }
        private ICellStyle GetHeaderCellStyle(int fs, short ic, bool summary = false)
        {
            IFont headerCellFont = Workbook.CreateFont();
            headerCellFont.IsBold = true;
            headerCellFont.FontHeightInPoints = fs;
            headerCellFont.IsItalic = summary ? true : false;
            ICellStyle headerCellStyle = Workbook.CreateCellStyle();
            headerCellStyle.SetFont(headerCellFont);
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
            headerCellStyle.BorderLeft = BorderStyle.Thin;
            headerCellStyle.BorderTop = BorderStyle.Thin;
            headerCellStyle.BorderRight = BorderStyle.Thin;
            headerCellStyle.BorderBottom = BorderStyle.Thin;
            headerCellStyle.LeftBorderColor = 0;
            headerCellStyle.TopBorderColor = 0;
            headerCellStyle.RightBorderColor = 0;
            headerCellStyle.BottomBorderColor = 0;
            headerCellStyle.WrapText = true;
            return headerCellStyle;
        }
        private ICellStyle GetDataCellStyle(bool red = false)
        {
            ICellStyle dataCellStyle = Workbook.CreateCellStyle();
            IFont dataCellFont = Workbook.CreateFont();
            dataCellFont.Color = red? HSSFColor.Red.Index : HSSFColor.Black.Index; 
            dataCellFont.IsBold = false;
            dataCellFont.FontHeightInPoints = 11;
            dataCellStyle.SetFont(dataCellFont);
            dataCellStyle.Alignment = HorizontalAlignment.Left;
            dataCellStyle.BorderLeft = BorderStyle.Thin;
            dataCellStyle.BorderTop = BorderStyle.Thin;
            dataCellStyle.BorderRight = BorderStyle.Thin;
            dataCellStyle.BorderBottom = BorderStyle.Thin;
            dataCellStyle.LeftBorderColor = 0;
            dataCellStyle.TopBorderColor = 0;
            dataCellStyle.RightBorderColor = 0;
            dataCellStyle.BottomBorderColor = 0;
            return dataCellStyle;
        }
        private ICellStyle GetDateCellStyle(bool red = false)
        {
            ICellStyle dateCellStyle = Workbook.CreateCellStyle();
            IFont dateCellFont = Workbook.CreateFont();
            dateCellFont.Color = red? HSSFColor.Red.Index : HSSFColor.Black.Index; 
            IDataFormat dateFormat = Workbook.CreateDataFormat();
            dateCellStyle.DataFormat = dateFormat.GetFormat("dd.MM.yyyy");
            dateCellFont.IsBold = false;
            dateCellFont.FontHeightInPoints = 11;
            dateCellStyle.SetFont(dateCellFont);
            dateCellStyle.Alignment = HorizontalAlignment.Center;
            dateCellStyle.BorderLeft = BorderStyle.Thin;
            dateCellStyle.BorderTop = BorderStyle.Thin;
            dateCellStyle.BorderRight = BorderStyle.Thin;
            dateCellStyle.BorderBottom = BorderStyle.Thin;
            dateCellStyle.LeftBorderColor = 0;
            dateCellStyle.TopBorderColor = 0;
            dateCellStyle.RightBorderColor = 0;
            dateCellStyle.BottomBorderColor = 0;
            return dateCellStyle;
        }
        private XSSFCellStyle GetPdCellStyle()
        {
            var color = new XSSFColor(new byte[] { 219, 112, 147 });
            XSSFCellStyle pdStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            pdStyle.SetFillForegroundColor(color);
            pdStyle.FillPattern = FillPattern.SolidForeground;
            IFont pdFont = Workbook.CreateFont();
            pdFont.IsBold = false;
            pdFont.FontHeightInPoints = 11;
            pdStyle.SetFont(pdFont);
            pdStyle.Alignment = HorizontalAlignment.Left;
            pdStyle.BorderLeft = BorderStyle.Thin;
            pdStyle.BorderTop = BorderStyle.Thin;
            pdStyle.BorderRight = BorderStyle.Thin;
            pdStyle.BorderBottom = BorderStyle.Thin;
            pdStyle.LeftBorderColor = 0;
            pdStyle.TopBorderColor = 0;
            pdStyle.RightBorderColor = 0;
            pdStyle.BottomBorderColor = 0;
            return pdStyle;
        }
        private XSSFCellStyle GetCmCellStyle()
        {
            var colorCs = new XSSFColor(new byte[] { 175, 238, 238 });
            var clmmStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            clmmStyle.SetFillForegroundColor(colorCs);
            clmmStyle.FillPattern = FillPattern.SolidForeground;
            IFont cFont = Workbook.CreateFont();
            cFont.IsBold = false;
            cFont.FontHeightInPoints = 11;
            clmmStyle.SetFont(cFont);
            clmmStyle.Alignment = HorizontalAlignment.Left;
            clmmStyle.BorderLeft = BorderStyle.Thin;
            clmmStyle.BorderTop = BorderStyle.Thin;
            clmmStyle.BorderRight = BorderStyle.Thin;
            clmmStyle.BorderBottom = BorderStyle.Thin;
            clmmStyle.LeftBorderColor = 0;
            clmmStyle.TopBorderColor = 0;
            clmmStyle.RightBorderColor = 0;
            clmmStyle.BottomBorderColor = 0;
            return clmmStyle;
        }
        private XSSFCellStyle GetCmmCellStyle()
        {
            var colorCms = new XSSFColor(new byte[] { 152, 251, 152 });
            XSSFCellStyle clmstrStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            clmstrStyle.SetFillForegroundColor(colorCms);
            clmstrStyle.FillPattern = FillPattern.SolidForeground;
            IFont cmFont = Workbook.CreateFont();
            cmFont.IsBold = false;
            cmFont.FontHeightInPoints = 11;
            clmstrStyle.SetFont(cmFont);
            clmstrStyle.Alignment = HorizontalAlignment.Left;
            clmstrStyle.BorderLeft = BorderStyle.Thin;
            clmstrStyle.BorderTop = BorderStyle.Thin;
            clmstrStyle.BorderRight = BorderStyle.Thin;
            clmstrStyle.BorderBottom = BorderStyle.Thin;
            clmstrStyle.LeftBorderColor = 0;
            clmstrStyle.TopBorderColor = 0;
            clmstrStyle.RightBorderColor = 0;
            clmstrStyle.BottomBorderColor = 0;
            return clmstrStyle;
        }
        private XSSFCellStyle GetCdDateStyle(XSSFColor xSsfColor)
        {
            var colorPdDate = xSsfColor; // new XSSFColor(new byte[] { 219, 112, 147 });
            XSSFCellStyle cdStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            IDataFormat pdDateFormat = Workbook.CreateDataFormat();
            cdStyle.DataFormat = pdDateFormat.GetFormat("dd.MM.yyyy");
            cdStyle.SetFillForegroundColor(colorPdDate);
            cdStyle.FillPattern = FillPattern.SolidForeground;
            IFont pdFontDate = Workbook.CreateFont();
            pdFontDate.IsBold = false;
            pdFontDate.FontHeightInPoints = 11;
            cdStyle.SetFont(pdFontDate);
            cdStyle.Alignment = HorizontalAlignment.Left;
            cdStyle.BorderLeft = BorderStyle.Thin;
            cdStyle.BorderTop = BorderStyle.Thin;
            cdStyle.BorderRight = BorderStyle.Thin;
            cdStyle.BorderBottom = BorderStyle.Thin;
            cdStyle.LeftBorderColor = 0;
            cdStyle.TopBorderColor = 0;
            cdStyle.RightBorderColor = 0;
            cdStyle.BottomBorderColor = 0;
            return cdStyle;
        }
        private XSSFCellStyle GetColoredHeaderCellStyle(byte[] rgbs)
        {
            var color = new XSSFColor(rgbs);
            XSSFCellStyle cHeadertyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            cHeadertyle.SetFillForegroundColor(color);
            cHeadertyle.FillPattern = FillPattern.SolidForeground;
            IFont pdFont = Workbook.CreateFont();
            pdFont.IsBold = true;
            pdFont.FontHeightInPoints = 11;
            cHeadertyle.SetFont(pdFont);
            cHeadertyle.Alignment = HorizontalAlignment.Center;
            cHeadertyle.BorderLeft = BorderStyle.Thin;
            cHeadertyle.BorderTop = BorderStyle.Thin;
            cHeadertyle.BorderRight = BorderStyle.Thin;
            cHeadertyle.BorderBottom = BorderStyle.Thin;
            cHeadertyle.LeftBorderColor = 0;
            cHeadertyle.TopBorderColor = 0;
            cHeadertyle.RightBorderColor = 0;
            cHeadertyle.BottomBorderColor = 0;
            return cHeadertyle;
        }
        private XSSFCellStyle GetColoredDateCellStyle(byte[] rgbs)
        {
            var color = new XSSFColor(rgbs);
            XSSFCellStyle cDateStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            cDateStyle.SetFillForegroundColor(color);
            cDateStyle.FillPattern = FillPattern.SolidForeground;
            IFont pdFont = Workbook.CreateFont();
            pdFont.IsBold = false;
            pdFont.FontHeightInPoints = 11;
            IDataFormat dateFormat = Workbook.CreateDataFormat();
            cDateStyle.DataFormat = dateFormat.GetFormat("dd.MM.yyyy");
            cDateStyle.SetFont(pdFont);
            cDateStyle.Alignment = HorizontalAlignment.Center;
            cDateStyle.BorderLeft = BorderStyle.Thin;
            cDateStyle.BorderTop = BorderStyle.Thin;
            cDateStyle.BorderRight = BorderStyle.Thin;
            cDateStyle.BorderBottom = BorderStyle.Thin;
            cDateStyle.LeftBorderColor = 0;
            cDateStyle.TopBorderColor = 0;
            cDateStyle.RightBorderColor = 0;
            cDateStyle.BottomBorderColor = 0;
            return cDateStyle;
        }
        private XSSFCellStyle GetColoredDataCellStyle(byte[] rgbs)
        {
            var color = new XSSFColor(rgbs);
            XSSFCellStyle cDataStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            cDataStyle.SetFillForegroundColor(color);
            cDataStyle.FillPattern = FillPattern.SolidForeground;
            IFont pdFont = Workbook.CreateFont();
            pdFont.IsBold = true;
            pdFont.FontHeightInPoints = 11;
            cDataStyle.SetFont(pdFont);
            cDataStyle.Alignment = HorizontalAlignment.Left;
            cDataStyle.BorderLeft = BorderStyle.Thin;
            cDataStyle.BorderTop = BorderStyle.Thin;
            cDataStyle.BorderRight = BorderStyle.Thin;
            cDataStyle.BorderBottom = BorderStyle.Thin;
            cDataStyle.LeftBorderColor = 0;
            cDataStyle.TopBorderColor = 0;
            cDataStyle.RightBorderColor = 0;
            cDataStyle.BottomBorderColor = 0;
            return cDataStyle;
        }
        private XSSFCellStyle GetColoredNumberCellStyle(byte[] rgbs)
        {
            var color = new XSSFColor(rgbs);
            XSSFCellStyle cnStyle = (XSSFCellStyle)Workbook.CreateCellStyle();
            cnStyle.SetFillForegroundColor(color);
            cnStyle.FillPattern = FillPattern.SolidForeground;
            IFont pdFont = Workbook.CreateFont();
            pdFont.IsBold = true;
            pdFont.FontHeightInPoints = 11;
            cnStyle.SetFont(pdFont);
            cnStyle.Alignment = HorizontalAlignment.Right;
            cnStyle.BorderLeft = BorderStyle.Thin;
            cnStyle.BorderTop = BorderStyle.Thin;
            cnStyle.BorderRight = BorderStyle.Thin;
            cnStyle.BorderBottom = BorderStyle.Thin;
            cnStyle.LeftBorderColor = 0;
            cnStyle.TopBorderColor = 0;
            cnStyle.RightBorderColor = 0;
            cnStyle.BottomBorderColor = 0;
            return cnStyle;
        }
        public bool CreateFileForReportDetails(string title, List<SourceIgb> sourceIgb, List<SourceSpvudk> sourceSpvudk, List<SourceSraod> sourceSraod , out string msg)
        {
            msg = "";
            try
            {
                sheet = Workbook.CreateSheet(title);
                IRow row; ICell cell;
                // Sayfa başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);

                ICellStyle hcs = GetHeaderCellStyle(14, IndexedColors.DarkBlue.Index);
                ICellStyle vhs = GetDataCellStyle();

                if (sourceIgb != null)
                {
                    merge = new CellRangeAddress(0, 0, 0, 3);
                    sheet.AddMergedRegion(merge);

                    // tablo başlığı
                    
                    row = sheet.CreateRow(1);
                    cell = row.CreateCell(0); cell.SetCellValue("İŞ GÖREMEZLİK BELGESİ"); 
                    merge = new CellRangeAddress(1, 1, 0, 3);
                    cell.CellStyle = hcs;
                    sheet.AddMergedRegion(merge);
                    int ix = 2;
                    foreach (var igbRow in sourceIgb)
                    {
                        row = sheet.CreateRow(ix);
                        cell = row.CreateCell(0); cell.SetCellValue(igbRow.Col1); cell.CellStyle = vhs;
                        cell = row.CreateCell(1); cell.SetCellValue(igbRow.Col2); cell.CellStyle = vhs;
                        cell = row.CreateCell(2); cell.SetCellValue(igbRow.Col3); cell.CellStyle = vhs;
                        cell = row.CreateCell(3); cell.SetCellValue(igbRow.Col4); cell.CellStyle = vhs;
                        ix++;
                    }
                    if (sourceSpvudk != null)
                    {
                        row = sheet.CreateRow(ix++);
                        cell = row.CreateCell(0); cell.SetCellValue("");
                        merge = new CellRangeAddress(ix, ix, 0, 3);
                        sheet.AddMergedRegion(merge);

                        row = sheet.CreateRow(ix++);
                        cell = row.CreateCell(0); cell.SetCellValue("SİGORTALININ PRİM VE ÜCRET DIŞINDAKİ KAZANÇLARI"); 
                        merge = new CellRangeAddress(ix, ix, 0, 3);
                        cell.CellStyle = hcs;
                        sheet.AddMergedRegion(merge);
                        foreach (var spvudkRow in sourceSpvudk)
                        {
                            row = sheet.CreateRow(ix);
                            cell = row.CreateCell(0); cell.SetCellValue(spvudkRow.Col1); cell.CellStyle = vhs;
                            cell = row.CreateCell(1); cell.SetCellValue(spvudkRow.Col2); cell.CellStyle = vhs;
                            cell = row.CreateCell(2); cell.SetCellValue(spvudkRow.Col3); cell.CellStyle = vhs;
                            cell = row.CreateCell(3); cell.SetCellValue(spvudkRow.Col4); cell.CellStyle = vhs;
                            ix++;
                        }
                        
                    }
                    for (int cc = 0; cc <= 3; cc++) sheet.AutoSizeColumn(cc);
                }
                else
                {
                    merge = new CellRangeAddress(0, 0, 0, 4);
                    sheet.AddMergedRegion(merge);
                    row = sheet.CreateRow(1);
                    cell = row.CreateCell(0); cell.SetCellValue("SEÇTİĞİNİZ RAPORA AİT ONAY DETAYLARI"); 
                    merge = new CellRangeAddress(1, 1, 0, 4);
                    cell.CellStyle = hcs;
                    sheet.AddMergedRegion(merge);
                    int iy = 2;
                    foreach (var sraodRow in sourceSraod)
                    {
                        row = sheet.CreateRow(iy);
                        cell = row.CreateCell(0); cell.SetCellValue(sraodRow.Col1); cell.CellStyle = vhs;
                        cell = row.CreateCell(1); cell.SetCellValue(sraodRow.Col2); cell.CellStyle = vhs;
                        cell = row.CreateCell(2); cell.SetCellValue(sraodRow.Col3); cell.CellStyle = vhs;
                        cell = row.CreateCell(3); cell.SetCellValue(sraodRow.Col4); cell.CellStyle = vhs;
                        cell = row.CreateCell(4); cell.SetCellValue(sraodRow.Col5); cell.CellStyle = vhs;
                        iy++;
                    }
                    for (int cc = 0; cc <= 4; cc++) sheet.AutoSizeColumn(cc);
                }
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForVisits(string title, string listeTipi, List<VisitsToBeProcessed> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            try
            {
                sheet = Workbook.CreateSheet(DateTime.Now.ToString("dd.MM.yyyy"));
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0,0,0,0);
                switch (listeTipi)
                {
                    case "onaylanacak":
                        merge = new CellRangeAddress(0, 0, 0, 11);
                        break;
                    case "onayli":
                        merge = new CellRangeAddress(0, 0, 0, 8);
                        break;
                    case "arsiv":
                        merge = new CellRangeAddress(0, 0, 0, 6);
                        break;
                }
                sheet.AddMergedRegion(merge);
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                XSSFCellStyle pdStyle = GetPdCellStyle();
                XSSFCellStyle clmmStyle = GetCmCellStyle();
                XSSFCellStyle clmstrStyle = GetCmmCellStyle();
                XSSFCellStyle pdStyleDate = GetCdDateStyle(new XSSFColor(new byte[] { 219, 112, 147 })); 
                XSSFCellStyle clmmStyleDate = GetCdDateStyle(new XSSFColor(new byte[] { 175, 238, 238 }));
                XSSFCellStyle clmStyleDate = GetCdDateStyle(new XSSFColor(new byte[] { 175, 238, 238 }));
                int rowCount = -1;
                switch (listeTipi)
                {
                    case "onaylanacak":
                        columnHeaders = new string[] {"Sıra","Firma Adı", "Kimlik Numarası", "Adı Soyadı", "Vaka Türü", "Rapor Takip No", "Rapor Sıra No", "Rapor Başlama Tarihi", "Rapor Bitiş Tarihi", "İşbaşı/Kontrol Tarihi","Açıklama", "Ceza Durumu"};
                        int i = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(i);
                            cell.SetCellValue(r);
                            cell.CellStyle = hcs;
                            i++;
                        }
                        int j = 1; 
                        foreach (var l in lst)
                        {
                            row = sheet.CreateRow(j+1);
                            int h = 0;
                            cell = row.CreateCell(h++);   cell.SetCellValue(j);                     cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.Cnm);                 cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.Tcno);                cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.AdSoyad);             cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.Vaka);                cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.RaporTakipNo);        cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.RaporSiraNo);         cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.RaporBaslamaTarihi);  cell.CellStyle = (l.WorkingStatus == 2)? pdStyleDate:( (l.WorkingStatus == 0) ? clmmStyleDate : clmStyleDate);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.RaporBitisTarihi);    cell.CellStyle = (l.WorkingStatus == 2)? pdStyleDate:( (l.WorkingStatus == 0) ? clmmStyleDate : clmStyleDate);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.IsBasiKontrolTarihi); cell.CellStyle = (l.WorkingStatus == 2)? pdStyleDate:( (l.WorkingStatus == 0) ? clmmStyleDate : clmStyleDate);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.Aciklama);            cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(h++);   cell.SetCellValue(l.CezaDurumu);          cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            j++; 

                        }
                        columnCount = 12;
                        sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
                        for (int cc = 0; cc <= 11; cc++) sheet.AutoSizeColumn(cc);
                        rowCount = j + 1;
                    break;
                    case "onayli":
                        columnHeaders = new string[] { "Sıra", "Firma Adı", "Kimlik Numarası", "Adı Soyadı", "Vaka Türü", "Rapor Takip No", "Rapor Sıra No", "Poliklinik Tarihi", "İşbaşı Tarihi"};
                        int k = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(k);
                            cell.SetCellValue(r);
                            cell.CellStyle = hcs;
                            k++;
                        }
                        int m = 1;
                        foreach (var l in lst)
                        {
                            row = sheet.CreateRow(m+1);
                            int n = 0;
                            cell = row.CreateCell(n++); cell.SetCellValue(m);                       cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.Cnm);                   cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.Tcno);                  cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.AdSoyad);               cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.Vaka);                  cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.RaporTakipNo);          cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.RaporSiraNo);           cell.CellStyle = vhs;
                            cell = row.CreateCell(n++); cell.SetCellValue(l.PoliklinikTarihi);      cell.CellStyle = ths; 
                            cell = row.CreateCell(n++); cell.SetCellValue(l.IsBasiKontrolTarihi);   cell.CellStyle = ths;
                            m++;
                        }
                        columnCount = 9;
                        sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
                        for (int cc = 0; cc <= 9; cc++) sheet.AutoSizeColumn(cc);
                        break;
                    case "arsiv":
                        columnHeaders = new string[] { "Sıra", "Firma Adı", "Kimlik Numarası", "Adı Soyadı", "Rapor Başlama Tarihi", "İşbaşı Tarihi", "Açıklama" };
                        int p = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(p);
                            cell.SetCellValue(r);
                            cell.CellStyle = hcs;
                            p++;
                        }
                        int s = 1;
                        foreach (var l in lst)
                        {
                            row = sheet.CreateRow(s+1);
                            int n = 0;
                            cell = row.CreateCell(n++); cell.SetCellValue(s);                       cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(n++); cell.SetCellValue(l.Cnm);                   cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(n++); cell.SetCellValue(l.Tcno);                  cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(n++); cell.SetCellValue(l.AdSoyad);               cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            cell = row.CreateCell(n++); cell.SetCellValue(l.RaporBaslamaTarihi);    cell.CellStyle = (l.WorkingStatus == 2) ? pdStyleDate : ((l.WorkingStatus == 0) ? clmmStyleDate : clmStyleDate);
                            cell = row.CreateCell(n++); cell.SetCellValue(l.IsBasiKontrolTarihi);   cell.CellStyle = (l.WorkingStatus == 2) ? pdStyleDate : ((l.WorkingStatus == 0) ? clmmStyleDate : clmStyleDate);
                            cell = row.CreateCell(n++); cell.SetCellValue(l.Aciklama);              cell.CellStyle = (l.WorkingStatus == 2)? pdStyle:( (l.WorkingStatus == 0)? clmmStyle : clmstrStyle);
                            s++;
                        }
                        columnCount = 7;
                        sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
                        for (int cc = 0; cc <= 6; cc++) sheet.AutoSizeColumn(cc);
                        rowCount = s + 1;
                        break;
                }

                row = sheet.CreateRow(rowCount + 1);
                for (int ix = 0; ix <= 2; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = pdStyle;
                    if (ix == 0)
                    {
                        cell.SetCellValue("PERSONELİM DEĞİL");
                    }
                }

                merge = new CellRangeAddress(rowCount + 1, rowCount + 1, 0, 2); sheet.AddMergedRegion(merge);
                
                row = sheet.CreateRow(rowCount + 2);
                for (int ix = 0; ix <= 2; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = clmmStyle;
                    if (ix == 0)
                    {
                        cell.SetCellValue("ÇALIŞMAMIŞTIR");
                    }
                }
                merge = new CellRangeAddress(rowCount + 2, rowCount + 2, 0, 2); sheet.AddMergedRegion(merge);
                
                row = sheet.CreateRow(rowCount + 3);
                for (int ix = 0; ix <= 2; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = clmstrStyle;
                    if (ix == 0)
                    {
                        cell.SetCellValue("ÇALIŞMIŞTIR");
                    }
                }
                merge = new CellRangeAddress(rowCount + 3, rowCount + 3, 0, 2); sheet.AddMergedRegion(merge);

                
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForCompanies(string title, List<Company> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            try
            {
                sheet = Workbook.CreateSheet("Firma Listesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 20);
                sheet.AddMergedRegion(merge);

                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                columnHeaders = new string[] { "Sıra", "Firma Adı", "Sgk Kullanıcı Adı", "Sgk Kullanıcı Kodu", "Sistem Şifresi", "İşyeri Şifresi", "Firma Merkezi", "Gib Kullanıcı Adı", "Gib Parola", "Gib Şifre", "Sicil No", "Unvan", "Adres", "Bağlı Olduğu Sgm", "Kanun Kapsamına Alınış", "Kanun Kapsamından Çıkış", "Özel Kod 1", "Özel Kod 2", "Özel Kod 3", "Özel Kod 4", "Özel Kod 5" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Id); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.CompanyName); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.CompanyId); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.CompanyId2); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.SystemPassword); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.CompanyPassword); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Fm); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Gun); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Gp); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Gs); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sgsc); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Unvan); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Adres); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sgm); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Kka); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Kkc); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sc1); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sc2); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sc3); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sc4); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Sc5); cell.CellStyle = vhs;

                    j++;
                }
                for (int cc = 0; cc <= 20; cc++) sheet.AutoSizeColumn(cc);
                columnCount = 21;

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForPersonals(string title, List<Personal> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            string[] companies = (from x in GlobalVars.Companies select x.CompanyName).ToArray();
            lst = lst.FindAll(q => q.Cid == GlobalVars.IzinComp.Id);
            try
            {
                sheet = Workbook.CreateSheet("Personel Listesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 5);
                sheet.AddMergedRegion(merge);

                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                ICellStyle shs = GetNumberCellStyle();
                columnHeaders = new string[] {"TC Kimlik Numarası", "Adı Soyadı", "Doğum Tarihi", "İşe Giriş Tarihi", "İşten Çıkış Tarihi", "Önceki Dönemlerden \r\nDevreden İzin Gün Sayısı" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Tcno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Ads); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Dtr); cell.SetCellType(CellType.Numeric); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Igt); cell.SetCellType(CellType.Numeric); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); if (l.Active) { cell.SetCellValue(""); cell.CellStyle = vhs; }
                    else { cell.SetCellValue(l.Ict); cell.SetCellType(CellType.Numeric); cell.CellStyle = ths; }
                    cell = row.CreateCell(h++); cell.SetCellValue((double)l.Tih); cell.SetCellType(CellType.Numeric); cell.CellStyle = shs;
                    j++;
                }
                for (int cc = 0; cc <= 5; cc++) { sheet.AutoSizeColumn(cc); int cs = sheet.GetColumnWidth(cc); sheet.SetColumnWidth(cc, cs + 1200); }
                sheet.SetColumnWidth(1, 12000);
                columnCount = 6;

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateTemplateForPersonals(out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            try
            {
                sheet = Workbook.CreateSheet("Personel Listesi");
                
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("PERSONEL LİSTESİ");
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 4);
                sheet.AddMergedRegion(merge);
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                ICellStyle shs = GetNumberCellStyle();
                columnHeaders = new string[] { "TC Kimlik Numarası", "Adı Soyadı", "Doğum Tarihi", "İşe Giriş Tarihi", "Önceki Dönemlerden \r\nDevreden İzin Gün Sayısı" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                for (int k = 2; j <= 302; j++)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = shs;
                    k++;
                }
                for (int cc = 0; cc <= 4; cc++) {sheet.AutoSizeColumn(cc); int cs = sheet.GetColumnWidth(cc); sheet.SetColumnWidth(cc, cs + 1200); }
                sheet.SetColumnWidth(1, 12000);
                sheet.SetColumnWidth(4, 16000);
                columnCount = 5;

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));

                //compListSheet = _workbook.CreateSheet("Firma Listesi");
                //j = 0;
                //List<string> companies = (from x in GlobalVars.companies orderby x.CompanyName ascending select x.CompanyName).ToList();
                //foreach (string comp in companies)
                //{
                //    row = compListSheet.CreateRow(j++);
                //    cell = row.CreateCell(0); cell.SetCellValue(comp); cell.CellStyle = vhs;
                //}

                //CellRangeAddressList cellRangeAddressList = new CellRangeAddressList(2, 300, 4, 4);
                //IDataValidationHelper dataValidationHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);
                //IDataValidationConstraint dataValidationConstraint = dataValidationHelper.CreateFormulaListConstraint($"='Firma Listesi'!$A$1:$A${GlobalVars.companies.Count+3}");
                //IDataValidation dataValidation = dataValidationHelper.CreateValidation(dataValidationConstraint, cellRangeAddressList);
                //dataValidation.ShowErrorBox = true;
                //dataValidation.CreateErrorBox("HATA!", "Geçersiz firma adı, lütfen listeden bir firma seçin");
                //dataValidation.SuppressDropDownArrow = true;
                //sheet.AddValidationData(dataValidation);
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateTemplateForCompanies(out string msg)
        {
            msg = "";
            string[] columnHeaders;
            try
            {
                sheet = Workbook.CreateSheet("Firma Listesi");
                IRow row; ICell cell;

                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 20;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = VerticalAlignment.Center;
                row = sheet.CreateRow(0);
                row.Height = 600;

                cell = row.CreateCell(0);
                for (int ix = 0; ix <= 12; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = titleStyle;
                    if (ix == 0)
                    {
                        cell.SetCellValue("FİRMA LİSTESİ");
                    }
                }
                cell.CellStyle.WrapText = true;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 12);
                sheet.AddMergedRegion(merge);
                ICellStyle hgcs = GetHeaderCellStyle(12, IndexedColors.Red.Index); 

                row = sheet.CreateRow(1);
                row.Height = 720;
                cell = row.CreateCell(0);
                for (int ix = 0; ix <= 4; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = hgcs;
                    if (ix == 0)
                    {
                        cell.SetCellValue("SGK KULLANICI BİLGİLERİ\r\n(ZORUNLU ALANLAR)");
                    }
                }
                
                cell.CellStyle.WrapText = true;

                merge = new CellRangeAddress(1, 1, 0, 4);
                sheet.AddMergedRegion(merge);

                cell = row.CreateCell(5);
                for (int ix = 5; ix <= 7; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = cell.CellStyle = hgcs;
                    ;
                    if (ix == 5)
                    {
                        cell.SetCellValue("GİB KULLANICI BİLGİLERİ\r\n(İSTEĞE BAĞLI)");
                    }
                }
                cell.CellStyle.WrapText = true;
                merge = new CellRangeAddress(1, 1, 5, 7);
                sheet.AddMergedRegion(merge);

                cell = row.CreateCell(8);
                for (int ix = 8; ix <= 12; ++ix)
                {
                    cell = row.CreateCell(ix);
                    cell.CellStyle = cell.CellStyle = hgcs;
                    
                    if (ix == 8)
                    {
                        cell.SetCellValue("FİLTRELEME İÇİN ÖZEL KODLAR\r\n(İSTEĞE BAĞLI)");
                    }
                }
                
                cell.CellStyle.WrapText = true;
                merge = new CellRangeAddress(1, 1, 8, 12);
                sheet.AddMergedRegion(merge);

                ICellStyle headerCellStyle = GetHeaderCellStyle(11, IndexedColors.Black.Index);

                // Sütun başlıkları için satır oluştur
                row = sheet.CreateRow(2);
                columnHeaders = new string[] { "Firma Adı", "Sgk Kullanıcı Adı", "Sgk Kullanıcı Kodu", "Sistem Şifresi", "İşyeri Şifresi", "Gib Kullanıcı Adı", "Gib Parola", "Gib Şifre", "Özel Kod 1", "Özel Kod 2", "Özel Kod 3", "Özel Kod 4", "Özel Kod 5" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = headerCellStyle;
                    i++;
                }

                // Veri hücreleri için stil oluştur
                ICellStyle vhs = GetDataCellStyle();
                for (int j= 3; j <= 23; j++)
                {
                    row = sheet.CreateRow(j);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                }
                for (int cc = 0; cc <= 12; cc++) sheet.AutoSizeColumn(cc);
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        public bool CreateFileForConfirm(string title, List<ConfirmReport> lst,  out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            try
            {
                sheet = Workbook.CreateSheet("Rapor Onay Listesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                if (SearchReport.GetConfirmPdf) { merge = new CellRangeAddress(0, 0, 0, 9); }  else{ merge = new CellRangeAddress(0, 0, 0, 8); }
                sheet.AddMergedRegion(merge);
                // Sütun başlıkları için satır oluştur
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle(); 


                if (SearchReport.GetConfirmPdf)
                {
                    columnHeaders = new string[] { "Kimlik No", "Adı Soyadı", "Vaka Türü", "Başlama Tarihi", "Bitiş Tarihi", "Takip No", "Sıra No", "İşlem Sonucu", "İşlem Tarihi", "Pdf Dosyası" };
                }
                else
                {
                    columnHeaders = new string[] { "Kimlik No", "Adı Soyadı", "Vaka Türü", "Başlama Tarihi", "Bitiş Tarihi", "Takip No", "Sıra No", "İşlem Sonucu", "İşlem Tarihi" };
                }
                

                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Tcid); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Fullname); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Vaka); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rbat); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rbit); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rtno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rsno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rslt); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Onyt); cell.CellStyle = ths;
                    if (SearchReport.GetConfirmPdf)
                    {
                        cell = row.CreateCell(h++); cell.SetCellValue(l.Pdffile); cell.CellStyle = vhs;
                    }
                    j++;
                }
                if (SearchReport.GetConfirmPdf)
                {
                    columnCount = 10;
                }
                else
                {
                    columnCount = 9;
                }
                for (int cc = 0; cc <= columnCount - 1; cc++) sheet.AutoSizeColumn(cc);
                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForUnConfirm(string title, List<ProcessReport> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 9;
            try
            {
                sheet = Workbook.CreateSheet("Rapor Onay İptal Listesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                if (SearchReport.GetConfirmPdf) { merge = new CellRangeAddress(0, 0, 0, 9); } else { merge = new CellRangeAddress(0, 0, 0, 8); }
                sheet.AddMergedRegion(merge);
                // Sütun başlıkları için satır oluştur
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();

                columnHeaders = new string[] { "Kimlik No", "Adı Soyadı", "Vaka Türü", "Takip No", "Sıra No", "Poliklinik Tarihi", "İşbaşı Tarihi",  "İşlem Sonucu", "İşlem Tarihi" };

                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Tcid); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Fullname); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Vaka); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rtno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rsno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rbat); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rbit); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Rslt); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Onyt); cell.CellStyle = ths;
                    j++;
                }
                for (int cc = 0; cc <= columnCount - 1; cc++) sheet.AutoSizeColumn(cc);
                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForLastNames(string title, List<LastNames> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 3;
            try
            {
                sheet = Workbook.CreateSheet("Rapor Onay Lİstesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 2); 
                sheet.AddMergedRegion(merge);
                // Sütun başlıkları için satır oluştur
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();

                columnHeaders = new string[] { "Kimlik No", "Güncelleme Öncesi Ad Soyad", "Güncelleme Sonrası Ad Soyad" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Tcno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Asf); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Asl); cell.CellStyle = vhs;
                    j++;
                }
                for (int cc = 0; cc <= columnCount - 1; cc++) sheet.AutoSizeColumn(cc);
                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
                
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }return true;
        }
        public bool CreateFileForIncentives(string title, List<Incentive> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 7;
            try
            {
                sheet = Workbook.CreateSheet("POTANSİYEL TEŞVİK SORGULAMA");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, columnCount - 1);
                sheet.AddMergedRegion(merge);
                // Sütun başlıkları için satır oluştur
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle shs = GetNumberCellStyle();
                columnHeaders = new string[] { "Firma Adı","Kimlik Numarası", "Yararlanılabilcek Teşvik", "Başlangıç - Bitiş Dönemi", "Teşvik Süresi", "Teşvik Kazancınız", "İlave Olunacak Sayı" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Cn); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Tcno); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.No); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Ts); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Bbd); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Tk); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Ios); cell.CellStyle = shs;
                    j++;
                }
                for (int cc = 0; cc <= columnCount - 1; cc++) sheet.AutoSizeColumn(cc);
                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForPersonalsAndPeriods(string title, List<Personal> lstPersonals, List<LeavePeriod> lstPeriods , out string msg)
        {
            msg = "";
            string[] columnHeaders, subColumnHeaders;
            int columnCount = 0;
            string[] companies = (from x in GlobalVars.Companies select x.CompanyName).ToArray();

            try
            {
                sheet = Workbook.CreateSheet("Personel Listesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 5);
                sheet.AddMergedRegion(merge);

                
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                ICellStyle shs = GetNumberCellStyle();
                XSSFCellStyle chcs = GetColoredHeaderCellStyle(new byte[] { 255, 242, 204 });
                XSSFCellStyle chcs2 = GetColoredHeaderCellStyle(new byte[] { 172, 185, 202 });
                XSSFCellStyle cvhs = GetColoredDataCellStyle(new byte[] { 255, 242, 204 });
                XSSFCellStyle cths = GetColoredDateCellStyle(new byte[] { 255, 242, 204 });
                XSSFCellStyle cshs = GetColoredNumberCellStyle(new byte[] { 255, 242, 204 });
                columnHeaders = new string[] { "TC Kimlik Numarası", "Adı Soyadı", "Doğum Tarihi", "İşe Giriş Tarihi", "İşten Çıkış Tarihi", "Kullanılmayan İzinler" };
                subColumnHeaders = new string[] { "Dönem", "Başlangıç Tarihi", "Bitiş Tarihi", "Hak Edilen \r\nİzin Süresi", "Kullanılan \r\nİzin Süresi", "Sarkan \r\nİzin Süresi" };
                
                int j = 1;
                foreach (Personal personal in lstPersonals)
                {
                    row = sheet.CreateRow(j++);
                    int i = 0;
                    foreach (var r in columnHeaders)
                    {
                        cell = row.CreateCell(i);
                        cell.SetCellValue(r);
                        cell.CellStyle = chcs;
                        i++;
                    }
                    row = sheet.CreateRow(j++);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(personal.Tcno); cell.CellStyle = cvhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(personal.Ads); cell.CellStyle = cvhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(personal.Dtr); cell.SetCellType(CellType.Numeric); cell.CellStyle = cths;
                    cell = row.CreateCell(h++); cell.SetCellValue(personal.Igt); cell.SetCellType(CellType.Numeric); cell.CellStyle = cths;
                    cell = row.CreateCell(h++); if (personal.Active) { cell.SetCellValue(""); cell.CellStyle = cvhs; }
                    else { cell.SetCellValue(personal.Ict); cell.SetCellType(CellType.Numeric); cell.CellStyle = cths; }
                    cell = row.CreateCell(h++); cell.SetCellValue((double)personal.Tih); cell.SetCellType(CellType.Numeric); cell.CellStyle = cshs;
                    
                    row = sheet.CreateRow(j++);
                    int y = 0;
                    foreach (var r in subColumnHeaders)
                    {
                        cell = row.CreateCell(y);
                        cell.SetCellValue(r);
                        cell.CellStyle = chcs2;
                        y++;
                    }
                    List<LeavePeriod> lstPersonalPeriods = (from x in lstPeriods where x.Tcno == personal.Tcno select x).ToList();
                    foreach (LeavePeriod period in lstPersonalPeriods)
                    {
                        row = sheet.CreateRow(j++);
                        int z = 0;
                        cell = row.CreateCell(z++); cell.SetCellValue(period.Period); cell.CellStyle = vhs;
                        cell = row.CreateCell(z++); cell.SetCellValue(period.Startdate); cell.SetCellType(CellType.Numeric); cell.CellStyle = ths;
                        cell = row.CreateCell(z++); cell.SetCellValue(period.Enddate); cell.SetCellType(CellType.Numeric); cell.CellStyle = ths;
                        cell = row.CreateCell(z++); cell.SetCellValue((double)period.His); cell.SetCellType(CellType.Numeric); cell.CellStyle = shs;
                        cell = row.CreateCell(z++); cell.SetCellValue((double)period.Ki); cell.SetCellType(CellType.Numeric); cell.CellStyle = shs;
                        cell = row.CreateCell(z++); cell.SetCellValue((double)period.Srk); cell.SetCellType(CellType.Numeric); cell.CellStyle = shs;
                       
                    }
                    
                }
                for (int cc = 0; cc <= 5; cc++) { sheet.AutoSizeColumn(cc); int cs = sheet.GetColumnWidth(cc); sheet.SetColumnWidth(cc, cs + 1200); }
                sheet.SetColumnWidth(1, 12000);
                columnCount = 6;

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateFileForLeaves(string title, List<Leaves> lst, out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            try
            {
                sheet = Workbook.CreateSheet("İzin Listesi");
                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 6);
                sheet.AddMergedRegion(merge);

                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                ICellStyle shs = GetNumberCellStyle();
                ICellStyle lhs = GetLinkCellStyle();
                columnHeaders = new string[] { "Başlangıç Tarihi", "Bitiş Tarihi", "İzin Süresi", "Ücretli İzin", "Resmi Tatiller\r\nİş Günü Sayılsın", "Açıklamalar", "Dosya Eki" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }
                int j = 1;
                foreach (var l in lst)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Startdate); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(l.Enddate); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue((double)l.Timeval); cell.SetCellType(CellType.Numeric); cell.CellStyle = shs;
                    string paid = l.Paid ? "Evet" : "Hayır";
                    cell = row.CreateCell(h++); cell.SetCellValue(paid); cell.SetCellType(CellType.String); cell.CellStyle = vhs;
                    string ph = l.Ph ? "Evet" : "Hayır";
                    cell = row.CreateCell(h++); cell.SetCellValue(ph); cell.SetCellType(CellType.String); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); if (l.Notes == null || l.Notes == "") { cell.SetCellValue(""); cell.CellStyle = vhs; }
                    else { cell.SetCellValue(l.Notes); cell.SetCellType(CellType.String); cell.CellStyle = vhs; }
                    cell = row.CreateCell(h++); if (l.Docref == null || l.Docref == "") { cell.SetCellValue(""); cell.CellStyle = vhs; }
                    else {
                        int slash = l.Docref.LastIndexOf(@"\") + 1; 
                        string fileName = l.Docref.Substring(slash);
                        cell.SetCellValue(fileName);
                        XSSFHyperlink fileLink = new XSSFHyperlink(HyperlinkType.File)
                        {
                            Address = l.Docref
                        };
                        cell.Hyperlink = (fileLink);
                        cell.SetCellType(CellType.String); 
                        cell.CellStyle = lhs; 
                    }
                    j++;
                }
                sheet.SetColumnWidth(0, 7000);
                sheet.SetColumnWidth(1, 7000);
                sheet.SetColumnWidth(2, 7000);
                sheet.SetColumnWidth(3, 7000);
                sheet.SetColumnWidth(4, 7000);
                sheet.SetColumnWidth(5, 16000);
                sheet.SetColumnWidth(6, 16000);
                columnCount = 7;

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateTemplateForExcelLeaves(out string msg)
        {
            msg = "";
            string[] columnHeaders;
            int columnCount = 0;
            try
            {
                sheet = Workbook.CreateSheet("izin listesi");

                IRow row; ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue("İZİN LİSTESİ");
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, 6);
                sheet.AddMergedRegion(merge);
                row = sheet.CreateRow(1);
                ICellStyle hcs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                ICellStyle shs = GetNumberCellStyle();
                columnHeaders = new string[] { "TC Kimlik Numarası", "Adı Soyadı", "Başlangıç Tarihi", "Bitiş Tarihi", "Ücretli İzin", "Açıklamalar", "Resmi Tatiller\r\nİş Günü Sayılsın" };
                int i = 0;
                foreach (var r in columnHeaders)
                {
                    cell = row.CreateCell(i);
                    cell.SetCellValue(r);
                    cell.CellStyle = hcs;
                    i++;
                }

                int j = 1;
                for (int k = 2; j <= 302; j++)
                {
                    row = sheet.CreateRow(j + 1);
                    int h = 0;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = ths;
                    cell = row.CreateCell(h++); cell.SetCellValue("Evet"); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue(""); cell.CellStyle = vhs;
                    cell = row.CreateCell(h++); cell.SetCellValue("Hayır"); cell.CellStyle = vhs;

                    k++;
                }
                for (int cc = 0; cc <= 4; cc++) { sheet.AutoSizeColumn(cc); int cs = sheet.GetColumnWidth(cc); sheet.SetColumnWidth(cc, cs + 1200); }
                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 12000);
                sheet.SetColumnWidth(2, 5000);
                sheet.SetColumnWidth(3, 5000);
                sheet.SetColumnWidth(4, 5000);
                sheet.SetColumnWidth(5, 16000);
                sheet.SetColumnWidth(6, 5000);
                columnCount = 7;

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));

                CellRangeAddressList cellRangeAddressList = new CellRangeAddressList(2, 300, 4, 4);
                IDataValidationHelper dataValidationHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);
                IDataValidationConstraint dataValidationConstraint = dataValidationHelper.CreateExplicitListConstraint(new string[] { "Evet", "Hayır"});
                IDataValidation dataValidation = dataValidationHelper.CreateValidation(dataValidationConstraint, cellRangeAddressList);
                dataValidation.ShowErrorBox = true;
                dataValidation.CreateErrorBox("HATA!", "Lütfen listeden Evet ya da Hayır seçin");
                dataValidation.SuppressDropDownArrow = true;
                sheet.AddValidationData(dataValidation);

                CellRangeAddressList cellRangeAddressList1 = new CellRangeAddressList(2, 300, 6, 6);
                IDataValidationHelper dataValidationHelper1 = new XSSFDataValidationHelper((XSSFSheet)sheet);
                IDataValidationConstraint dataValidationConstraint1 = dataValidationHelper.CreateExplicitListConstraint(new string[] { "Hayır", "Evet" });
                IDataValidation dataValidation1 = dataValidationHelper.CreateValidation(dataValidationConstraint1, cellRangeAddressList1);
                dataValidation1.ShowErrorBox = true;
                dataValidation1.CreateErrorBox("HATA!", "Lütfen listeden Evet ya da Hayır seçin");
                dataValidation1.SuppressDropDownArrow = true;
                sheet.AddValidationData(dataValidation1);

            }
            catch (Exception ex)
            {
                msg = "Excel dosyası oluşturulamıyor." + ex.Message.ToString();
                return false;
            }
            return true;
        }
        public bool CreateXlsHesap(string title, int type, out string msg)
        {
            msg = "";
            string[] columnHeaders = null;
            int columnCount = 0;
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            List<SgkDb> lstDb; List<SgkEt> lstEt; List<SgkMe> lstMe; List<SgkCr> lstCr; List<Sgk6661> lst6661; List<SgkHlp> lstHlp; List<SgkIgl> lstIgl; List<SgkThkk> lstThkk;
            
            try
            {
                sheet = Workbook.CreateSheet(title);
                IRow row;
                ICell cell;
                // Tablo başlığı oluştur
                IFont titleFont = Workbook.CreateFont();
                titleFont.IsBold = true;
                titleFont.FontHeightInPoints = 16;
                ICellStyle titleStyle = Workbook.CreateCellStyle();
                titleStyle.SetFont(titleFont);
                titleStyle.Alignment = HorizontalAlignment.Center;
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle.WrapText = true;
                cell.CellStyle = titleStyle;


                // Sütun başlıkları için satır oluştur
                row = sheet.CreateRow(1);

                ICellStyle sbs = GetHeaderCellStyle(11, IndexedColors.Black.Index);
                ICellStyle vhs = GetDataCellStyle();
                ICellStyle ths = GetDateCellStyle();
                ICellStyle phs = GetCurrencyCellStyle();

                ICellStyle cvhs = GetDataCellStyle(true);
                ICellStyle cths = GetDateCellStyle(true);
                ICellStyle cphs = GetCurrencyCellStyle(true);
                ICellStyle sphs = GetCurrencyCellStyle(false, true);
                ICellStyle ssbs = GetHeaderCellStyle(11, IndexedColors.Black.Index, true);

                int xx = 1;
                switch (type)
                {
                    case 1:
                        columnCount = 21; lstDb = GlobalVars.LstDb;
                        columnHeaders = new string[] { "FİRMA ADI", "YIL", "AY", "DURUMU", "PRİM BORCU", "PRİM BORCU GECİKME ZAMMI", "İDARİ PARA CEZASI BORCU", "İDARİ PARA CEZASI BORCU GECİKME ZAMMI", "EĞİTİME KATKI PAYI BORCU", "EĞİTİME KATKI PAYI BORCU GECİKME ZAMMI", "ÖZEL İŞLEM VERGİSİ BORCU", "ÖZEL İŞLEM VERGİSİ BORCU GECİKME ZAMMI", "İŞSİZLİK BORCU", "İŞSİZLİK BORCU GECİKME ZAMMI", "DAMGA VERGİSİ BORCU", "DAMGA VERGİSİ BORCU GECİKME ZAMMI", "DONMUŞ PRİM GECİKME ZAMMI BORCU", "DONMUŞ PRİM GECİKME ZAMMI BORCU GECİKME ZAMMI", "DONMUŞ EKP GECİKME ZAMMI BORCU", "SADECE BORÇLAR TOPLAMI", "GECİKME ZAMLARI TOPLAMI" };
                        int i = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(i);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            i++;
                        }
                        int j = 1;
                        foreach (SgkDb db in lstDb)
                        {
                            row = sheet.CreateRow(j + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(db.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(db.Yil); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(db.Ay); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(db.Drm); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Pb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.PbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Ipcb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.IpcbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Ekpb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.EkpbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Oivb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.OivbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Ib); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.IbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Dvb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.DvbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Dpgzb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.DpgzbGz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Dekpgzb); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.Sbt); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)db.SbtGz); cell.CellStyle = phs;
                            j++;
                        }
                        break;
                    case 2:
                        columnCount = 6; lstEt = GlobalVars.LstEt;
                        columnHeaders = new string[] { "FİRMA ADI", "TAHSİLAT TARİHİ", "DÖNEM YIL", "DÖNEM AY", "BORÇ TÜRÜ", "TAHSİLAT TUTARI" };
                        int k = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(k);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            k++;
                        }
                        int m = 1;
                        foreach (SgkEt et in lstEt)
                        {
                            row = sheet.CreateRow(m + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(et.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(et.Tt); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue(et.Yil); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(et.Ay); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(et.Bt); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)et.Ttr); cell.CellStyle = phs;
                            m++;
                        }
                        break;
                    case 3:
                        columnCount = 4; lstMe = GlobalVars.LstMe;
                        columnHeaders = new string[] { "FİRMA ADI", "BANKAYA YATIRILMA TARİHİ", "EMANETTEKİ TAHSİLAT TUTARI", "TAHSİLAT TÜRÜ" };
                        int n = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(n);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            n++;
                        }
                        int p = 1;
                        foreach (SgkMe me in lstMe)
                        {
                            row = sheet.CreateRow(p + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(me.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(me.Byt); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)me.Etr); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue(me.Tur); cell.CellStyle = vhs;
                            p++;
                        }
                        break;
                    case 4:
                        columnCount = 9; lstCr = GlobalVars.LstCr;
                        columnHeaders = new string[] { "FİRMA ADI", "KART NO", "TAKİP YIL", "TAKİP NO", "BORÇ TÜRÜ", "BORÇ ASLI", "GECİKME ZAMMI", "TAKİP MASRAFI", "TOPLAM" };
                        int s = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(s);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            s++;
                        }
                        int t = 1;
                        foreach (SgkCr cr in lstCr)
                        {
                            row = sheet.CreateRow(t + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(cr.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(cr.Kn); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue(cr.Ty); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(cr.Tn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(cr.Bt); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)cr.Ba); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)cr.Gz); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)cr.Tm); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)cr.Tp); cell.CellStyle = phs;
                            t++;
                        }
                        break;
                    case 5:
                        columnCount = 6; lst6661 = GlobalVars.Lst6661;
                        columnHeaders = new string[] { "FİRMA ADI", "YIL", "AY", "FAYDALANILAN GÜN SAYISI", "DESTEK TUTARI", "TAHSİLAT TARİHİ" };
                        int u = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(u);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            u++;
                        }
                        int v = 1;
                        foreach (Sgk6661 aaab in lst6661)
                        {
                            row = sheet.CreateRow(v + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(aaab.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(aaab.Yil); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue(aaab.Ay); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(aaab.Fgs); cell.SetCellType(CellType.Numeric); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)aaab.Dt); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue(aaab.Tt); cell.CellStyle = ths;
                            v++;
                        }
                        break;
                    case 6:
                        columnCount = 16; lstHlp = GlobalVars.LstHlp;
                        columnHeaders = new string[] { "FİRMA ADI", "DÖNEM", "BELGE ÇEŞİDİ", "BELGE MAHİYETİ", "KANUN NO", "TC KİMLİK NO", "AD SOYAD", "ÜCRET", "İKRAMİYE", "GÜN" , "EKSİK GÜN", "GİRİŞ GÜNÜ", "ÇIKIŞ GÜNÜ", "İŞTEN ÇIKIŞ NEDENİ", "EKSİK GÜN NEDENİ", "MESLEK KODU" };
                        int y = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(y);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            y++;
                        }
                        int z = 1;
                        foreach (SgkHlp hlp in lstHlp)
                        {
                            row = sheet.CreateRow(z + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Ya); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Bt); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Bm); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Kk); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Tcno); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Ads); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)hlp.Utl); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)hlp.Itl); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Gun); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.EGun); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.GGun); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.CGun); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Icn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Egn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(hlp.Mk); cell.CellStyle = vhs;
                            z++;
                        }
                        break;
                    case 7:
                        columnCount = 10; lstIgl = GlobalVars.LstIgl;
                        columnHeaders = new string[] { "FİRMA ADI", "TC KİMLİK NO", "ADI SOYADI", "GİRİŞ/ÇIKIŞ", "TARİH", "İSTİSNA", "İDARİ PARA CEZASI", "İŞLEM", "İŞLEM TARİHİ", "İŞLEM SAATİ"};
                        int w = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(w);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            w++;
                        }
                        int x = 1;
                        foreach (SgkIgl igl in lstIgl)
                        {
                            row = sheet.CreateRow(x + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Cn); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Tc); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Ads); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Gc); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Tr); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)igl.Stn); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)igl.Ipc); cell.CellStyle = phs;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Isl); cell.CellStyle = vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Ist); cell.CellStyle = ths;
                            cell = row.CreateCell(h++); cell.SetCellValue(igl.Isa); cell.CellStyle = vhs;
                            x++;
                        }
                        break;
                    case 8:
                        columnCount = 8; lstThkk = GlobalVars.LstThkk;
                        List<string> ch = new List<string>(){ "FİRMA ADI", "TAHAKKUK\r\nYIL / AY", "BELGE\r\nMAHİYETİ", "SOSYAL GÜVENLİK\r\nMERKEZİ", "TOPLAM PRİM\r\nÖDEMESİ", "İŞSİZLİK PRİMİ\r\nÖDEMESİ"};
                       
                        /*if ((from q in lstThkk select q.Kn14857).Sum() > 0)*/ { columnCount++; ch.Add("14857 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn15921).Sum() > 0)*/ { columnCount++; ch.Add("15921 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn6645).Sum() > 0)*/ { columnCount++; ch.Add("6645 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn15510).Sum() > 0)*/ { columnCount++; ch.Add("15510 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn2828).Sum() > 0)*/ { columnCount++; ch.Add("2828 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn6111).Sum() > 0)*/ { columnCount++; ch.Add("6111 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn17103).Sum() > 0)*/ { columnCount++; ch.Add("17103 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn17103I).Sum() > 0)*/ { columnCount++; ch.Add("17103 SAYILI KANUN\r\nİŞSİZLİK İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn27103).Sum() > 0)*/ { columnCount++; ch.Add("27103 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn27103I).Sum() > 0)*/ { columnCount++; ch.Add("27103 SAYILI KANUN\r\nİŞSİZLİK İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn37103).Sum() > 0)*/ { columnCount++; ch.Add("37103 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn37103I).Sum() > 0)*/ { columnCount++; ch.Add("37103 SAYILI KANUN\r\nİŞSİZLİK İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn7252).Sum() > 0)*/ { columnCount++; ch.Add("7252 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn17256).Sum() > 0)*/ { columnCount++; ch.Add("17256 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn7316).Sum() > 0)*/ { columnCount++; ch.Add("7316 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn7319).Sum() > 0)*/ { columnCount++; ch.Add("7319 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn5510).Sum() > 0)*/ { columnCount++; ch.Add("5510 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn4857).Sum() > 0)*/ { columnCount++; ch.Add("4857 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn159210).Sum() > 0)*/ { columnCount++; ch.Add("159210 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        /*if ((from q in lstThkk select q.Kn3294).Sum() > 0)*/ { columnCount++; ch.Add("3294 SAYILI KANUN\r\nPRİM İNDİRİMİ"); }
                        ch.Add("ÖDENECEK\r\nNET TUTAR");
                        ch.Add("ONAYLI");
                        columnHeaders = ch.ToArray();
                        int ii = 0;
                        foreach (var r in columnHeaders)
                        {
                            cell = row.CreateCell(ii);
                            cell.SetCellValue(r);
                            cell.CellStyle = sbs;
                            ii++;
                        }
                        IDataFormat dateFormat = Workbook.CreateDataFormat();
                        
                        ths.DataFormat = dateFormat.GetFormat("yyyy / MM");
                        cths.DataFormat = dateFormat.GetFormat("yyyy / MM");

                        foreach (SgkThkk thkk in lstThkk)
                        {
                            row = sheet.CreateRow(xx + 1);
                            int h = 0;
                            cell = row.CreateCell(h++); cell.SetCellValue(thkk.Cn); cell.CellStyle = thkk.Bm == "İPTAL"? cvhs : vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(thkk.Tya);  cell.CellStyle = thkk.Bm == "İPTAL"? cths : ths;
                            cell = row.CreateCell(h++); cell.SetCellValue(thkk.Bm); cell.CellStyle = thkk.Bm == "İPTAL"? cvhs : vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue(thkk.Sgm); cell.CellStyle = thkk.Bm == "İPTAL"? cvhs : vhs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Tp); cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs;
                            cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Ip); cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs;
                            /*if ((from q in lstThkk select q.Kn14857).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn14857);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn15921).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn15921);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn6645).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn6645);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn15510).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn15510);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn2828).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn2828);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn6111).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn6111);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn17103).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn17103);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn17103I).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn17103I);    cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn27103).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn27103);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn27103I).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn27103I);    cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn37103).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn37103);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn37103I).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn37103I);    cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn7252).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn7252);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn17256).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn17256);      cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn7316).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn7316);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn7319).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn7319);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn5510).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn5510);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn4857).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn4857);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn159210).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn159210);    cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            /*if ((from q in lstThkk select q.Kn3294).Sum() > 0)*/ { cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Kn3294);        cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs; }
                            cell = row.CreateCell(h++); cell.SetCellValue((double)thkk.Odenecek); cell.CellStyle = thkk.Bm == "İPTAL"? cphs : phs;
                            cell = row.CreateCell(h++); if (thkk.Onayli) cell.SetCellValue("EVET"); else cell.SetCellValue("HAYIR"); cell.CellStyle = thkk.Bm == "İPTAL"? cvhs : vhs; 
                            xx++;
                        }
                        #region summary
                        row = sheet.CreateRow(xx + 1);
                        int rr = 0;
                        cell = row.CreateCell(rr++); cell.CellStyle = ssbs; cell.SetCellValue("TOPLAMLAR");
                        cell = row.CreateCell(rr++); cell.CellStyle = vhs; cell.SetCellValue("");
                        cell = row.CreateCell(rr++); cell.CellStyle = vhs; cell.SetCellValue("");
                        cell = row.CreateCell(rr++); cell.CellStyle = vhs; cell.SetCellValue("");
                        cell = row.CreateCell(rr++); cell.SetCellType(CellType.Formula); cell.CellStyle = sphs; cell.CellFormula = GetFormula(cell.ColumnIndex, xx);
                        cell = row.CreateCell(rr++); cell.SetCellType(CellType.Formula); cell.CellStyle = sphs; cell.CellFormula = GetFormula(cell.ColumnIndex, xx);
                        /*if ((from q in lstThkk select q.Kn14857).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn15921).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn6645).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn15510).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn2828).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn6111).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn17103).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn17103I).Sum() > 0)*/    { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn27103).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn27103I).Sum() > 0)*/    { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn37103).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn37103I).Sum() > 0)*/    { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn7252).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn17256).Sum() > 0)*/     { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn7316).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn7319).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn5510).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn4857).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn159210).Sum() > 0)*/    { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Kn3294).Sum() > 0)*/      { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        /*if ((from q in lstThkk select q.Odenecek).Sum() > 0)*/    { cell = row.CreateCell(rr++);  cell.SetCellType(CellType.Formula); cell.CellStyle = sphs;  cell.CellFormula = GetFormula(cell.ColumnIndex, xx); }
                        cell = row.CreateCell(rr++); cell.CellStyle = vhs; cell.SetCellValue("");
                        CellRangeAddress mergeThkk = new CellRangeAddress(xx + 1, xx + 1, 0, 3); 
                        sheet.AddMergedRegion(mergeThkk);
                        #endregion

                    break;
                }
                CellRangeAddress merge = new CellRangeAddress(0, 0, 0, 0);
                merge = new CellRangeAddress(0, 0, 0, columnCount - 1);
                sheet.AddMergedRegion(merge);

                

                sheet.SetAutoFilter(new CellRangeAddress(1, sheet.PhysicalNumberOfRows - 2, 0, columnCount - 1));
                for (int cc = 0; cc < columnCount; cc++) { sheet.AutoSizeColumn(cc); int cs = sheet.GetColumnWidth(cc); sheet.SetColumnWidth(cc, cs + 1200); }
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                return true;
            }
            catch (Exception ex)
            {
                msg = "Excel Sayfası oluşturulamadı!" + ex.Message.ToString(); GlobalVars.ErrorReport += $"<li>{msg}</li>"; return false;
            }
        }
        public string GetFormula(int colIndex, int rowIndex = 0)
        {
            string letter = "";
            if (colIndex == 0) letter = "A";
            else if (colIndex == 1) letter = "B";
            else if (colIndex == 2) letter = "C";
            else if (colIndex == 3) letter = "D";
            else if (colIndex == 4) letter = "E";
            else if (colIndex == 5) letter = "F";
            else if (colIndex == 6) letter = "G";
            else if (colIndex == 7) letter = "H";
            else if (colIndex == 8) letter = "I";
            else if (colIndex == 9) letter = "J";
            else if (colIndex == 10) letter = "K";
            else if (colIndex == 11) letter = "L";
            else if (colIndex == 12) letter = "M";
            else if (colIndex == 13) letter = "N";
            else if (colIndex == 14) letter = "O";
            else if (colIndex == 15) letter = "P";
            else if (colIndex == 16) letter = "Q";
            else if (colIndex == 17) letter = "R";
            else if (colIndex == 18) letter = "S";
            else if (colIndex == 19) letter = "T";
            else if (colIndex == 20) letter = "U";
            else if (colIndex == 21) letter = "V";
            else if (colIndex == 22) letter = "W";
            else if (colIndex == 23) letter = "X";
            else if (colIndex == 24) letter = "Y";
            else if (colIndex == 25) letter = "Z";
            else if (colIndex == 26) letter = "AA";
            else if (colIndex == 27) letter = "AB";
            else if (colIndex == 28) letter = "AC";
            else if (colIndex == 29) letter = "AD";
            else if (colIndex == 30) letter = "AE";
            else if (colIndex == 31) letter = "AF";
            else if (colIndex == 32) letter = "AG";
            else if (colIndex == 33) letter = "AH";
            else if (colIndex == 34) letter = "AI";
            else if (colIndex == 35) letter = "AJ";
            else if (colIndex == 36) letter = "AK";
            else if (colIndex == 37) letter = "AL";
            else if (colIndex == 38) letter = "AM";
            else if (colIndex == 39) letter = "AN";
            else if (colIndex == 40) letter = "AO";
            //"ETOPLA(C3:C7;"<>İPTAL";E3:E7)"
            //return $"SUM({letter}3:{letter}{rowIndex + 1})";
            string formul = string.Format( $"SUMIF(C3:C{rowIndex + 1},\"<>İPTAL\",{letter}3:{letter}{rowIndex + 1})");
            return formul;
        }
        public bool Save(string fn, out string msg)
        {
            msg = "";
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Title = "Listeyi excel'e aktar";
                saveFileDialog.Filter = "Excel Dosyası|*.xlsx";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.FileName = fn;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileName = saveFileDialog.FileName;
                }
                else
                {
                    msg = "Dosya oluşturma iptal edildi.";
                    return false;
                }
                using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    Workbook.Write(stream);
                }
                string path = FileName.Remove(FileName.LastIndexOf("\\") + 1);
                string name = FileName.Replace(path, "") ;
                msg = $"'{name}' dosyası '{path}' konumuna kaydedildi.";
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("başka bir işlem")) msg = "Dosya kullanımda, lütfen açık olan dosyayı kapatın";
                else msg = $"EXCEL Dosyası {this.FileName} kaydedilemiyor." + ex.Message.ToString();
                return false; 
            }
        }
    }

}