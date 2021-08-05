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
        public ActionResult<List<Exam>> GetAllExam() =>
             _examService.GetAll();

        [HttpGet("getExamById", Name = nameof(GetExamById))]
        public ActionResult<Exam> GetExamById(string id)
        {
            var exam = _examService.GetById(id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }

        [HttpPost("createExam", Name = nameof(CreateExam))]
        public ActionResult<Exam> CreateExam(Exam exam)
        {
            if (!(_examService.Create(exam)))
                 return NotFound();

            return CreatedAtRoute("getExamById", new { id = exam.Id }, exam);
        }

        [HttpPut("updateExam", Name = nameof(UpdateExam))]
        public IActionResult UpdateExam(Exam examIn)
        {
            logger.LogInformation("Updating existed exam: " + examIn.Id);
            if (examIn != null)
            {
                var course = _examService.GetById(examIn.Id);
                if (course != null)
                {
                    if (_examService.Update(examIn.Id, examIn))
                    {
                        logger.LogInformation("Given Course profile Updated successfully");
                        return Ok(true);
                    }
                    else
                        logger.LogError("Cannot update the Course profile: " + examIn.Id + " wrong format");
                }
                else
                    logger.LogError("Course with Id: " + examIn.Id + " doesn't exist");
            }
            else
                logger.LogError("CourseIn objest is null");
            return BadRequest(false);
        }
        [HttpGet("getExamByGroup", Name = nameof(GetExamByGroup))]
        public ActionResult<List<Exam>> GetExamByGroup(string groupNumber, string semester, string year, string testNo)
        {
            if (groupNumber != null)
            {
                var result = _examService.GetExamByGroup(groupNumber, semester, year, testNo);
                return result;
            }
            else
                return null;
        }


        [HttpDelete("deleteExamById", Name = nameof(DeleteExamById))]
        public ActionResult<bool> DeleteExamById(string id)
        {
            if(id != null)
            {
                var exam = _examService.GetById(id);
                if (exam != null && _examService.RemoveById(exam.Id))
                    return Ok(true);
            }
            return NotFound(false);
        }
    }
}
