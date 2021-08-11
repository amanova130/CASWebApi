using CASWebApi.IServices;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

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
            catch (Exception e)
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
            catch (Exception e)
            {
                res = false;
            }

            return res;
        }

        public bool InsertMany<T>(string collectionName, List<T> listOfDocuments)
        {
            bool res = true;
            var collection = database.GetCollection<T>(collectionName);
            try
            {
                collection.InsertMany(listOfDocuments);
            }
            catch (Exception e)
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
            var result = collection.Find(filter).ToList();
            return result;
        }



        /// <summary>
        ///  Get all documents in given collection with specific filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">collection's name in db</param>
        /// <param name="fieldName">name of specific field in collection</param>
        /// <param name="value">value of fieldname</param>
        /// <returns></returns>
        public List<T> GetListByFilter<T>(string collectionName, string fieldName, string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, value) & Builders<T>.Filter.Eq("status", true);

            return collection.Find(filter).ToList();
        }
        /// <summary>
        /// Get single document in given collection with specific filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        public bool RemoveField<T>(string collectionName, string fieldName, string id)
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
            var update = Builders<T>.Update.Pull(fieldName, id);
            var res=collection.UpdateManyAsync(filter, update);
            return res != null;
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
            var update = Builders<T>.Update.PullFilter(arrayName, Builders<T>.Filter.Eq(objectKey, objectId));
            var result = collection.UpdateOne(filter, update);
            return result.IsAcknowledged;
        }
        


        /// <summary>
        /// push new element to array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="arrayName"></param>
        /// <param name="element"></param>
        /// <param name="fieldId"></param>
        /// <param name="fieldName"></param>
        /// <returns>true if added,false otherwise </returns>
        public bool PushElement<T>(string collectionName, string arrayName, T element, string fieldId, string fieldName)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, fieldId);
            var update = Builders<T>.Update.Push(arrayName, element);
            var res=collection.UpdateOne(filter, update);
            return res.IsAcknowledged;
        }

        /// <summary>
        /// Get document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="id"></param>
        /// <returns>found object</returns>
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
        /// <returns>true if updated successfully</returns>
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
        public bool UpdateRecordAttribute<T>(string collectionName, string id, string attributeName, string value)
        {
            var collection = database.GetCollection<T>(collectionName);

            var filter = Builders<T>.Filter.Eq("_id", id);
            var update = Builders<T>.Update.Set(attributeName, value);

           var updated= collection.UpdateOne(filter, update);
            return updated != null;
        }


        /// <summary>
        /// Delete document by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">collection's name</param>
        /// <param name="id">id value of the document to remove</param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById<T>(string collectionName, string id)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var update = Builders<T>.Update.Set("status", false);
            var updated = collection.UpdateOne(filter, update);
            
            return updated.IsAcknowledged;
        }

        public List<T> AggregateJoinDocuments<T>(LookUpDetails filterDetails)
        {
            var collection2 = database.GetCollection<T>(filterDetails.CollectionName);

            var _match = new BsonDocument("$match", new BsonDocument(filterDetails.MatchField, filterDetails.Match));
            //var match2 = new BsonDocument("$match", new BsonDocument("CD_CLIENTE", codCond));

            var lookup1 = new BsonDocument { { "$lookup", new BsonDocument { { "from", filterDetails.CollectionNameFrom },
                                                                            { "localField", filterDetails.LocalField },
                                                                            { "foreignField", filterDetails.ForeignField }, 
                                                                            { "as", filterDetails.JoinedField } } } };
            var pipeline = new[] { _match, lookup1 };
            var result = collection2.Aggregate<T>(pipeline).ToList();
            return result;
        }

        /// <summary>
        /// delete document by specific filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">collection's name</param>
        /// <param name="fieldName">A field to which a filter should be made</param>
        /// <param name="value">value of the field</param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveByFilter<T>(string collectionName, string fieldName, string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq(fieldName, value);
            var update = Builders<T>.Update.Set("status", false);
            var updated = collection.UpdateManyAsync(filter, update);
            return updated != null;

        }
        /// <summary>
        /// a fuction to get number of documents in given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">collection where we need to count the number of documents</param>
        /// <returns>number of documents</returns>
        public int GetCountOfDocuments<T>(string collectionName)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter =  Builders<T>.Filter.Eq("status", true);
            var countOfDocs = (int)collection.Find(filter).CountDocuments();
            return countOfDocs;
        }

        /// <summary>
        /// a fuction to get number of documents in given collection with specific filter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">collection where we need to count the number of documents</param>
        /// <param name="field">field to filter</param>
        /// <param name="value">value of the field</param>
        /// <returns>number of documents</returns>
        public int GetCountOfDocumentsByFilter<T>(string collectionName,string field, string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("status", true) & Builders<T>.Filter.Eq(field, value);
            var countOfDocs = (int)collection.Find(filter).CountDocuments();
            return countOfDocs;
        }
        public List<T> GetDeletedDocumentsByFilter<T>(string collectionName,string field,string value)
        {
            var collection = database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("status", false) & Builders<T>.Filter.Eq(field, value);
            var deletedList= collection.Find(filter).ToList();    
            return deletedList;

        }


    }
}
