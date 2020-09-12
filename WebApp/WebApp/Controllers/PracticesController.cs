using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
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
            IList<Practice> practices = practiceService.GetPractices();

            return View(practices);
        }

        [HttpGet]
        public IActionResult Plan(String menuId)
        {
            Menu menu = menuService.GetMenu(menuId);
            
            return View(new Practice() { MenuId = menu.MenuId, Unit=menu.Unit,AudioFiles=menu.AudioFiles});
        }

        [HttpPost]
        public IActionResult Plan(IFormCollection form) 
        {
            var practice = new Practice() {
                Id = Guid.NewGuid().ToString(), 
                DateTimeOfImplementation = DateTime.Parse(form["Date"]),
                MenuId = form["MenuId"],
                ValueOfUnit = int.Parse(form["ValueOfUnit"]),
            };
            practiceService.Add(practice);
            return Redirect("~/menu");
        }
    }
}
