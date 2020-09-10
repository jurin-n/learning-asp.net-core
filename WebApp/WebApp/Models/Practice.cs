using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class Practice
    {
        public String Id { get; set; }
        public DateTimeOffset DateTimeOfImplementation { get; set; }
        public String MenuId { get; set; }
        public int ValueOfUnit { get; set; }
        public String Unit { get; set; }
        public IList<AudioFile> AudioFiles { get; set; }
    }
}
