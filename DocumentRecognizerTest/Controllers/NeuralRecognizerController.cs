using Microsoft.AspNetCore.Mvc;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using OfficeOpenXml;
using DocumentRecognizerTest.Service;
using DocumentRecognizerTest.Models;

namespace DocumentRecognizerTest.Controllers
{
    public class NeuralRecognizerController : Controller
    {
        public static string _endpoint = "https://docinttestld.cognitiveservices.azure.com/";
        public static string _apiKey = "e54a774708664f88a8b5b6db7d295976";

        [HttpPost]
        public static async Task<IActionResult> Uploaded(IList<IFormFile> files, string Batch)
        {
            string folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Uploads\\";
            string folderPathsplit = Directory.GetCurrentDirectory() + "\\wwwroot\\UploadsSplit\\";
            string folderExcel = Directory.GetCurrentDirectory() + "\\wwwroot\\Excels\\";
            string folderReport = Directory.GetCurrentDirectory() + "\\wwwroot\\Reports\\";
            AzureKeyCredential credential = new(_apiKey);
            DocumentAnalysisClient client = new(new Uri(_endpoint), credential);
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackages excelPackages1 = new();
            ExcelPackage wb2 = new();
            ExcelPackage rwb2 = new();
            ExcelBuilder.CreateExcel(wb2);
            ReportBuilder.CreateReportExcel(rwb2);
            excelPackages1.wb2 = wb2;
            excelPackages1.rwb2 = rwb2;

            ClearFolders(folderPath, folderPathsplit, folderExcel);

            foreach (IFormFile source in files)
            {
                string path;
                if (source.ContentType == "application/pdf")
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", source.FileName);
                    await SaveFileAsync(path, source);
                }
                else if (source.ContentType == "application/vnd.ms-excel" || source.ContentType == "application/excel" || source.ContentType == "application/x-msexcel" || source.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Excels", source.FileName);
                    await SaveFileAsync(path, source);
                    PDFSplitter.ExcelToPDF(path, source.FileName);
                }
            }


            try
            {
                string test = await DocRecogTestAsync_Document(wb2,rwb2,client,folderPath);
                return null;
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    ErrorMessage = ex.ToString()
                };
                Console.WriteLine("exception obtained  " + ex);

                throw;
            }
        }

        private static async Task SaveFileAsync(string path, IFormFile source)
        {
            if (System.IO.File.Exists(path))
            {
                FileInfo f2 = new(path);
                f2.Delete();
            }
            using var stream = new FileStream(path, FileMode.Create);
            await source.CopyToAsync(stream);
        }

        public static async Task<string> DocRecogTestAsync_Document(ExcelPackage wkb, ExcelPackage rwkb, DocumentAnalysisClient client, string folderPath)
        {
            int count = 2;
            int iteration = 1;
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.pdf"))
            {
                using (FileStream stream = new(file, FileMode.Open))
                {
                    AnalyzeDocumentOperation forms = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", stream);
                    AnalyzeResult result = forms.Value;

                    foreach (AnalyzedDocument form in result.Documents)
                    {
                        foreach (KeyValuePair<string, DocumentField> fieldKvp in form.Fields)
                        {
                            foreach (DocumentPage page in result.Pages)
                            {
                                DocumentField field = fieldKvp.Value;

                                string data = fieldKvp.Value?.Content ?? "please check original Document Value";
                                string fieldName = fieldKvp.Key ?? "Check Document Value";
                                float confidence = field.Confidence.HasValue ? (float)field.Confidence : 0;

                                ExcelBuilder.AddToExcel(fieldName, data, stream.Name, wkb, count, iteration);
                                ReportBuilder.AddToExcel(fieldName, confidence, stream.Name, rwkb, count, iteration);
                                iteration++;
                            }
                        }
                    }
                    ExcelBuilder.SaveExcel(Path.GetFileNameWithoutExtension(stream.Name), wkb);
                    ReportBuilder.SaveExcel(Path.GetFileNameWithoutExtension(stream.Name), rwkb);
                }
                //count += 2;
            }
            return "Ok";
        }

        public static void ClearFolders(string folderPath, string folderPathsplit, string folderExcel)
        {
            DirectoryInfo di = new(folderPath);
            foreach (FileInfo file in di.GetFiles("*.pdf")) { file.Delete(); }

            DirectoryInfo displit = new(folderPathsplit);
            foreach (FileInfo file in displit.GetFiles("*.pdf")) { file.Delete(); }

            DirectoryInfo diExcel = new(folderExcel);
            foreach (FileInfo file in diExcel.GetFiles("*.xls")) { file.Delete(); }
        }
    }
}
