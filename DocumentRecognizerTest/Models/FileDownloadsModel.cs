using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocumentRecognizerTest.Models
{
    public class FileDownloadsModel
    {
        public List<FileInfoModel> GetFile()
        {
            List<FileInfoModel> listFiles = new List<FileInfoModel>();
            string fileSavePath = Environment.CurrentDirectory + @"\\wwwroot\\Downloads\\";
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;

            string[] filePaths = Directory.GetFiles(Environment.CurrentDirectory + @"\\wwwroot\\Downloads\\", "*.csv*", SearchOption.AllDirectories);
            List<FileListModel> files = new List<FileListModel>();
            foreach (string filePath in filePaths)
            {
                string subdir = Path.GetDirectoryName(filePath);
                listFiles.Add(new FileInfoModel()
                {
                    FileId = i + 1,
                    FileName = Path.GetFileName(filePath),
                    FilePath = filePath,
                    BankFolder = subdir.Split('\\').Last()
                });
            }
            return listFiles;
        }
    }
}
