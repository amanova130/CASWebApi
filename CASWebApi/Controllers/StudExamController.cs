using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
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

        /// <summary>
        /// get all studeExam objects by examId
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        [HttpGet("getGradesByExamId", Name = nameof(GetGradesByExamId))]
        public ActionResult<List<StudExam>> GetGradesByExamId(string examId)
        {
            logger.LogInformation("Getting all studExam from studExamController");
            if(examId == null)
            {
                logger.LogError("examId is null");
                return BadRequest("Incorrect format of examId param");
            }
            try
            {
                var list = _studExamService.GetStudentsDetailByExamId(examId);
                    logger.LogInformation("Fetched All studExam data");
                    return Ok(list);           
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
        /// <summary>
        /// get all studExam objects by studentId and year
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("getGradesByStudentId", Name = nameof(GetGradesByStudentId))]
        public ActionResult<List<StudExam>> GetGradesByStudentId(string studentId, string year)
        {
            logger.LogInformation("Getting all studExam from studExamController");
            if (studentId == null || year == null)
            {
                logger.LogError("studentIdor year is null");
                return BadRequest("Incorrect format of examId param");
            }
            try
            {
                var list = _studExamService.GetGradesByStudentIdAndYear(studentId, year);
                    return Ok(list);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
        /// <summary>
        /// function to calculate average to student for every course by year
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="year"></param>
        /// <param name="groupNumber"></param>
        /// <returns></returns>
        [HttpGet("getAvgOfGradesByStudentId", Name = nameof(GetAvgOfGradesByStudentId))]
        public ActionResult<List<GradeDetails>> GetAvgOfGradesByStudentId(string studentId, string year, string groupNumber)
        {
            logger.LogInformation("Getting all Course from StudExamController");
            if (studentId == null || year == null || groupNumber == null)
            {
                logger.LogError("one of parameters is null");
                return BadRequest("Incorrect format of parameters");
            }
            try
            {
                var list = _studExamService.GetGradesAverage(studentId.Trim(), year.Trim(), groupNumber.Trim());        
                    logger.LogInformation("Fetched All course data");
                    return Ok(list);             
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }

        }
        /// <summary>
        ///         
        /// get list of grade details of student by years and course
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="year"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpGet("getSemiGradesByStudentId", Name = nameof(GetSemiGradesByStudentId))]
        public ActionResult<List<GradeDetails>> GetSemiGradesByStudentId(string studentId, string year, string course)
        {
            logger.LogInformation("Getting all Course from StudExamController");
            if (studentId == null || year == null || course == null)
            {
                logger.LogError("one of parameters is null");
                return BadRequest("Incorrect format of parameters");
            }
            try
            {
                var list = _studExamService.GetSemiGradesByStudentIdAndYear(studentId, year, course);            
                    logger.LogInformation("Fetched All course data");
                    return Ok(list);           
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }

        }

        /// <summary>
        /// Update grade for student
        /// </summary>
        /// <param name="gradeIn">StudExam document to update </param>
        /// <returns>Updated grade profile</returns>
        [HttpPut("updateGrade", Name = nameof(UpdateGrade))]
        public IActionResult UpdateGrade(StudExam gradeIn)
        {
            if (gradeIn == null)
            {
                logger.LogError("gradeIn is null");
                return BadRequest("Incorrect format of gradeIn");
            }
            try
            {
                bool res = false;
                gradeIn.Status = true;

                var student = _studExamService.GetById(gradeIn.Id);
                if (student == null)
                {
                    return NotFound("object studExam with given id not found");
                }
                res = _studExamService.Update(gradeIn);
                return Ok(res);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot get access to db");
                return BadRequest("No connection to database");
            }
        }
    }
}
