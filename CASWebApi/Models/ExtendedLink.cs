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
        public string Description { get; set; }
        public string URL { get; set; }

        public string Fac_name { get; set; }
        public string Image { get; set; }
    }
}
