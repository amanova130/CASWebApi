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
      /// <summary>
      /// get course object by id
      /// </summary>
      /// <param name="courseId"></param>
      /// <returns>found course object</returns>
        public Course GetById(string courseId)
        {
            return DbContext.GetById<Course>("course", courseId);
        }
        /// <summary>
        /// get all courses from db
        /// </summary>
        /// <returns></returns>
        public List<Course> GetAll()
        {
            return DbContext.GetAll<Course>("course");

        }

        /// <summary>
        /// get total number of courses in db
        /// </summary>
        /// <returns>number of courses</returns>
        public int GetNumberOfCourses()
        {
            return DbContext.GetCountOfDocuments<Course>("course");
        }


        /// <summary>
        /// create new course object in db
        /// </summary>
        /// <param name="course"></param>
        /// <returns>true if successed</returns>
        public bool Create(Course course)
        {
            course.Id = ObjectId.GenerateNewId().ToString();
            bool res=DbContext.Insert<Course>("course", course);
            return res;
        }

        /// <summary>
        /// edit an existing course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseIn"></param>
        /// <returns>true if successed</returns>
        public bool Update(string id, Course courseIn)
        {
            return DbContext.Update<Course>("course", id, courseIn);
        }
          

       
        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
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
