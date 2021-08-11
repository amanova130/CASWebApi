using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class StudentPartial
    {
        [BsonId]
        [BsonIgnoreIfDefault]

        public string Id { get; set; }

        [BsonElement("f_name")]
        public string First_name { get; set; }

        [BsonElement("l_name")]
        public string Last_name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
