using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.Menu;
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
        public MenuLog GetMenuLog([FromQuery] string id,[FromQuery] string start,[FromQuery] string end)
        {
            return practiceService.GetMenuLog(id, start, end);
        }
    }
}
