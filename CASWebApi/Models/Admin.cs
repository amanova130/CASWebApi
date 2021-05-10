using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Admin
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        [BsonElement("admin_id")]
        public User user { get; set; }

        public int AdminId { get; set; }
        [BsonElement("f_name")]
        public string FirstName { get; set; }
      
        [BsonElement("l_name")]
        public string LastName { get; set; }
        [BsonElement("email")]

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        
    }
}
