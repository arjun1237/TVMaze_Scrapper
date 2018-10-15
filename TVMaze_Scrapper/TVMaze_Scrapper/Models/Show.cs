using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVMaze_Scrapper.Models
{
    [BsonIgnoreExtraElements]
    public class Show
    {
        public int id { get; set; }

        public string name { get; set; }
        
        public List<Cast> cast { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Cast
    {
        public int id { get; set; }

        public string name { get; set; }

        public string birthday { get; set; }
    }
}
