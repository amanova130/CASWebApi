using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
/*
    Address class
    Contains all methods and properties for address Model
*/
    public class AddressBook
    {
        [BsonElement("street")]

        public string Street{ get; set; }
        [BsonElement("city")]

        public string City{ get; set; }
        [BsonElement("zip_code")]

        public int ZipCode { get; set; }

    }
}
