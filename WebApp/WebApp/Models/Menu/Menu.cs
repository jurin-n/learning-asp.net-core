using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace WebApp.Models.Menu
{
    public class Menu
    {
        public String MenuId { get; set; }
        public String Description { get; set; }
        public String Unit { get; set; }
        public List<AudioFile> AudioFiles { get; set; }
        public List<SelectListItem> Units { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "回", Text = "回" },
            new SelectListItem { Value = "分", Text = "分"  }
        };
    }
}
