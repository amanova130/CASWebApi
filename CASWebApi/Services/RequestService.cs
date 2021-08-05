using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class RequestService : IRequestService
    {
        private readonly ILogger logger;

        IDbSettings DbContext;

        public RequestService(IDbSettings settings, ILogger<FacultyService> logger)
        {
            DbContext = settings;
            this.logger = logger;

        }
        /// <summary>
        /// get request object by id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns>found request object</returns>
        public Request GetById(string requestId)
        {
            logger.LogInformation("RequestService:Getting request by id");

            var request = DbContext.GetById<Request>("request", requestId);
            if (request == null)
                logger.LogError("RequestService:Cannot get a request with a requestId: " + requestId);
            else
                logger.LogInformation("RequestService:Fetched request data by id ");
            return request;
            
        }
        /// <summary>
        /// get all requests from db
        /// </summary>
        /// <returns></returns>
        public List<Request> GetAll()
        {
            logger.LogInformation("RequestService:Getting all requests");
            var requests = DbContext.GetAll<Request>("request");
            if (requests == null)
                logger.LogError("RequestService:Cannot get access to requests collection in Db");
            else
                logger.LogInformation("RequestService:fetched All requests collection data");
            return requests;

        }
        /// <summary>
        /// Get Request by sender Id
        /// </summary>
        /// <param name="groupNumber"></param>
        /// <param name="semester"></param>
        /// <param name="year"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public List<Request> GetRequestBySenderId(string senderId)
        {
            var requestList = DbContext.GetListByFilter<Request>("request", "sender_id", senderId);
            if (requestList != null)
            {
                return requestList;
            }
            else
                return null;
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
            if (res)
                logger.LogInformation("RequestService:A new request profile added successfully :" + request);
            else
                logger.LogError("RequestService:Cannot create a request, duplicated id or wrong format");
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
            logger.LogInformation("RequestService:updating an existing request profile with id : " + requestIn.Id);

            bool res = DbContext.Update<Request>("request", id, requestIn);
            if (!res)
                logger.LogError("RequestService:request with Id: " + requestIn.Id + " doesn't exist");
            else
                logger.LogInformation("RequestService:request with Id" + requestIn.Id + "has been updated successfully");
            return res;
        }



        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            bool res = DbContext.RemoveById<Request>("request", id);
            if (res)
            {
                logger.LogInformation("RequestService:a request profile with id : " + id + "has been deleted successfully");
            }
            {
                logger.LogError("RequestService:request with Id: " + id + " doesn't exist");

            }
            return res;

        }
        public int GetCountByFilter(string fieldName, string value)
        {
            return DbContext.GetCountOfDocumentsByFilter<Request>("request", fieldName, value);

        }
    }
}
