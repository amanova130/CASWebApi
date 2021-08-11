using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudExamController : ControllerBase
    {
        //Logger to create streammer of logs 
        private readonly ILogger logger;
        IStudExamService _studExamService;

        public StudExamController(IStudExamService studExamService, ILogger<StudExamController> logger)
        {
            this.logger = logger;
            _studExamService = studExamService;
        }

        [HttpGet("getGradesByExamId", Name = nameof(GetGradesByExamId))]
        public ActionResult<List<StudExam>> GetGradesByExamId(string examId)
        {
            logger.LogInformation("Getting all Course from CourseController");
            var list = _studExamService.GetStudentsDetailByExamId(examId);
            if (list != null)
            {
                logger.LogInformation("Fetched All course data");
                return Ok(list);
            }
            else
            {
                logger.LogError("Cannot get access to course collection in Db");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
