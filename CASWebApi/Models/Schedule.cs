using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CASWebApi.Models
{
  /*
   Schedule class
   Contains all methods and properties for schedule Model
*/
    public class Schedule
    {

        
        [BsonElement("courseName")]
        public string Title{ get; set; }
   
        [BsonElement("startTime")]
        public DateTime Start { get; set; }
       
        [BsonElement("endTime")]
        public DateTime End { get; set; }
       
        [BsonElement("lastDate")]
        public DateTime LastDate { get; set; }
        [BsonElement("color")]
        public string Color { get; set; }

        [BsonElement("eventId")]
        public string EventId { get; set; }

        [BsonElement("teacher")]
        public Teacher Teacher { get; set; }




    }
}
