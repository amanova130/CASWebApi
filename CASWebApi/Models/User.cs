using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class User
    {
      
        [BsonId]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        //[BsonElement("user_id")]
        //public string UserId { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("user_name")]
        public string UserName { get; set; }
        [BsonElement("log_in_time")]
        public DateTime LogIn { get; set; }
        [BsonElement("log_off_time")]
        public DateTime LogOff { get; set; }
        [BsonElement("role")]
        public string Role { get; set; }

    }
}
