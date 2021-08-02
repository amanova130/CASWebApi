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
        ITimeTableService _timeTableService;
        public GroupController(IGroupService groupService,ITimeTableService timeTableService, ILogger<GroupController> logger)
        {
            this.logger = logger;
            _groupService = groupService;
            _timeTableService = timeTableService;
        }

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <returns>List of Groups</returns>
        [HttpGet("getAllGroups", Name = nameof(GetAllGroups))]
        public ActionResult<List<Group>> GetAllGroups()
        {
            logger.LogInformation("Getting all Faculties data");
            var groupList = _groupService.GetAll();
            if(groupList != null)
            {
                logger.LogInformation("Fetched all data");
                return groupList;
            }
            else
            {
                logger.LogError("Cannot get access to Group collection in Db");
                return StatusCode(500, "Internal server error");
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
            var numberOfGroups = _groupService.GetNumberOfGroups();
            if (numberOfGroups > 0)
            {
                return Ok(numberOfGroups);
            }
            else
            {
                logger.LogError("Cannot get access to Group collection in Db");
                return StatusCode(500, "Internal server error");
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
            if (id != null)
            {
                var group = _groupService.GetById(id);
                if (group != null)
                {
                    return Ok(group);
                }
                else
                {
                    logger.LogError("Cannot get access to Group collection in Db");
                }
            }
            else
                logger.LogError("Group Id is null or empty string");
            return BadRequest(null);
        }

        [HttpGet("getGroupsByFaculty", Name = nameof(GetGroupsByFaculty))]
        public ActionResult<List<Group>> GetGroupsByFaculty(string id)
        {
            logger.LogInformation("Getting group by Faculty id ");
            if(id != null)
            {
                var groups = _groupService.GetGroupsByFaculty(id);
                if (groups != null)
                {
                    return Ok(groups);
                }
                else
                    logger.LogError("Groups by Faculty id not found or cannot access to DB");
            }
            else
                logger.LogError("Faculty Id is null");
            return BadRequest(null);

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
            if(group != null)
            {
                TimeTable timeTable = new TimeTable();
                timeTable.CalendarName = group.GroupNumber;
                timeTable.GroupSchedule = new Schedule[0];
                timeTable.status = true;
                group.Status = true;
                if (_groupService.Create(group))
                {
                    timeTable.CalendarId = CalendarService.CreateCalendar(timeTable.CalendarName);
                    if (timeTable.CalendarId != null)
                    {
                        _timeTableService.Create(timeTable);
                        return CreatedAtRoute("getGroupById", new { id = group.Id }, group);
                    }
                    else
                        logger.LogError("Failed to create a calendar for Group ");
                }
                else
                    logger.LogError("Failed to access to DB and create a new group");
            }
            else
                logger.LogError("Group is null");
            return BadRequest(null);

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
            if(groupIn != null)
            {
                var group = _groupService.GetById(groupIn.Id);

                if (group == null)
                {
                   if(_groupService.Update(groupIn.Id, groupIn))
                    {
                        logger.LogInformation("Given Group profile Updated successfully");
                        return Ok(true);
                    }
                    else
                        logger.LogError("Cannot update the Faculty profile: " + groupIn.Id + " wrong format");
                }
                else
                    logger.LogError("Faculty with Id: " + groupIn.Id + " doesn't exist");
            }
            else
                logger.LogError("CourseIn objest is null");
            return BadRequest(false);
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
            if (id != null)
            {
                var group = _groupService.GetById(id);
                if (group != null && _groupService.RemoveById(group.Id) && _timeTableService.RemoveById(group.GroupNumber))
                    return Ok(true);
                else
                    logger.LogError("Cannot get access to group collection in Db");
            }
            else
                logger.LogError("Id is not valid format or null");
            return NotFound(false);
        }
    }
}
