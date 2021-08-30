using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Extended_LinkController : ControllerBase
    {
        //Logger to create streammer of logs 
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        IExtendedLinkService _linkService;
        public Extended_LinkController(IExtendedLinkService linkService, ILogger<Extended_LinkController> logger)
        {
            this.logger = logger;
            _linkService = linkService;
        }

        /// <summary>
        /// Get all links from Database
        /// </summary>
        /// <returns>List of links</returns>
        [HttpGet("getAllLinks", Name = nameof(GetAllLinks))]
        public ActionResult<List<ExtendedLink>> GetAllLinks()
        {
            logger.LogInformation("Getting all Admins data");
            try
            {
                return _linkService.GetAll();   
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }
                 

        /// <summary>
        /// Get link profile by Id
        /// </summary>
        /// <param name="id">Id of link</param>
        /// <returns>link profile</returns>
        [HttpGet("getLinkbyId", Name = nameof(GetLinkbyId))]
        public ActionResult<ExtendedLink> GetLinkbyId(string id)
        {
            logger.LogInformation("Getting Link by Id");
            if (id == null)
            {
                logger.LogError("Given Link Id is null");
                return BadRequest("given id was null");
            }
            try
            { 
                var link = _linkService.GetById(id);
                if (link != null)
                {
                    logger.LogInformation("Fetched data for link id: " + id);
                    return Ok(link);
                }
                else
                {
                    logger.LogError("Link is doesn't exist");
                    return NotFound("link with given id is doesn't exists");
                }
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }

        }

        /// <summary>
        /// Create a new link
        /// </summary>
        /// <param name="link">New link data</param>
        /// <returns>Created link</returns>
        [HttpPost("createNewLink", Name = nameof(CreateNewLink))]
        public ActionResult<ExtendedLink> CreateNewLink(ExtendedLink link)
        {
            logger.LogError("Creating a new Link");
            if (link == null)
            {
                logger.LogError("Link is null");
                return BadRequest("Link is null");
            }
            try
            {
                link.Status = true;
                _linkService.Create(link);
                    return CreatedAtRoute("getLinkbyId", new { id = link.Id }, link);                  
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }


        }

        /// <summary>
        /// Update existed link profile
        /// </summary>
        /// <param name="linkIn"> link to update</param>
        /// <returns>Boolean if link updated true, otherwise false</returns>
        [HttpPut("updateLink", Name = nameof(UpdateLink))]
        public IActionResult UpdateLink(ExtendedLink linkIn)
        {
            logger.LogInformation("Updating existed link: " + linkIn.Id);
            if (linkIn == null || linkIn.Id == null)
            {
                logger.LogError("Given object is null");
                return BadRequest("Given object model is not correct");
            }
            try
            {
                var link = _linkService.GetById(linkIn.Id);
                if (link != null)
                {
                    logger.LogInformation("Given link profile Updated successfully");
                    return Ok(true);
                }
                else
                    logger.LogError("link with given id is desesn't exists");
                return NotFound("link with given id is desesn't exists");

            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// Delete link by Id
        /// </summary>
        /// <param name="id"> link id</param>
        /// <returns>True if link deleted, otherwise false</returns>
        [HttpDelete("deleteLinkById", Name = nameof(DeleteLinkById))]
        public IActionResult DeleteLinkById(string id)
        {
            logger.LogInformation("Deleting link By Id");
            if (id == null)
            {
                logger.LogError("Given Link Id is null");
                return NotFound(false);
            }
            try
            {
                var link = _linkService.GetById(id);
                if (link != null && _linkService.RemoveById(link.Id))
                    return Ok(true);
                else
                    logger.LogError("Cannot access to Link collection in DB");
                return NotFound("link with given id doesn't exists in database");
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }


        }
    }
}
