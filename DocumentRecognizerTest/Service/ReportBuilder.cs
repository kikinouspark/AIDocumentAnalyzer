using OfficeOpenXml;
using System.Drawing;

namespace DocumentRecognizerTest.Service
{
    public class ReportBuilder
    {

        public static ExcelPackage CreateReportExcel(ExcelPackage wb2)
        {
            ExcelWorksheet wsSheet1 = wb2.Workbook.Worksheets.Add("reportdata");
            return wb2;
        }

        public static ExcelPackage AddToExcel(string? Header, float? value, string filename, ExcelPackage wb2, int count, int iteration)
        {
                var wsSheet1 = wb2.Workbook.Worksheets[0];

                if (count == 0)
                    count = 1;

                wsSheet1.Cells[2, 1].Value = "File Name";
                wsSheet1.Cells[count + 1, 1].Value = Path.GetFileNameWithoutExtension(filename);

                wsSheet1.Cells[count, iteration + 1].Value = Header;
                wsSheet1.Cells[count + 1, iteration + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                wsSheet1.Cells[count + 1, iteration + 1].Value = value;

                if (value > .9)
                    wsSheet1.Cells[count + 1, iteration + 1].Style.Fill.BackgroundColor.SetColor(Color.Green);
                else if (value < .9 && value > .8)
                    wsSheet1.Cells[count + 1, iteration + 1].Style.Fill.PatternColor.SetColor(Color.Yellow);
                else if (value < .8 && value > 0.3)
                    wsSheet1.Cells[count + 1, iteration + 1].Style.Fill.PatternColor.SetColor(Color.Red);
                else
                    wsSheet1.Cells[count + 1, iteration + 1].Style.Fill.PatternColor.SetColor(Color.Green);

                return wb2;
            }

        public static void SaveExcel(string filename, ExcelPackage wb2)
            {
                wb2.SaveAs(new FileInfo(Environment.CurrentDirectory + @"\\wwwroot\\Reports\\" + filename + "_report.csv"));
            }
    }
}
