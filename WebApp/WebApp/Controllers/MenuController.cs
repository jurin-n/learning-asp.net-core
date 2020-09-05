using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        private MenuService menuService;

        public MenuController(IConfiguration configuration)
        {
            this.menuService = new MenuService(configuration);
        }

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

            System.Diagnostics.Debug.Write(form["AudioFileDescription"]);
            var files = new List<AudioFile>();
            var UploadFiles = form.Files.GetFiles("formFile");
            for (int i = 0; i < UploadFiles.Count; i++)
            {
                files.Add(new AudioFile() { FileName = UploadFiles[i].FileName, Description = form["AudioFileDescription"+$"{(i+1):00}"] });
            }
            var menu = new Menu() { MenuId=form["MenuId"], Description = form["MenuDescription"], Unit= form["MenuUnit"] ,AudioFiles=files};
            menuService.Add(menu);

            //S3にAudioファイルアップロード
            menuService.PutAudioFilesToS3(form.Files);

            return View(new Menu() { MenuId = "", Description = "" });
        }
    }
}