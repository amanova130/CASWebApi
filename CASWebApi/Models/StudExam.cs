using MongoDB.Bson.Serialization.Attributes;
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
        [BsonElement("exam_id")]
        public string ExamId { get; set; }

        [BsonElement("grade")]
        public int Grade { get; set; }
    }

    public class Average
    {
       public string name;
       public double avg;
    }
}
