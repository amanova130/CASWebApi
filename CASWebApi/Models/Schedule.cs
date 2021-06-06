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
        public string Summary;
        [BsonElement("startTime")]

        public DateTime StartTime;
        [BsonElement("endTime")]

        public DateTime EndTime;
        [BsonElement("eventId")]
        public string eventId;

    }
}
