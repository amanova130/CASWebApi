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
   Group class
   Contains all methods and properties for Group Model
*/
    public class Group
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("num_group")]
        public string GroupNumber { get; set; }
        [BsonElement("num_of_students")]

        public int NumberOfStudent { get; set; }
        [BsonElement("academic_year")]

        public int AcademicYear { get; set; }
        [BsonElement("semester")]

        public int Semester { get; set; }
        [BsonElement("courses")]

        public string[] courses { get; set; }
        [BsonElement("fac_name")]

        public string Fac_Name {get;set;}
        


        [BsonElement("status")]

        public bool Status { get; set; }

       

    }
}
