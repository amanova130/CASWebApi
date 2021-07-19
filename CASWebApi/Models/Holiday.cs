using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
 /*
   Holiday class
   Contains all methods and properties for Holiday Model
*/
    public class Holiday
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("startDate")]
        public string StartDate { get; set; }

        [BsonElement("endDate")]
        public string EndDate { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("details")]
        public string Details { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }
}
