using CASWebApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of dbSettings service
    /// </summary>
    public interface IDbSettings
    {
        IMongoDatabase database { get; }
        public bool Insert<T>(string collectionName, T document);
        public bool InsertMany<T>(string collectionName, List<T> listOfDocuments);
        public List<T> GetAll<T>(string collectionName);
        public T GetById<T>(string collectionName, string id);
        public bool Update<T>(string collectionName, string id, T document);
        public bool UpdateRecordAttribute<T>(string collectionName, string id, string attributeName, string value);
        public bool RemoveById<T>(string collectionName, string id);
        public bool RemoveField<T>(string collectionName, string fieldName, string id);
        public bool RemoveByFilter<T>(string collectionName, string fieldName, string value);
        public bool PushElement<T>(string collectionName, string arrayName, T element, string fieldId, string fieldName);
        public bool PullElement<T>(string collectionName, string fieldName, string id);
        public bool PullObject<T>(string collectionName, string arrayName, string objectId, string fieldId, string fieldName, string objectKey);

        public T GetDocumentByFilter<T>(string collectionName, string fieldName, string value);
        public List<T> GetListByFilter<T>(string collectionName, string fieldName, string value);
        public int GetCountOfDocuments<T>(string collectionName);
        public int GetCountOfDocumentsByFilter<T>(string collectionName, string field, string value);
        public List<T> GetDeletedDocumentsByFilter<T>(string collectionName, string field, string value);





    }





}
