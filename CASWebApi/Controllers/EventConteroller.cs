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
    public class EventController : ControllerBase
    {
        IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<EventTest>> Get() =>
             _eventService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetEvent")]
        public ActionResult<EventTest> Get(string id)
        {
            var newEvent = _eventService.GetById(id);

            if (newEvent == null)
            {
                return NotFound();
            }

            return newEvent;
        }

        [HttpPost]
        public ActionResult<EventTest> Create(EventTest newEvent)
        {
            if (!(_eventService.Create(newEvent)))
                return NotFound();

            return CreatedAtRoute("GetEvent", new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, EventTest eventIn)
        {
            var newEvent = _eventService.GetById(id);

            if (newEvent == null)
            {
                return NotFound();
            }
            eventIn.Id = id;

            _eventService.Update(id, eventIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var newEvent = _eventService.GetById(id);

            if (newEvent != null && _eventService.RemoveById(newEvent.Id))
                return NoContent();
            return NotFound();
        }
    }
}
