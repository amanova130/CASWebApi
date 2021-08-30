using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        //Logger to create streammer of logs 
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        IRequestService _requestService;
        public RequestController(IRequestService requestService, ILogger<RequestController> logger)
        {
            this.logger = logger;
            _requestService = requestService;
        }

        /// <summary>
        /// Get all requests from Database
        /// </summary>
        /// <returns>List of requests</returns>
        [HttpGet("getAllRequests", Name = nameof(GetAllRequests))]
        public ActionResult<List<Request>> GetAllRequests()
        {
            logger.LogInformation("Getting all Requests data");
            try
            {
                var requestList = _requestService.GetAll();
                logger.LogInformation("Fetched all data");
                return requestList;
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }

        [HttpGet("getRequestsListBySenderId", Name = nameof(GetRequestsListBySenderId))]
        public ActionResult<List<Request>> GetRequestsListBySenderId(string senderId)
        {
            logger.LogInformation("Getting all Requests data");
            if (senderId == null || senderId == "")
            {
                logger.LogError("sender Id is null or empty string");
                return BadRequest("Incorrect format of sender param");
            }
            try
            {
                var requestList = _requestService.GetRequestBySenderId(senderId);
                    logger.LogInformation("Fetched all data");
                    return requestList;             
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }


        /// <summary>
        /// Get request profile by Id
        /// </summary>
        /// <param name="id">Id of request</param>
        /// <returns>request profile</returns>
        [HttpGet("getRequestbyId", Name = nameof(GetRequestbyId))]
        public ActionResult<Request> GetRequestbyId(string id)
        {
            logger.LogInformation("Getting Request by Id");
            if (id == null || id == "")
            {
                logger.LogError("Id is null or empty string");
                return BadRequest("Incorrect format of id param");
            }
            try
            {
                var request = _requestService.GetById(id);
                if (request != null)
                {
                    return Ok(request);
                }
                else
                {
                    return NotFound("request with given id not found");
                }
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="request">New request data</param>
        /// <returns>Created request</returns>
        [HttpPost("createNewRequest", Name = nameof(CreateNewRequest))]
        public ActionResult<Request> CreateNewRequest(Request request)
        {
            logger.LogInformation("Creating a new request");
            if (request == null)
            {
                logger.LogError("request Id is null or empty string");
                return BadRequest("Incorrect format of request param");
            }
            request.Status = true;
            try
            {
               _requestService.Create(request);
                logger.LogInformation("A new request profile added successfully " + request);
                return Ok(request);           
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
}

        /// <summary>
        /// Update existed requestIn profile
        /// </summary>
        /// <param name="requestIn"> link to update</param>
        /// <returns>Boolean if requestIn updated true, otherwise false</returns>
        [HttpPut("updateRequest", Name = nameof(UpdateRequest))]
        public IActionResult UpdateRequest(Request requestIn)
        {
            if (requestIn == null)
            {
                logger.LogError("requestIn object is null or empty string");
                return BadRequest("Incorrect format of requestIn param");
            }
            logger.LogInformation("Updating existed requestIn: " + requestIn.Id);
            try
            {
                var link = _requestService.GetById(requestIn.Id);
                if (link == null)
                {
                    logger.LogError("requestIn with Id: " + requestIn.Id + " doesn't exist");
                    return NotFound("request with given id doesn't exists");
                }
                 _requestService.Update(requestIn.Id, requestIn);
                    logger.LogInformation("Given requestIn profile Updated successfully");
                return Ok(requestIn);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// Delete request by Id
        /// </summary>
        /// <param name="id"> request id</param>
        /// <returns>True if request deleted, otherwise false</returns>
        [HttpDelete("deleteRequestById", Name = nameof(DeleteRequestById))]
        public IActionResult DeleteRequestById(string id)
        {
            logger.LogInformation("Deleting Request by Id " + id);
            if (id == null)
            {
                logger.LogError("id is null or empty string");
                return BadRequest("Incorrect format of id param");
            }
            try
            {
                var request = _requestService.GetById(id);
                if (request == null)
                    return NotFound("request with given id not found");
                _requestService.RemoveById(request.Id);
                return Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
        /// <summary>
        /// function to get count of requests with status "new"
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns>number of new requests </returns>
        [HttpGet("getCountOFNewRequest", Name = nameof(GetCountOFNewRequest))]
        public ActionResult<int> GetCountOFNewRequest(string fieldName, string value)
        {
            if (fieldName == null && value == null)
            {
                logger.LogError("fieldName or value is null");
                return BadRequest("Incorrect format of parameters");
            }
                logger.LogInformation("Getting count of Request");
            try
            { 
                var count = _requestService.GetCountByFilter(fieldName, value);
                    return Ok(count);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }


        }

    }
}
