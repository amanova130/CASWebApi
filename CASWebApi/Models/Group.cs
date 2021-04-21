using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Group
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        [BsonElement("id")]
        public string Id { get; set; }
       
        [BsonElement("group_id")]
        public string GroupId { get; set; }
        [BsonElement("num_group")]
        public string GroupNumber { get; set; }
        [BsonElement("num_of_students")]

        public int NumberOfStudent { get; set; }
        [BsonElement("academic_year")]

        public int AcademicYear { get; set; }
        [BsonElement("semester")]

        public int Semester { get; set; }

       

    }
}
