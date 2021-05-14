using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class ExamService : IExamService
    {
        IDbSettings DbContext;

        public ExamService(IDbSettings settings)
        {
            DbContext = settings;
        }
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public Exam GetById(string examId)
        {
            return DbContext.GetById<Exam>("examination", examId);
        }

        public List<Exam> GetAll()
        {
            return DbContext.GetAll<Exam>("examination");

        }

        public Exam Create(Exam exam)
        {
           exam.Id = ObjectId.GenerateNewId().ToString();
            DbContext.Insert<Exam>("examination", exam);
            return exam;
        }

        public void Update(string id, Exam examIn) =>
          DbContext.Update<Exam>("examination", id, examIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Exam>("examination", id);
    }
}
