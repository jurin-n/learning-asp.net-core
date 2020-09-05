using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class Menu
    {
        public String MenuId { get; set; }
        public String Description { get; set; }
        public String Unit { get; set; }

        public List<AudioFile> AudioFiles { get; set; }
    }
}
