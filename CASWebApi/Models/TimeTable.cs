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
    timeTable class
    Contains all methods and properties for timeTable Model
*/
    public class TimeTable
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        [BsonElement("groupName")]

        public string CalendarName { get; set; }
        [BsonElement("schedule")]

        public Schedule[] GroupSchedule { get; set; }
        [BsonElement("calendarId")]
        public string CalendarId { get; set; }

        [BsonElement("status")]

        public bool status { get; set; }
    }
}
