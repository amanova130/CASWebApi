using CASWebApi.IServices;
using CASWebApi.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ILogger logger;
        IReportService _reportService;
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            this.logger = logger;
            _reportService = reportService;
        }
        [HttpGet("getAvgByGroup", Name = nameof(GetAvgByGroup))]
        public ActionResult<List<Average>> GetAvgByGroup(string groupName, string year)
        {
            var avgList = _reportService.GetAvgByGroup(groupName, year);
            return Ok(avgList);
        }
        [HttpGet("getAvgOFAllTeachers", Name = nameof(GetAvgOFAllTeachers))]
        public ActionResult<List<Average>> GetAvgOFAllTeachers(string year)
        {
            var avgList = _reportService.GetAvgOfAllTeachers(year);
            return Ok(avgList);
        }

    }
}
