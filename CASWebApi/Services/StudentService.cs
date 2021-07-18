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
    public class StudentService : IStudentService
    {
        IDbSettings DbContext;

        public StudentService(IDbSettings settings)
        {
            DbContext = settings;
        }
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;
            
        //}

        public Student GetById(string studentId)
        {
            return DbContext.GetById<Student>("student", studentId);
            
        }

        public List<Student> GetAll()
        {
             return DbContext.GetAll<Student>("student");
        }
        public int GetNumberOfStudents()
        {
            return DbContext.GetCountOfDocuments<Student>("student");
        }
        public int GetNumberOfStudentsByClass(string groupNum)
        {
            return DbContext.GetCountOfDocumentsByFilter<Student>("student", "group", groupNum);
        }
        public bool Create(Student student)
        {
           bool res= DbContext.Insert<Student>("student", student);
            return res;
        }

        public bool Update(string id, Student studentIn)
        {
            return DbContext.Update<Student>("student", id, studentIn);
        }

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id)
        {
           return DbContext.RemoveById<Student>("student", id);     
        }
    }
}
