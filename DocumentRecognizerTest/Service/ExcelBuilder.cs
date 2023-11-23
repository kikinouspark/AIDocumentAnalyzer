using OfficeOpenXml;

namespace DocumentRecognizerTest.Service
{
    public class ExcelBuilder
    {

        public static ExcelPackage CreateExcel(ExcelPackage wb2)
        {
            ExcelWorksheet wsSheet1 = wb2.Workbook.Worksheets.Add("maindata");
            return wb2;
        }

        public static ExcelPackage AddToExcel(string? Header, string? value, string filename, ExcelPackage wb2, int count, int iteration)
        {
            var wsSheet1 = wb2.Workbook.Worksheets[0];
            if (count == 0)
                count = 1;

            wsSheet1.Cells[2, 1].Value = "File Name";
            wsSheet1.Cells[count + 1, 1].Value = Path.GetFileNameWithoutExtension(filename); ;

            wsSheet1.Cells[count, iteration + 1].Value = Header;
            wsSheet1.Cells[count + 1, iteration + 1].Value = value;

            return wb2;
        }

        public static void SaveExcel(string filename, ExcelPackage wb2)
        {
            using FileStream s = File.Create(Environment.CurrentDirectory + @"\\wwwroot\\Downloads\\" + filename + ".csv");
            wb2.SaveAs(s);
            s.Close();
        }
    }
}
