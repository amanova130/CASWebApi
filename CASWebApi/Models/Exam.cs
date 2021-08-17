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
    Exam class
    Contains all methods and properties for exam Model
*/
    public class Exam
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        
        [BsonElement("course")]
        public string Course { get; set; }

        [BsonElement("fac_name")]
        public string Fac_name { get; set; }

        [BsonElement("group_num")]
        public string Group_num { get; set; }

        [BsonElement("room")]
        public int Room { get; set; }

        [BsonElement("teacher_id")]
        public string Teacher_id { get; set; }

        [BsonElement("test_num")]
        public string Test_num { get; set; }

        [BsonElement("startTime")]
        public string StartTime { get; set; }
        [BsonElement("endTime")]
        public string EndTime { get; set; }

        [BsonElement("semester")]
        public string Semester { get; set; }

        [BsonElement("year")]
        public string Year { get; set; }

        [BsonElement("examDate")]
        public string ExamDate{get;set;}
        [BsonElement("JoinedField")]
        [BsonIgnoreIfNull]
        public StudExam[] JoinedField { get; set; }


        [BsonElement("status")]
        public Boolean Status { get; set; }
        
    }
}
