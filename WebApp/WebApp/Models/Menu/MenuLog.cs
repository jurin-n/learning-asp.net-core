using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApp.Models.Menu
{
    public class MenuLog
    {
        public string MenuId { get; internal set; }
        public string Unit { get; internal set; }
        public IList<Log> Logs { get; set; }
    }
}
