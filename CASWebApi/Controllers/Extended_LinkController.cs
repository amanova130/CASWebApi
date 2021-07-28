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
        public ActionResult<List<ExtendedLink>> GetAllLinks() =>
                 _linkService.GetAll();

        /// <summary>
        /// Get link profile by Id
        /// </summary>
        /// <param name="id">Id of link</param>
        /// <returns>link profile</returns>
        [HttpGet("getLinkbyId", Name = nameof(GetLinkbyId))]
        public ActionResult<ExtendedLink> GetLinkbyId(string id)
        {
            var link = _linkService.GetById(id);

            if (link == null)
            {
                return NotFound();
            }

            return link;
        }

        /// <summary>
        /// Create a new link
        /// </summary>
        /// <param name="link">New link data</param>
        /// <returns>Created link</returns>
        [HttpPost("createNewLink", Name = nameof(CreateNewLink))]
        public ActionResult<ExtendedLink> CreateNewLink(ExtendedLink link)
        {
            link.Status = true;
            if (!(_linkService.Create(link)))
                return NotFound();

            return CreatedAtRoute("getLinkbyId", new { id = link.Id }, link);
        }

        /// <summary>
        /// Update existed link profile
        /// </summary>
        /// <param name="courseIn"> link to update</param>
        /// <returns>Boolean if link updated true, otherwise false</returns>
        [HttpPut("updateLink", Name = nameof(UpdateLink))]
        public IActionResult UpdateLink(ExtendedLink linkIn)
        {
            logger.LogInformation("Updating existed link: " + linkIn.Id);
            var link = _linkService.GetById(linkIn.Id);

            if (link == null)
            {
                logger.LogError("link with Id: " + linkIn.Id + " doesn't exist");
                return NotFound(false);
            }
            bool updated = _linkService.Update(linkIn.Id, linkIn);
            if (updated)
                logger.LogInformation("Given link profile Updated successfully");
            else
                logger.LogError("Cannot update the link profile: " + linkIn.Id + " wrong format");

            return Ok(updated);
        }

        /// <summary>
        /// Delete link by Id
        /// </summary>
        /// <param name="id"> link id</param>
        /// <returns>True if link deleted, otherwise false</returns>
        [HttpDelete("deleteLinkById", Name = nameof(DeleteLinkById))]
        public IActionResult DeleteLinkById(string id)
        {
            var link = _linkService.GetById(id);

            if (link != null && _linkService.RemoveById(link.Id))
                return Ok(true);
            return NotFound(false);
        }
    }
}
