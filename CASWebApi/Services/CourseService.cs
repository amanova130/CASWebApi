using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        IDbSettings DbContext;

        public CourseService(IDbSettings settings, ILogger<CourseService> logger)
        {
            this.logger = logger;
            DbContext = settings;
        }
      /// <summary>
      /// get course object by id
      /// </summary>
      /// <param name="courseId"></param>
      /// <returns>found course object</returns>
        public Course GetById(string courseId)
        {
            logger.LogInformation("CourseService:Getting course by id");
            Course course= DbContext.GetById<Course>("course", courseId);
            if(course == null)
                logger.LogError("CourseService:Cannot get a course with a courseId: "+courseId);
            
            else
                logger.LogInformation("CourseService:Fetched course data by id "); 
            return course;
        }
        /// <summary>
        /// get all courses from db
        /// </summary>
        /// <returns></returns>
        public List<Course> GetAll()
        {
            logger.LogInformation("CourseService:Getting all courses");
            var courses= DbContext.GetAll<Course>("course");
            if (courses == null)
                logger.LogError("CourseService:Cannot get access to courses collection in Db");
            else
                logger.LogInformation("CourseService:fetched All course collection data");
            return courses;

        }

        /// <summary>
        /// get total number of courses in db
        /// </summary>
        /// <returns>number of courses</returns>
        public int GetNumberOfCourses()
        {
            logger.LogInformation("CourseService:Getting count of course collections");
            int res= DbContext.GetCountOfDocuments<Course>("course");
                logger.LogInformation("CourseService:fetched number of courses");
            
            return res;
        }


        /// <summary>
        /// create new course object in db
        /// </summary>
        /// <param name="course"></param>
        /// <returns>true if successed</returns>
        public bool Create(Course course)
        {
            logger.LogInformation("CourseService:creating a new course profile : "+ course);

            course.Id = ObjectId.GenerateNewId().ToString();
            bool res=DbContext.Insert<Course>("course", course);
            if(res)
                logger.LogInformation("CourseService:A new course profile added successfully :" + course);
            else
                logger.LogError("CourseService:Cannot create a course, duplicated id or wrong format");

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
            logger.LogInformation("CourseService:updating an existing course profile with id : " + courseIn.Id);

            bool res = DbContext.Update<Course>("course", id, courseIn);
            if(!res)
                logger.LogError("CourseService:Course with Id: " + courseIn.Id + " doesn't exist");
            else
                logger.LogInformation("CourseService:Course with Id" + courseIn.Id + "has been updated successfully");

            return res;
        }
          

       
        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("CourseService:deleting a course profile with id : " + id);

            bool res = DbContext.RemoveById<Course>("course", id);
            if (res)
            {
                DbContext.PullElement<Course>("faculty", "courses", id);
                DbContext.PullElement<Course>("group", "courses", id);
                logger.LogInformation("CourseService:a course profile with id : " + id + "has been deleted successfully");

            }
            else
            {
                logger.LogError("CourseService:Course with Id: " + id + " doesn't exist");

            }
            return res;
           
        }
    }
}
