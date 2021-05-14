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
    public class CourseService : ICourseService
    {
        IDbSettings DbContext;

        public CourseService(IDbSettings settings)
        {
            DbContext = settings;
        }
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public Course GetById(string courseId)
        {
            return DbContext.GetById<Course>("course", courseId);
        }

        public List<Course> GetAll()
        {
            return DbContext.GetAll<Course>("course");

        }

        public Course Create(Course course)
        {
            course.Id = ObjectId.GenerateNewId().ToString();
            DbContext.Insert<Course>("course", course);
            return course;
        }

        public void Update(string id, Course courseIn) =>
          DbContext.Update<Course>("course", id, courseIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Course>("course", id);
    }
}
