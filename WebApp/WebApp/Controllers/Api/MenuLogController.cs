using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuLogController : ControllerBase
    {
        private PracticeService practiceService;

        public MenuLogController(AppConfig appConfig)
        {
            this.practiceService = new PracticeService(appConfig);
        }

        [HttpGet]
        public Dictionary<string, Object> GetMenuLog([FromQuery] string id,[FromQuery] string start,[FromQuery] string end)
        {
            var menuLogs = practiceService.GetMenuLogs(id, start, end);
            var response = new Dictionary<string, Object>();
            if(menuLogs.Count != 0)
            {
                response.Add("menuId", menuLogs[0].MenuId);
                response.Add("unit", menuLogs[0].Unit);
                var logs = new List<MenuLog>();
                foreach(MenuLog log in menuLogs)
                {
                    logs.Add(new MenuLog() {
                            DateTimeOfImplementation= log.DateTimeOfImplementation,
                            ValueOfUnit = log.ValueOfUnit,
                        }
                    );
                }
                response.Add("logs", logs);
            }
            return response;
        }
    }
}
