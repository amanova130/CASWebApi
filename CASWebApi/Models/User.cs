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
    User class
    Contains all methods and properties for User Model
*/
    public class User
    {

        [BsonId]
        [BsonIgnoreIfDefault]
        public string UserName { get; set; }
      

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("log_in_time")]
        public DateTime LogIn { get; set; }
        
        [BsonElement("log_off_time")]
        public DateTime LogOff { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("changePwdDate")]
        public string ChangePwdDate { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }
}
