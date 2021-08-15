using CASWebApi.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger logger;
        IReportService _reportService;
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            this.logger = logger;
            _reportService = reportService;
        }


        [HttpGet("getAvgOfFacultiesByCourse", Name = nameof(GetAvgOfFacultiesByCourse))]
        public ActionResult<List<object>> GetAvgOfFacultiesByCourse(string courseName, string facId)
        {
            logger.LogInformation("Getting Faculty by Id");
            if (courseName != null && facId != null)
            {
                var faculties = _reportService.GetAvgOfFacultiesByCourse(courseName, facId);
                if (faculties != null)
                {
                    return Ok(faculties);
                }
                else
                {
                    logger.LogError("Cannot get access to faculty collection in Db");
                }
            }
            else
                logger.LogError("Course Id is null or empty string");
            return BadRequest(null);
        }
    }
}
