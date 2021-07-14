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
      
        public Course GetById(string courseId)
        {
            return DbContext.GetById<Course>("course", courseId);
        }

        public List<Course> GetAll()
        {
            return DbContext.GetAll<Course>("course");

        }
        public int GetNumberOfCourses()
        {
            return DbContext.GetCountOfDocuments<Course>("course");
        }

        public bool Create(Course course)
        {
            course.Id = ObjectId.GenerateNewId().ToString();
            bool res=DbContext.Insert<Course>("course", course);
            return res;
        }

        public bool Update(string id, Course courseIn)
        {
            return DbContext.Update<Course>("course", id, courseIn);
        }
          

       

        public bool RemoveById(string id)
        {
            //DbContext.GetById<Course>("course",id);
            bool res= DbContext.RemoveById<Course>("course", id);
            if (res)
            {
                DbContext.PullElement<Course>("faculty", "courses", id);
                DbContext.PullElement<Course>("group", "courses", id);
            }
            return res;
           
        }
    }
}
