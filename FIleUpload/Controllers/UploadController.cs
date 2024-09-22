using FIleUpload.Models;
using FIleUpload.Controllers;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
namespace FIleUpload.Controllers
{
    public class UploadController : ControllerBase
    {
        private FileDb db;
        public UploadController(IConfiguration config)
        {
            db = new FileDb(config.GetConnectionString("connstr"));
        }

        [HttpPost("Upload", Name = "UploadFile")]

        public async Task<int> UploadFile(IFormFile file)
        {
            if (file.ContentType != "text/plain")
            {
                throw new Exception("File type not supported");
            }
            if (Request.ContentLength > (5 * 1024 * 1024))
            {
                throw new Exception($"Uploaded file is greater than 5 MB");
            }

            string fileName = $@"C:\Users\prinamda\source\repos\FIleUpload\{Guid.NewGuid().ToString()}.txt";

            using (FileStream writer = new FileStream(fileName, FileMode.Create))
            {

                file.CopyTo(writer);
            }
            TextFile t = new TextFile() { Name = fileName, Size = Request.ContentLength.Value };
            db.TextFile.Add(t);
            db.SaveChanges();
            Process.Start($@"C:\Users\prinamda\source\repos\FIleUpload\FileProcessor\bin\Debug\net8.0\FileProcessor.exe", $"{fileName}" );
            return t.Id;
        }

        [HttpGet("File/{Id}", Name ="GetFileData")]
        public IEnumerable<TextFile> GetFileData(int Id)
        {
            var r = (from t in db.TextFile
                     where t.Id == Id
                     select t).ToList();
            return r;
        } 

    }
}
