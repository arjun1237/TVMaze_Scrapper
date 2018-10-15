using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVMaze_Scrapper.Models
{
    public class ScrapedShow
    {
        public int id { get; set; }

        public string name { get; set; }
    }

    public class ScrapedCast
    {
        public Cast person { get; set; }
    }    
}