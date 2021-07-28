using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Request
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonElement("sender_id")]
        public string SenderId { get; set; }

        [BsonElement("created_date")]
        public string CreatedDate { get; set; }

        [BsonElement("reason")]
        public string Reason { get; set; }

        [BsonElement("subject")]
        public string Subject { get; set; }

        [BsonElement("group_num")]
        public string GroupNum { get; set; }


        [BsonElement("status_request")]
        public string StatusOfRequest { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }


    }
}

