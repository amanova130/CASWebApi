using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    /*
    ExtendedLink class
    Contains all methods and properties for extendedLink Model
*/
    public class ExtendedLink
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonElement("description_link")]
        public string Description { get; set; }

        [BsonElement("url")]
        public string URL { get; set; }

        [BsonElement("fac_name")]
        public string Fac_name { get; set; }

        [BsonElement("image")]
        public string Image { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }
}
