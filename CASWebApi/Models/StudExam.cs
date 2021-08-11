﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models.DbModels
{ /*
    StudExam class
    Contains all methods and properties for StudExam Model
*/
    public class StudExam
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        [BsonElement("stud_id")]
        public string StudId { get; set; }

        [BsonElement("exam_id")]
        public string ExamId { get; set; }

        [BsonElement("grade")]
        public int Grade { get; set; }
        [BsonElement("updated_date")]
        public string UpdatedDate { get; set; }
        [BsonElement("year")]
        public string Year { get; set; }
        [BsonElement("JoinedField")]
        public Object[] JoinedField { get; set; }
    }

    public class Average
    {
       public string name;
       public double avg;
    }
}
