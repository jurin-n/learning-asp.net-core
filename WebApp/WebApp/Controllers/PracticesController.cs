using System;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class PracticesController : Controller
    {
        private PracticeService practiceService;
        private MenuService menuService;

        public PracticesController(AppConfig appConfig)
        {
            this.practiceService = new PracticeService(appConfig);
            this.menuService = new MenuService(appConfig);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Plan(String menuId)
        {
            Menu menu = menuService.GetMenu(menuId);
            
            return View(new Practice() { MenuId = menu.MenuId, Unit=menu.Unit,AudioFiles=menu.AudioFiles});
        }
    }
}
