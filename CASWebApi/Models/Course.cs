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
       Course class
       Contains all methods and properties for course Model
   */
    public class Course
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        
        [BsonElement("course_name")]
        public string CourseName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("duration")]
        public int Duration { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
        

    }
}
