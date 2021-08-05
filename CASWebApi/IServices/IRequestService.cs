using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IRequestService
    {
        Request GetById(string requestId);
        List<Request> GetAll();
        bool Create(Request request);
        bool Update(string id, Request requestIn);
        bool RemoveById(string id);
        int GetCountByFilter(string fieldName, string value);
        public List<Request> GetRequestBySenderId(string senderId);
    }
}
