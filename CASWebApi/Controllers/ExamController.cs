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

        [HttpGet]
        public ActionResult<List<Exam>> Get() =>
             _examService.GetAll();

        [HttpGet("{id:length(24)}", Name = "GetExam")]
        public ActionResult<Exam> Get(string id)
        {
            var exam = _examService.GetById(id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }

        [HttpPost]
        public ActionResult<Exam> Create(Exam exam)
        {
            _examService.Create(exam);

            return CreatedAtRoute("GetExam", new { id = exam.Id }, exam);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Exam examIn)
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

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var exam = _examService.GetById(id);

            if (exam == null)
            {
                return NotFound();
            }

            _examService.RemoveById(exam.Id);

            return NoContent();
        }
    }
}
