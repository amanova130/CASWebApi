using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class LookUpDetails
    {
        public string CollectionName;
        public string CollectionNameFrom;
        public string MatchField;
        public BsonDocument Match;
        public string LocalField;
        public string ForeignField;
        public string JoinedField;
    }
}
