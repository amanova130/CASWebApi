﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IDbSettings
    {
        IMongoDatabase database { get; }
        public bool Insert<T>(string collectionName, T document);
        public List<T> GetAll<T>(string collectionName);
        public T GetById<T>(string collectionName, string id);
        public bool Update<T>(string collectionName, string id, T document);
        public void UpdateRecordAttribute<T>(string collectionName, string id, string attributeName, string value);
        public bool RemoveById<T>(string collectionName, string id);
        public List<T> GetByFilter<T>(string collectionName, string fieldName, string id);
        public bool RemoveField<T>(string collectionName, string fieldName, string id);
        public bool PullElement<T>(string collectionName, string fieldName, string id);

    }





}
