using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ILogger logger;
        IExamService _examService;
        public ExamController(IExamService examService, ILogger<ExamController> logger)
        {
            this.logger = logger;
            _examService = examService;
        }

        [HttpGet("getAllExam", Name = nameof(GetAllExam))]
        public ActionResult<List<Exam>> GetAllExam()
        {
            try
            {
                return _examService.GetAll();
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        [HttpGet("getExamById", Name = nameof(GetExamById))]
        public ActionResult<Exam> GetExamById(string id)
        {
            if(id == null)
                return BadRequest("given id was null");
            try
            {
                var exam = _examService.GetById(id);
                if (exam == null)
                {
                    return NotFound("examination with given id doesn't exists in db");
                }
                return exam;
            }
            catch (Exception e)
            {
                return BadRequest("No connection to database");
            }
        }

        [HttpPost("createExam", Name = nameof(CreateExam))]
        public ActionResult<Exam> CreateExam(Exam exam)
        {
            if(exam == null)
                return BadRequest("given id was null");
            try
            {
                if (!(_examService.Create(exam)))
                    return NotFound();

                return CreatedAtRoute("getExamById", new { id = exam.Id }, exam);
            }
            catch (Exception e)
            {
                return Conflict(e);
            }
        }

        [HttpPut("updateExam", Name = nameof(UpdateExam))]
        public IActionResult UpdateExam(Exam examIn)
        {
            logger.LogInformation("Updating existed exam: " + examIn.Id);
            if (examIn == null)
            {
                logger.LogError("CourseIn objest is null");
                return BadRequest("CourseIn objest is null");
            }
            try
            {
                var exam = _examService.GetById(examIn.Id);
                if (exam == null)
                {
                    logger.LogError("examination with Id: " + examIn.Id + " doesn't exist");
                    return NotFound("examination doesn't exists");
                }

                _examService.Update(examIn.Id, examIn);
                        logger.LogInformation("Given Course profile Updated successfully");
                        return Ok(true);                  
            }
            catch (Exception e)
            {
                logger.LogError("error catched: "+ e);
                return BadRequest("No connection to database");
            }
        }
        [HttpGet("getExamByGroup", Name = nameof(GetExamByGroup))]
        public ActionResult<List<Exam>> GetExamByGroup(string groupNumber, string semester, string year, string testNo)
        {
            if (groupNumber == null || semester == null || year == null || testNo == null)
                return BadRequest("one of the given parameters is null");
            try
            {
                var result = _examService.GetExamByGroup(groupNumber, semester, year, testNo);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError("error catched: " + e);
                return BadRequest("No connection to database");
            }

        }


        [HttpDelete("deleteExamById", Name = nameof(DeleteExamById))]
        public ActionResult<bool> DeleteExamById(string id)
        {
            if (id == null)
            {
                return BadRequest("given id is null");
            }
            try
            { 
            var exam = _examService.GetById(id);
            if (exam != null && _examService.RemoveById(exam.Id))
                 return Ok(true);
                return NotFound("exam with given id doesn't exists");
            }
            catch (Exception e)
            {
                return BadRequest("No Connection to database");
            }

        }
    }
}
