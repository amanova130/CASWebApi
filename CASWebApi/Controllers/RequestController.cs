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
            var requestList = _requestService.GetAll();
            if (requestList != null)
            {
                logger.LogInformation("Fetched all data");
                return requestList;
            }
            else
            {
                logger.LogError("Cannot get access to Request collection in Db");
                return StatusCode(500, "Internal server error");
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
            if (id != null)
            {
                logger.LogInformation("Getting Request by Id");
                var request = _requestService.GetById(id);
                if (request != null)
                {
                    return Ok(request);
                }
                else
                {
                    logger.LogError("Cannot get access to Request collection in Db");
                }
            }
            else
                logger.LogError("Request Id is null or empty string");
            return BadRequest(null);
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
            if (request != null)
            {
                request.Status = true;
                if (_requestService.Create(request))
                    return CreatedAtRoute("getFacById", new { id = request.Id }, request);
                else
                    return StatusCode(409, "Duplicated Id");
            }
            else
                logger.LogError("request object is null " + request);
            return BadRequest(null);
        }

        /// <summary>
        /// Update existed requestIn profile
        /// </summary>
        /// <param name="requestIn"> link to update</param>
        /// <returns>Boolean if requestIn updated true, otherwise false</returns>
        [HttpPut("updateRequest", Name = nameof(UpdateRequest))]
        public IActionResult UpdateRequest(Request requestIn)
        {
            logger.LogInformation("Updating existed requestIn: " + requestIn.Id);
            var link = _requestService.GetById(requestIn.Id);

            if (link == null)
            {
                logger.LogError("requestIn with Id: " + requestIn.Id + " doesn't exist");
                return NotFound(false);
            }
            bool updated = _requestService.Update(requestIn.Id, requestIn);
            if (updated)
                logger.LogInformation("Given requestIn profile Updated successfully");
            else
                logger.LogError("Cannot update the requestIn profile: " + requestIn.Id + " wrong format");

            return Ok(updated);
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
            if (id != null)
            {
                var request = _requestService.GetById(id);
                if (request != null && _requestService.RemoveById(request.Id))
                    return Ok(true);
                else
                    logger.LogError("Cannot get access to request collection in Db");
            }
            else
                logger.LogError("Id is not valid format or null");
            return NotFound(false);
        }

        [HttpGet("getCountOFNewRequest", Name = nameof(GetCountOFNewRequest))]
        public ActionResult<int> GetCountOFNewRequest(string fieldName, string value)
        {
            if (fieldName != null && value != null)
            {
                logger.LogInformation("Getting count of Request");
                var count = _requestService.GetCountByFilter(fieldName, value);
                if (count >= 0)
                {
                    return Ok(count);
                }
                else
                {
                    logger.LogError("Cannot get access to Request collection in Db");
                }
            }
            else
                logger.LogError("Request Id is null or empty string");
            return BadRequest(null);
        }

    }
}
