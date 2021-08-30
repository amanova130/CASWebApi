using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ILogger logger;
        IGroupService _groupService;
        public GroupController(IGroupService groupService, ILogger<GroupController> logger)
        {
            this.logger = logger;
            _groupService = groupService;
        }

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <returns>List of Groups</returns>
        [HttpGet("getAllGroups", Name = nameof(GetAllGroups))]
        public ActionResult<List<Group>> GetAllGroups()
        {
            logger.LogInformation("Getting all Faculties data");
            try
            { 
            var groupList = _groupService.GetAll();
             logger.LogInformation("Fetched all data");
                return groupList;
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }
             

        /// <summary>
        /// Get Number of existed Groups
        /// </summary>
        /// <returns>Number of Groups</returns>
        [HttpGet("getNumberOfGroups", Name = nameof(getNumberOfGroups))]
        public ActionResult<int> getNumberOfGroups()
        {
            logger.LogInformation("Getting number of Groups");
            try
            {
                var numberOfGroups = _groupService.GetNumberOfGroups();
                   return numberOfGroups;                                     
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
             

        /// <summary>
        /// Get Group profile by Id
        /// </summary>
        /// <param name="id">Group Id</param>
        /// <returns>Group profile</returns>
        [HttpGet("getGroupById", Name = nameof(GetGroupById))]
        public ActionResult<Group> GetGroupById(string id)
        {
            if (id == null || id == "")
            {
                logger.LogError("Group Id is null or empty string");
                return BadRequest("Incorrect format of Id param");
            }
            try
            {
                var group = _groupService.GetById(id);
                if (group != null)             
                    return Ok(group);    
                    logger.LogError("group with given id doesn't exists");
                        return NotFound("group with given id doesn't exists");     
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }



        [HttpGet("getGroupsByFaculty", Name = nameof(GetGroupsByFaculty))]
        public ActionResult<List<Group>> GetGroupsByFaculty(string id)
        {
            logger.LogInformation("Getting group by Faculty id ");
            if (id == null)
            {
                logger.LogError("Faculty Id is null");
                return BadRequest("incorrect format of id param");
            }
            try
            {
                var groups = _groupService.GetGroupsByFaculty(id);
                    return Ok(groups);   
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
        

        /// <summary>
        /// Create a new Group
        /// </summary>
        /// <param name="group">Group profile to create a new one</param>
        /// <returns>Created Group profile</returns>
        [HttpPost("createGroup", Name = nameof(CreateGroup))]
        public ActionResult<Group> CreateGroup(Group group)
        {
            logger.LogInformation("Creating a new group and creating timetable for this group");
            if (group == null)
            {
                logger.LogError("Group is null");
                return BadRequest(null);
            }
            try
            {
             _groupService.Create(group);
             return CreatedAtRoute("getGroupById", new { id = group.Id }, group);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }

        /// <summary>
        /// Update existed group profile
        /// </summary>
        /// <param name="groupIn">Group profile to update</param>
        /// <returns>True if group updated, otherwise false</returns>
        [HttpPut("updateGroup", Name = nameof(UpdateGroup))]
        public IActionResult UpdateGroup(Group groupIn)
        {
            logger.LogInformation("Updating existed group profile");
            if (groupIn == null)
            {
                logger.LogError("group objest is null");
                return BadRequest("Incorrect format of group object");
            }
            try
            {
                var group = _groupService.GetById(groupIn.Id);

                if (group != null)
                {
                    _groupService.Update(groupIn.Id, groupIn);
                    logger.LogInformation("Given Group profile Updated successfully");
                    return Ok(true);
                }
                else
                {
                    logger.LogError("group with Id: " + groupIn.Id + " doesn't exist");
                    return NotFound("group with given id doesn't exists");
                }
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }




        }

        /// <summary>
        /// Delete Group by Id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>True if group deleted, otherwise false</returns>
        [HttpDelete("deleteGroupById", Name = nameof(DeleteGroupById))]
        public IActionResult DeleteGroupById(string id)
        {
            logger.LogInformation("Deleting Course by Id " + id);
            if (id == null)
            {
                logger.LogError("Id is not valid format or null");
                return NotFound("Incorrect format of given id param");
            }
            try
            {
                var group = _groupService.GetById(id);
                if (group != null)
                {
                    _groupService.RemoveById(group.Id, group.GroupNumber);
                    return Ok(true);
                }
                    logger.LogError("group with given id not found");
                return NotFound("group with given id not found");
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }                  
    }
}

