using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocumentRecognizerTest.Controllers
{
    public class ReadFileController : Controller
    {
        private IWebHostEnvironment _env;

        public ReadFileController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload(IList<IFormFile> files)
        {
            foreach (IFormFile source in files)
            {

                string filename = source.FileName;
                filename = this.EnsureCorrectFilename(filename);
                byte[] buffer = new byte[16 * 1024];
                string namafileExcell = "";
                string namafilenewExcell = "";
                string extension = "";

                namafileExcell = source.FileName;
                extension = namafileExcell.Substring(namafileExcell.LastIndexOf('.') + 1);
                namafileExcell = namafileExcell.Replace(namafileExcell.Substring(namafileExcell.LastIndexOf('.') + 1), "");
                namafilenewExcell = namafileExcell.Replace(".", "") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot\\Uploads",
                namafilenewExcell);
                if (System.IO.File.Exists(path))
                {
                    FileInfo f2 = new FileInfo(path);
                    f2.Delete();
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await source.CopyToAsync(stream);
                }

                try
                {
                    int readBytes;
                    using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(namafilenewExcell)))
                    {
                        using (Stream input = source.OpenReadStream())
                        {
                            while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                await output.WriteAsync(buffer, 0, readBytes);
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    string msgError = err.ToString();
                    return Json(new { message = msgError });
                }
            }
            return Json(new { message = "Failed!" });
        }

        private string GetPathAndFilename(string filename)
        {
            string path = _env.WebRootPath + "/Uploads/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            return filename;
        }
    }
}
