using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CASWebApi.Models
{
    public class Schedule
    {

        
        [BsonElement("courseName")]
        public string title{ get; set; }
   
        [BsonElement("startTime")]
        public DateTime start { get; set; }
       
        [BsonElement("endTime")]
        public DateTime end { get; set; }
       
        [BsonElement("lastDate")]
        public DateTime lastDate { get; set; }
        [BsonElement("color")]
        public string color { get; set; }

        [BsonElement("eventId")]
        public string eventId { get; set; }
       



    }
}
