using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class AudioFile
    {
        public String FileName { get; set; }
        public String Description { get; set; }
        public String S3Url { get; set; }
    }
}
