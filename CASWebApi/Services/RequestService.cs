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

        public RequestService(IDbSettings settings, ILogger<RequestService> logger)
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
            try
            {
                var request = DbContext.GetById<Request>("request", requestId);
                if (request == null)
                    logger.LogError("RequestService:Cannot get a request with a requestId: " + requestId);
                else
                    logger.LogInformation("RequestService:Fetched request data by id ");
                return request;
            }
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }
        }
        /// <summary>
        /// get all requests from db
        /// </summary>
        /// <returns></returns>
        public List<Request> GetAll()
        {
            logger.LogInformation("RequestService:Getting all requests");
            try
            {
             var requests = DbContext.GetAll<Request>("request");
                logger.LogInformation("RequestService:fetched All requests collection data");
            return requests;
            }
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }
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
            logger.LogInformation("RequestService:Getting requests by sender id");
            try
            {
                var requestList = DbContext.GetListByFilter<Request>("request", "sender_id", senderId);
                return requestList;
            }
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// create new course object in db
        /// </summary>
        /// <param name="course"></param>
        /// <returns>true if successed</returns>
        public bool Create(Request request)
        {
            request.Id = ObjectId.GenerateNewId().ToString();
            try
            {
                DbContext.Insert<Request>("request", request);
                    logger.LogInformation("RequestService:A new request profile added successfully :" + request);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }
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
            try
            {
                DbContext.Update<Request>("request", id, requestIn);
                    logger.LogInformation("RequestService:request with Id" + requestIn.Id + "has been updated successfully");
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }
        }



        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            try
            {
                DbContext.RemoveById<Request>("request", id);
               
                    logger.LogInformation("RequestService:a request profile with id : " + id + "has been deleted successfully");
                return true;
            }
         
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }
}
        /// <summary>
        /// get count of requests by sent filter
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns>number of requests</returns>
        public int GetCountByFilter(string fieldName, string value)
        {
            try
            {
                return DbContext.GetCountOfDocumentsByFilter<Request>("request", fieldName, value);
            }
            catch (Exception e)
            {
                logger.LogError("requestService:got error : " + e);
                throw e;
            }

        }
    }
}
