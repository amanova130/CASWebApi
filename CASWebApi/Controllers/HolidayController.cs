﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly ILogger logger;
        IHolidayService _holidayService;
        public HolidayController(IHolidayService holidayService, ILogger<HolidayController> logger)
        {
            this.logger = logger;
            _holidayService = holidayService;
        }

        [HttpGet("getAllHolidays", Name = nameof(GetAllHolidays))]
        public ActionResult<List<Holiday>> GetAllHolidays()
        {
            logger.LogInformation("Getting all Holidays from HolidayController");
            var holidayList = _holidayService.GetAll();
            if (holidayList != null)
                logger.LogInformation("Fetched All holiday data");
            else
                logger.LogError("Cannot get access to holiday collection in Db");
            return holidayList;
        }


        [HttpGet("getHolidayById", Name = nameof(GetHolidayById))]
        public ActionResult<Holiday> GetHolidayById(string id)
        {
            logger.LogInformation("Getting holiday by given Id from HolidayController");
            var holiday = _holidayService.GetById(id);

            if (holiday == null)
            {
                logger.LogError("Cannot get access to holiday collection in Db");
            }
            logger.LogInformation("Fetched holiday data by id");

            return holiday;
        }

        [HttpPost("createHoliday", Name = nameof(CreateHoliday))]
        public ActionResult<Holiday> CreateHoliday(Holiday holiday)
        {
            logger.LogInformation("Creating a new holiday profile: " + holiday);
            if (!(_holidayService.Create(holiday)))
            {
                logger.LogError("Cannot create a holiday, duplicated id or wrong format");
                return NotFound(null);
            }
            logger.LogInformation("A new holiday profile added successfully " + holiday);
            return CreatedAtRoute("getHolidayById", new { id = holiday.Id }, holiday);
        }

        [HttpPut("updateHoliday", Name = nameof(UpdateHoliday))]
        public IActionResult UpdateHoliday(Holiday holidayIn)
        {
            logger.LogInformation("Updating existed Holiday profile: " + holidayIn.Id);
            var teacher = _holidayService.GetById(holidayIn.Id);

            if (teacher == null)
            {
                logger.LogError("Holiday with Id: " + holidayIn.Id + " doesn't exist");
                return NotFound(false);
            }
            bool updated = _holidayService.Update(holidayIn.Id, holidayIn);
            if (updated)
                logger.LogInformation("Given Holiday profile Updated successfully");
            else
                logger.LogError("Cannot update the Holiday profile: " + holidayIn.Id + " wrong format");

            return Ok(updated);
        }

        [HttpDelete("deleteHolidayById", Name = nameof(DeleteHolidayById))]
        public IActionResult DeleteHolidayById(string id)
        {
            logger.LogInformation("Deleting existed holiday profile: " + id);
            var holiday = _holidayService.GetById(id);

            if (holiday != null && _holidayService.RemoveById(holiday.Id))
            {
                logger.LogInformation("holiday profile has been deleted successfully " + holiday);
                return Ok(true);
            }
            logger.LogError("holiday not found in Database");
            return NotFound(false);
        }
    }
}