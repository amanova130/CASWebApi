using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        IExamService _examService;
        public ExamController(IExamService examService)
        {
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
        public IActionResult UpdateExam(string id, Exam examIn)
        {
            var exam = _examService.GetById(id);

            if (exam == null)
            {
                return NotFound();
            }
            examIn.Id = id;

            _examService.Update(id, examIn);

            return NoContent();
        }

        [HttpDelete("deleteExamById", Name = nameof(DeleteExamById))]
        public IActionResult DeleteExamById(string id)
        {
            var exam = _examService.GetById(id);

            if (exam != null && _examService.RemoveById(exam.Id))
                return NoContent();
            return NotFound();
        }
    }
}
