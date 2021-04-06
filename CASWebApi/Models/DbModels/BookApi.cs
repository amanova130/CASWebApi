using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace CASWebApi.Models.DbModels
{
    public class BookApi
    {
        public class Books
        {
            [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
            [BsonRepresentation(BsonType.ObjectId)]
            [BsonIgnoreIfDefault]
            public string Id { get; set; }

            public string bookName { get; set; }

            public decimal price { get; set; }

            public string category { get; set; }

            public string author { get; set; }
        }
    }
}
