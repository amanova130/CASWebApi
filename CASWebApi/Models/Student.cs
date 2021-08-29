using CASWebApi.Models.DbModels;
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
   Student class
   Contains all methods and properties for Student Model
*/
    public class Student
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

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }
        [BsonElement("birth_date")]

        public string Birth_date { get; set; }

        [BsonElement("address")]
        public AddressBook Address { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; } = true;

        [BsonElement("group")]
        public string Group_Id { get; set; }
        [BsonElement("image")]
        public string Image { get; set; }

    }
}
