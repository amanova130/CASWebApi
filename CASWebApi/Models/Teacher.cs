using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Teacher
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        [BsonElement("teacher_id")]
        public int TeacherId { get; set; }
        [BsonElement("f_name")]
        public string FirstName { get; set; }
        [BsonElement("l_name")]
        public string LastName { get; set; }
        [BsonElement("email")]

        public string Email { get; set; }
        [BsonElement("phone")]

        public string Phone { get; set; }
        [BsonElement("gender")]

        public string Gender { get; set; }
        [BsonElement("birth_date")]

        public DateTime BirthDate { get; set; }
        [BsonElement("teachesCourses")]

        public string[] TeachesCourses { get; set; }
        [BsonElement("status")]

        public bool Status { get; set; }


    }
}
