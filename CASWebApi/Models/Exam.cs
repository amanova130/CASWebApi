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
    Exam class
    Contains all methods and properties for exam Model
*/
    public class Exam
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        
        [BsonElement("subject")]
        public string Subject { get; set; }

        [BsonElement("fac_id")]
        public string Fac_Id { get; set; }

        [BsonElement("group_id")]
        public string Group_Id { get; set; }

        [BsonElement("room")]
        public int Room { get; set; }

        [BsonElement("teacher_id")]
        public string Teacher_id { get; set; }

        [BsonElement("test_num")]
        public string Test_num { get; set; }

        [BsonElement("duration")]
        public string Duration { get; set; }

        [BsonElement("examDate")]
        public DateTime ExamDate{get;set;}

        [BsonElement("status")]
        public Boolean Status { get; set; }
        
    }
}
