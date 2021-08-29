using MongoDB.Bson;
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
        [BsonIgnoreIfNull]
        public Object[] JoinedField { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }

    public class Average
    {
       public string Name;
       public string Id;
       public List<CourseAvg> courseAvg;

        public Average(string id,string studentName)
        {
            Id = id;
            Name = studentName;
            courseAvg = new List<CourseAvg>();
        }
    }

    public class CourseAvg
    {
        public string courseName;
        public double avg;
        public CourseAvg(string course, double avg)
        {
            courseName = course;
            this.avg = avg;
            
        }
    }

    
}
