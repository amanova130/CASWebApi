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
        public ActionResult<List<Request>> GetAllRequests() =>
                 _requestService.GetAll();

        /// <summary>
        /// Get request profile by Id
        /// </summary>
        /// <param name="id">Id of request</param>
        /// <returns>request profile</returns>
        [HttpGet("getRequestbyId", Name = nameof(GetRequestbyId))]
        public ActionResult<Request> GetRequestbyId(string id)
        {
            var request = _requestService.GetById(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="request">New request data</param>
        /// <returns>Created request</returns>
        [HttpPost("createNewRequest", Name = nameof(CreateNewRequest))]
        public ActionResult<Request> CreateNewRequest(Request request)
        {
            request.Status = true;
            if (!(_requestService.Create(request)))
                return NotFound();

            return CreatedAtRoute("getRequestbyId", new { id = request.Id }, request);
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
            var request = _requestService.GetById(id);

            if (request != null && _requestService.RemoveById(request.Id))
                return Ok(true);
            return NotFound(false);
        }
    }
}
