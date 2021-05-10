using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class AddressBook
    {
        [BsonElement("address")]

        public string Address{ get; set; }
        [BsonElement("city")]

        public string City{ get; set; }
        [BsonElement("zip_code")]

        public int ZipCode { get; set; }
        [BsonElement("status")]

        public bool Status { get; set; }
    }
}
