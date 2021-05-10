using CASWebApi.IServices;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models.DbModels
{
    public class DbSettings : IDbSettings
    {
        public IMongoDatabase database { get; }

        public DbSettings(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            database = client.GetDatabase(settings.Value.Database);
        }

        /// <summary>
        /// Function to Create a new document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="document"></param>
        public void Insert<T>(string collectionName, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            collection.InsertOne(document);
        }

        /// <summary>
        /// Get all documents in given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            /*var filter = Builders<T>.Filter.Eq("status", true);

            return collection.Find(filter).ToList();*/
            return collection.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// Get document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(string collectionName, string id)
        {
            var collection = database.GetCollection<T>(collectionName);
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                var i = false;
            }
            var filter = Builders<T>.Filter.Eq("_id", id);

            return collection.Find<T>(filter).FirstOrDefault();
        }


        /// <summary>
        /// update or insert depending if document exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <param name="document"></param>
        public bool Update<T>(string collectionName, string id, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                var i = false;
            }
            var filterId = Builders<T>.Filter.Eq("_id", objectId);
            var updated = collection.FindOneAndReplace(filterId, document);
            return updated != null;
        }

        /// <summary>
        /// Update document by changed attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        public void UpdateRecordAttribute<T>(string collectionName, string id, string attributeName, string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                var i = false;
            }
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            var update = Builders<T>.Update.Set(attributeName, value);

            collection.UpdateOne(filter, update);
        }


        /// <summary>
        /// Delete document by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        public bool RemoveById<T>(string collectionName, string id)
        {
            var collection = database.GetCollection<T>(collectionName);
            ObjectId objectId;
            if (!ObjectId.TryParse(id.ToString(), out objectId))
            {
                return false;
            }
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            collection.DeleteOne(filter);
            return true;
        }



    }
}
