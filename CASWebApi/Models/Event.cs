using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class EventTest

    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        [BsonElement("id")]

        public string Id { get; set; }
        [BsonElement("start")]

        public DateTime start { get; set; }
        [BsonElement("end")]

        public DateTime end { get; set; }
        [BsonElement("title")]

        public string title { get; set; }
        [BsonElement("color")]

        public Color color { get; set; }
        [BsonElement("resizable")]

        public Resible resizable { get; set; }
        [BsonElement("draggable")]

        public bool draggable { get; set; }
        [BsonElement("allDay")]

        public bool allDay { get; set; }

    }
    public class Color
    {
        [BsonElement("primary")]

        public string primary { get; set; }
        [BsonElement("secondary")]

        public string secondary { get; set; }

    }

    public class Resible
    {
        [BsonElement("beforeStart")]

        public bool beforeStart = false;
       
        [BsonElement("afterEnd")]

        public bool afterEnd = true;
    }
}
