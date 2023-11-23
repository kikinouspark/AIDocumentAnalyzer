using DocumentRecognizerTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DocumentRecognizerTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/Error/{statusCode?}")]
        public IActionResult Error(int statusCode)
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            if (User?.Identity != null)
            {
                ViewData["DisplayName"] = "Visitor"; 
            }

            if (statusCode != 0)
            {
                return View(new ErrorViewModel
                {
                    ErrorCode = statusCode,
                    PreviousPage = exceptionHandlerPathFeature?.Path ?? Request.Path,
                    ErrorMessage = "Status code error : " + statusCode
                });
            }
            else
            {
                ErrorViewModel errorViewModel = new()
                {
                    PreviousPage = exceptionHandlerPathFeature.Path ?? Request.Path,
                    Exception = exceptionHandlerPathFeature?.Error
                };
                return View(errorViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FileUploaded(IList<IFormFile> file, string Batch)
        {
            try
            {
                //await FormRecognizerController.Uploaded(file, Batch);
                await NeuralRecognizerController.Uploaded(file, Batch);
                string[] filePaths = Directory.GetFiles(Environment.CurrentDirectory + @"\\wwwroot\\Downloads\\", "*.csv*", SearchOption.AllDirectories);
                string[] filePathsReports = Directory.GetFiles(Environment.CurrentDirectory + @"\\wwwroot\\Reports\\", "*.csv*", SearchOption.AllDirectories);
                List<FileListModel> files = new();
                foreach (string filePath in filePaths)
                {
                    string subdir = Path.GetDirectoryName(filePath);
                    string report = "";
                    if (Path.GetFileName(filePath).Contains("report"))
                        report = Path.GetFileName(filePath);

                    files.Add(new FileListModel
                    {
                        FileName = Path.GetFileName(filePath),
                        Resid_EIN_name = subdir.Split('\\').Last()
                    });
                }
                return View("Downloads", files);
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel
                {
                    ErrorMessage = ex.ToString()
                };
                Console.WriteLine("exception obtained  " + ex);

                return View("Error", error);
            }
        }

        [Route("/Downloads")]
        [Route("/Home/Downloads")]
        public IActionResult Downloads()
        {
            string[] filePaths = Directory.GetFiles(Environment.CurrentDirectory + @"\\wwwroot\\Downloads\\", "*.csv*", SearchOption.AllDirectories);
            List<FileListModel> files = new();
            foreach (string filePath in filePaths)
            {
                string subdir = Path.GetDirectoryName(filePath);
                files.Add(new FileListModel
                {
                    FileName = Path.GetFileName(filePath),
                    Resid_EIN_name = subdir.Split('\\').Last()
                });
            }
            return View(files);
        }

        public FileResult DownloadFile(string fileName)
        {
            string path = Environment.CurrentDirectory + @"\\wwwroot\\Downloads\\" + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/CSV", fileName);
        }

        public FileResult DownloadFileBK(string fileName)
        {
            string path = Environment.CurrentDirectory + @"\\wwwroot\\Backup\\" + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application /CSV", fileName);
        }

        public FileResult DownloadReport(string fileName)
        {
            string report_file_name = Path.GetFileNameWithoutExtension(fileName) + "_report.csv";
            string path = Environment.CurrentDirectory + @"\\wwwroot\\Reports\\" + report_file_name;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/CSV", report_file_name);
        }

        [Route("/Backup")]
        [Route("/Home/Backup")]
        public IActionResult Backup()
        {
            string[] filePaths = Directory.GetFiles(Environment.CurrentDirectory + @"\\wwwroot\\Backup\\", "*.csv*", SearchOption.AllDirectories);
            List<FileListModel> files = new List<FileListModel>();
            foreach (string filePath in filePaths)
            {
                string subdir = Path.GetDirectoryName(filePath);
                files.Add(new FileListModel
                {
                    FileName = Path.GetFileName(filePath),
                    Resid_EIN_name = subdir.Split('\\').Last()
                });
            }
            return View(files);
        }

        public ActionResult DeleteAll() //Moving the files now instead of Delete
        {
            string folderDW = Directory.GetCurrentDirectory() + "\\wwwroot\\Downloads";
            string folderBK = Directory.GetCurrentDirectory() + "\\wwwroot\\Backup";
            List<FileListModel> files = new();

            DirectoryInfo diCB = new(folderDW);

            foreach (FileInfo file in diCB.GetFiles("*.csv"))
                file.MoveTo(folderBK + "\\" + file.Name);

            return View("Downloads", files);
        }
    }
}