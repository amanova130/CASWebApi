using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    /*
   Message Class
   Contains all methods and properties for Message Model
*/
    public class Message
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("description")]

        public string Description { get; set; }
        [BsonElement("subject")]

        public string Subject { get; set; }

        [BsonElement("receiver_id")]

        public ReceiverDetails[] Receiver { get; set; }
        [BsonElement("sender_id")]

        public string Sender { get; set; }
        [BsonElement("dateTime")]

        public DateTime DateTime { get; set; }
        [BsonElement("status")]

        public bool status { get; set; }
    }

    public class ReceiverDetails
    {
        [BsonElement("Id")]

        public string Id;
        [BsonElement("Email")]
        public string Email;
    }

}
