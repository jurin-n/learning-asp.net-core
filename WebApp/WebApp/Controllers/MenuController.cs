using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Menu() { MenuId = "", Description = "" });
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {

            //System.Diagnostics.Debug.Write(form["MenuId"]);

            // Create a client
            using (var client = new AmazonS3Client())
            {
                // ファイルをS3にput
                foreach (var formFile in form.Files)
                {
                    //using (var newMemoryStream = new MemoryStream())
                    using (var stream = formFile.OpenReadStream())
                    {
                        //formFile.CopyTo(newMemoryStream);
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = stream,
                            Key = formFile.FileName,
                            BucketName = "com.jurin-n.audio-files/webapp"
                        };

                        var fileTransferUtility = new TransferUtility(client);
                        fileTransferUtility.Upload(uploadRequest);
                    }
                }
            }

            return View(new Menu() { MenuId = "", Description = "" });
        }
    }
}