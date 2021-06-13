using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpGet("getAllGroups", Name = nameof(GetAllGroups))]
        public ActionResult<List<Group>> GetAllGroups() =>
             _groupService.GetAll();

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

        [HttpPost("createGroup", Name = nameof(CreateGroup))]
        public ActionResult<Group> CreateGroup(Group group)
        {
            _groupService.Create(group);

            return CreatedAtRoute("getGroupById", new { id = group.Id }, group);
        }

        [HttpPut("updateGroup", Name = nameof(UpdateGroup))]
        public IActionResult UpdateGroup(string id, Group groupIn)
        {
            var group = _groupService.GetById(id);

            if (group == null)
            {
                return NotFound();
            }
            groupIn.Id = id;

            _groupService.Update(id, groupIn);

            return NoContent();
        }

        [HttpDelete("deleteGroupById", Name = nameof(DeleteGroupById))]
        public IActionResult DeleteGroupById(string id)
        {
            var group = _groupService.GetById(id);

            if (group != null && _groupService.RemoveById(group.Id))
                return NoContent();
            return NotFound();
        }
    }
}
