using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class MenuController : Controller
    {
        private MenuService menuService;

        public MenuController(AppConfig appConfig)
        {
            this.menuService = new MenuService(appConfig);
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            if(string.IsNullOrEmpty(id))
            { 
                IList<Menu> menus = menuService.GetMenus();
                return View(menus);
            }
            Menu menu = menuService.GetMenu(id);
            return View("Edit",menu);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Menu() { MenuId = "", Description = "" ,AudioFiles = new List<AudioFile> { new AudioFile(), new AudioFile(), new AudioFile() } });
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
            menuService.PutAudioFilesToS3(form["MenuId"], form.Files);

            return View(new Menu() { MenuId = "", Description = "" });
        }
    }
}