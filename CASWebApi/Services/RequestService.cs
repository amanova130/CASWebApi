using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class RequestService : IRequestService
    {
        IDbSettings DbContext;

        public RequestService(IDbSettings settings)
        {
            DbContext = settings;
        }
        /// <summary>
        /// get request object by id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>found request object</returns>
        public Request GetById(string requestId)
        {
            return DbContext.GetById<Request>("request", requestId);
        }
        /// <summary>
        /// get all requests from db
        /// </summary>
        /// <returns></returns>
        public List<Request> GetAll()
        {
            return DbContext.GetAll<Request>("request");

        }

        /// <summary>
        /// create new course object in db
        /// </summary>
        /// <param name="course"></param>
        /// <returns>true if successed</returns>
        public bool Create(Request request)
        {
            request.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Request>("request", request);
            return res;
        }

        /// <summary>
        /// edit an existing course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestIn"></param>
        /// <returns>true if successed</returns>
        public bool Update(string id, Request requestIn)
        {
            return DbContext.Update<Request>("request", id, requestIn);
        }



        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            bool res = DbContext.RemoveById<Request>("request", id);
            return res;

        }
    }
}
