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
    public class ScheduleController : ControllerBase
    {
        IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public ActionResult<List<Schedule>> Get() =>
             _scheduleService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetSchedule")]
        public ActionResult<Schedule> Get(string id)
        {
            var newEvent = _scheduleService.GetById(id);

            if (newEvent == null)
            {
                return NotFound();
            }

            return newEvent;
        }

        [HttpPost]
        public ActionResult<Schedule> Create(Schedule newEvent)
        {
            Calendar.CreateEvent(newEvent);

            if (!(_scheduleService.Create(newEvent)))
                return NotFound();

            return Ok("added");
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Schedule eventIn)
        {
            var newEvent = _scheduleService.GetById(id);

            if (newEvent == null)
            {
                return NotFound();
            }
            eventIn.EventId = id;

            _scheduleService.Update(id, eventIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var newEvent = _scheduleService.GetById(id);

            if (newEvent != null && _scheduleService.RemoveById(newEvent.EventId))
                return NoContent();
            return NotFound();
        }
    }
}
