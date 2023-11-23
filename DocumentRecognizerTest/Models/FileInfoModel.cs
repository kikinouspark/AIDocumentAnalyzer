using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRecognizerTest.Models
{
    public class FileInfoModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string BankFolder { get; set; }
        public string Batch { get; set; }
        public IList<IFormFile> File { get; set; }

    }
}
