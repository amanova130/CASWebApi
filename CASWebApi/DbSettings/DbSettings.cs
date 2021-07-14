﻿using CASWebApi.IServices;
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
            try
            {
                var client = new MongoClient(settings.Value.ConnectionString);
                database = client.GetDatabase(settings.Value.Database);
            }
            catch(Exception e)
            {
                throw; 
            }
           // database = client.GetDatabase(settings.Value.Database);
        }

        /// <summary>
        /// Function to Create a new document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="document">A new document that we need to insert into DB </param>
        public bool Insert<T>(string collectionName, T document)
        {
            bool res = true;
            var collection = database.GetCollection<T>(collectionName);
           
            try
            {
                collection.InsertOne(document);
            }
            catch(Exception e)
            {
                res = false;
            }
            
                return res;
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
            var filter = Builders<T>.Filter.Eq("status", true);

            return collection.Find(filter).ToList();
        }



        /// <summary>
        ///  Get all documents in given collection with specific filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetListByFilter<T>(string collectionName,string fieldName,string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, value) & Builders<T>.Filter.Eq("status",true);

            return collection.Find(filter).ToList();
        }
        public T GetDocumentByFilter<T>(string collectionName, string fieldName, string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, value) & Builders<T>.Filter.Eq("status", true);

            return collection.Find<T>(filter).FirstOrDefault();
        }
        /// <summary>
        /// Function to delete the given key and it's value,in given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveField<T>(string collectionName,string fieldName,string id)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, id);
            var update = Builders<T>.Update.Unset(fieldName);
            collection.UpdateManyAsync(filter, update);
            return true;
        }
        /// <summary>
        /// removes a specific element from an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PullElement<T>(string collectionName, string fieldName, string id)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, id);
            var update = Builders<T>.Update.Pull(fieldName,id);
            collection.UpdateManyAsync(filter, update);
            return true;
        }
        /// <summary>
        /// removes a specific object from an array 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PullObject<T>(string collectionName, string arrayName, string objectId, string fieldId, string fieldName,string objectKey)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, fieldId);
            var update = Builders<T>.Update.PullFilter(arrayName, Builders<Schedule>.Filter.Eq(objectKey, objectId));
            var result = collection.UpdateOne(filter, update);
            return true;
        }


        public bool PushElement<T>(string collectionName, string arrayName, T element,string fieldId,string fieldName)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, fieldId);
            var update = Builders<T>.Update.Push(arrayName, element);
            collection.UpdateOne(filter, update);
            return true;
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
            var filter = Builders<T>.Filter.Eq("_id", id) & Builders<T>.Filter.Eq("status", true);
            return collection.Find<T>(filter).FirstOrDefault();
        }


        /// <summary>
        /// update or insert depending if document exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <param name="document">Update the existed document with new changes</param>
        public bool Update<T>(string collectionName, string id, T document)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filterId = Builders<T>.Filter.Eq("_id", id) & Builders<T>.Filter.Eq("status", true);
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
           
            var filter = Builders<T>.Filter.Eq("_id", id);
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
            var filter = Builders<T>.Filter.Eq("_id", id);
            var update = Builders<T>.Update.Set("status", false);        
            var updated=collection.UpdateOne(filter, update);
            return updated != null;
           

        }

        public bool RemoveByFilter<T>(string collectionName,string fieldName,string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, value);
            var update = Builders<T>.Update.Set("status", false);
            var updated = collection.UpdateManyAsync(filter, update);
            return updated != null;

        }

        public int GetCountOfDocuments<T>(string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter =  Builders<T>.Filter.Eq("status", true);
            var updated = (int)collection.Find(filter).CountDocuments();
            return updated;
        }
        public int GetCountOfDocumentsByFilter<T>(string collectionName,string field,string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("status", true) & Builders<T>.Filter.Eq(field, value);
            var updated = (int)collection.Find(filter).CountDocuments();
            return updated;
        }


    }
}
