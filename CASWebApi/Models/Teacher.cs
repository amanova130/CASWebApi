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
        [BsonId]
        [BsonIgnoreIfDefault]
        // [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("f_name")]
        public string First_name { get; set; }

        [BsonElement("l_name")]
        public string Last_name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("birth_date")]
        public DateTime Birth_date { get; set; }

        [BsonElement("address")]
        public AddressBook Address { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("teachesCourses")]

        public string[] TeachesCourses { get; set; }



    }
}
