using Aspose.Cells;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.IO.enums;
using System.Text;

namespace DocumentRecognizerTest.Service
{
    public class PDFSplitter
    {

        public static void Splitpdf(string file)
        {
            // Get a fresh copy of the sample PDF file
            string filename = file;//"Portable Document Format.pdf";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Open the file
            PdfSharpCore.Pdf.PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import, PdfReadAccuracy.Moderate);

            string name = Path.GetFileNameWithoutExtension(filename);
            for (int idx = 0; idx < inputDocument.PageCount; idx++)
            {
                // Create new document
                PdfSharpCore.Pdf.PdfDocument outputDocument = new PdfSharpCore.Pdf.PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Title =
                  String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[idx]);
                outputDocument.Save(String.Format(Directory.GetCurrentDirectory() + "\\wwwroot\\UploadsSplit\\" + "{0} - Page {1}.pdf", name, idx + 1));
            }
        }

        public static int CountPdfPages(string filepath)
        {
            string filename = filepath;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfSharpCore.Pdf.PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import, PdfReadAccuracy.Moderate);
            int pagecount = inputDocument.PageCount;
            inputDocument.Close();
            return pagecount;
        }

        public static void ExcelToPDF(string path,string excelname)
        {
            try
            {
                var newpath = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\";

                // Instantiate the Workbook object with the Excel file
                Workbook workbook = new(path);

                int sheetCount = workbook.Worksheets.Count;

                if (sheetCount != 1)
                    workbook.Worksheets[sheetCount - 1].IsVisible = false;

                // Save the document in PDF format
                workbook.Save(newpath + Path.GetFileNameWithoutExtension(excelname) + ".pdf", SaveFormat.Pdf);

                workbook.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
