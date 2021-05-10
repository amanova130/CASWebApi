using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models.DbModels
{
    public class StudExam
    {
        [BsonElement("exam_id")]
        public string ExamId { get; set; }
        [BsonElement("grade")]

        public int Grade { get; set; }

        public bool isPassed { get; set; }
    }
}
