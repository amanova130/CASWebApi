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
    Faculty class
    Contains all methods and properties for Faculty Model
*/
    public class Faculty
    {

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        [BsonElement("fac_name")]
        public string FacultyName { get; set; }
        public string Description { get; set; }
        [BsonElement("courses")]

        public string[] Courses { get; set; }
        [BsonElement("status")]

        public bool Status { get; set; }

    }
}
