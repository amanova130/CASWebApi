using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Course
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        //
        //[BsonElement("course_id")]
        //public string CourseId { get; set; }
        
        [BsonElement("course_name")]
        public string CourseName { get; set; }
        public string Description { get; set; }
        [BsonElement("duration")]

        public int Duration { get; set; }
        
        [BsonElement("status")]

        public bool status { get; set; }
    }
}
