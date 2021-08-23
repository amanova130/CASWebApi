using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class GradeDetails
    {
        [BsonElement("grade")]
        public int Grade { get; set; }

        [BsonElement("year")]
        public string Year { get; set; }
        [BsonElement("JoinedField")]
        [BsonIgnoreIfNull]
        public Exam[] JoinedField { get; set; }
    }
}
