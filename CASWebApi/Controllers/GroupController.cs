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

        [HttpGet]
        public ActionResult<List<Group>> Get() =>
             _groupService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetGroup")]
        public ActionResult<Group> Get(string id)
        {
            var group = _groupService.GetById(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        [HttpPost]
        public ActionResult<Group> Create(Group group)
        {
            _groupService.Create(group);

            return CreatedAtRoute("GetGroup", new { id = group.Id }, group);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Group groupIn)
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

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var group = _groupService.GetById(id);

            if (group == null)
            {
                return NotFound();
            }

            _groupService.RemoveById(group.Id);

            return NoContent();
        }
    }
}
