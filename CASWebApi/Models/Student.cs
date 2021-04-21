using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Student
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        public string s_id { get; set; }

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

        [BsonElement("class_num")]
        public string ClassNum { get; set;}

    }
}
