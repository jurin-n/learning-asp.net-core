using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class MenuLog
    {
        public string MenuId { get; internal set; }
        public DateTimeOffset DateTimeOfImplementation { get; internal set; }
        public int ValueOfUnit { get; internal set; }
        public string Unit { get; internal set; }
    }
}
