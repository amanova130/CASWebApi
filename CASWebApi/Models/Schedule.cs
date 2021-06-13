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
        public string Summary{ get; set; }
    [BsonElement("startTime")]

        public DateTime StartTime { get; set; }
        [BsonElement("endTime")]

        public DateTime EndTime { get; set; }
        [BsonElement("eventId")]
        public string EventId { get; set; }

        [BsonElement("groupId")]
        public string GroupId { get; set; }

    }
}
