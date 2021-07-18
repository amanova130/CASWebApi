using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <returns>List of Groups</returns>
        [HttpGet("getAllGroups", Name = nameof(GetAllGroups))]
        public ActionResult<List<Group>> GetAllGroups() =>
             _groupService.GetAll();

        /// <summary>
        /// Get Number of existed Groups
        /// </summary>
        /// <returns>Number of Groups</returns>
        [HttpGet("getNumberOfGroups", Name = nameof(getNumberOfGroups))]
        public ActionResult<int> getNumberOfGroups() =>
             _groupService.GetNumberOfGroups();

        /// <summary>
        /// Get Group profile by Id
        /// </summary>
        /// <param name="id">Group Id</param>
        /// <returns>Group profile</returns>
        [HttpGet("getGroupById", Name = nameof(GetGroupById))]
        public ActionResult<Group> GetGroupById(string id)
        {
            var group = _groupService.GetById(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        /// <summary>
        /// Create a new Group
        /// </summary>
        /// <param name="group">Group profile to create a new one</param>
        /// <returns>Created Group profile</returns>
        [HttpPost("createGroup", Name = nameof(CreateGroup))]
        public ActionResult<Group> CreateGroup(Group group)
        {
            group.Status = true;
            _groupService.Create(group);

            return CreatedAtRoute("getGroupById", new { id = group.Id }, group);
        }

        /// <summary>
        /// Update existed group profile
        /// </summary>
        /// <param name="groupIn">Group profile to update</param>
        /// <returns>True if group updated, otherwise false</returns>
        [HttpPut("updateGroup", Name = nameof(UpdateGroup))]
        public IActionResult UpdateGroup(Group groupIn)
        {
            bool updated = false;
            var group = _groupService.GetById(groupIn.Id);

            if (group == null)
            {
                return NotFound();
            }
           updated=_groupService.Update(groupIn.Id, groupIn);

            return Ok(updated);
        }

        /// <summary>
        /// Delete Group by Id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>True if group deleted, otherwise false</returns>
        [HttpDelete("deleteGroupById", Name = nameof(DeleteGroupById))]
        public IActionResult DeleteGroupById(string id)
        {
            var group = _groupService.GetById(id);

            if (group != null && _groupService.RemoveById(group.Id))
                return Ok(true);
            return NotFound(false);
        }
    }
}
