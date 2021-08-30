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
        IFacultyService _facultyService;

        public CourseService(IDbSettings settings, ILogger<CourseService> logger,IFacultyService facultyService)
        {
            this.logger = logger;
            DbContext = settings;
            _facultyService = facultyService;
        }
      /// <summary>
      /// get course object by id
      /// </summary>
      /// <param name="courseId"></param>
      /// <returns>found course object</returns>
        public Course GetById(string courseId)
        {
            logger.LogInformation("CourseService:Getting course by id");
            try { 
            Course course= DbContext.GetById<Course>("course", courseId);
            if(course == null)
                logger.LogError("CourseService:Cannot get a course with a courseId: "+courseId);
            
            else
                logger.LogInformation("CourseService:Fetched course data by id "); 
                return course;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// get all courses from db
        /// </summary>
        /// <returns></returns>
        public List<Course> GetAll()
        {
            logger.LogInformation("CourseService:Getting all courses");
            try
            { 
                var courses= DbContext.GetAll<Course>("course");
                if (courses == null)
                    logger.LogError("CourseService:Cannot get access to courses collection in Db");
                else
                    logger.LogInformation("CourseService:fetched All course collection data");
                return courses;
            }
            catch (Exception e)
            {
                    throw e;
            }
        }

        /// <summary>
        /// get total number of courses in db
        /// </summary>
        /// <returns>number of courses</returns>
        public int GetNumberOfCourses()
        {
            logger.LogInformation("CourseService:Getting count of course collections");
            try
            {
                int res = DbContext.GetCountOfDocuments<Course>("course");
                logger.LogInformation("CourseService:fetched number of courses");
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// function to get courses by facultyName
        /// </summary>
        /// <param name="facultyName"></param>
        /// <returns>array of courses names</returns>
        public string[] GetCoursesByFaculty(string facultyName)
        {
            logger.LogInformation("CourseService:Getting all courses by faculty");
            try
            {
                var faculty = _facultyService.GetByFacultyName(facultyName);
                string[] courses;
                courses = faculty.Courses;
                if (courses == null)
                    logger.LogError("CourseService:Cannot get access to courses collection in Db");
                else
                    logger.LogInformation("CourseService:fetched All course collection data by facultyName");
                return courses;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// get courses objects by their names
        /// </summary>
        /// <param name="courses">courses names to find</param>
        /// <returns>list of courses</returns>
        public List<Course> GetCoursesByCourseNames(string[] courses)
        {
            try
            {
                List<Course> courseList = new List<Course>();
                logger.LogInformation("CourseService:Getting all courses by course names");
                for (int i = 0; i < courses.Length; i++)
                {
                    var course = DbContext.GetDocumentByFilter<Course>("course", "course_name", courses[i]);
                    if (course != null)
                    {
                        courseList.Add(course);
                    }
                }
                return courseList;
            }
            catch(Exception e)
            {
                throw e;
            }
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
            try
            {
                bool res = DbContext.Insert<Course>("course", course);
                if (res)
                    logger.LogInformation("CourseService:A new course profile added successfully :" + course);
                else
                    logger.LogError("CourseService:Cannot create a course, duplicated id or wrong format");

                return res;
            }
            catch (Exception e)
            {
                if (e is MongoWriteException)
                    throw new Exception(String.Format("User with Id: {0} already exists", course.Id), e);
                throw e;
            }
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
            try
            {
                bool res = DbContext.Update<Course>("course", id, courseIn);
                if (!res)
                    logger.LogError("CourseService:Course with Id: " + courseIn.Id + " doesn't exist");
                else
                    logger.LogInformation("CourseService:Course with Id" + courseIn.Id + "has been updated successfully");
                return res;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
          

       
        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("CourseService:deleting a course profile with id : " + id);
            var course = GetById(id);
            try
            {
                bool res = DbContext.RemoveById<Course>("course", id);
                if (res)
                {
                    DbContext.PullElement<Course>("faculty", "courses", course.CourseName);
                    DbContext.PullElement<Course>("group", "courses", course.CourseName);
                    logger.LogInformation("CourseService:a course profile with id : " + id + "has been deleted successfully");
                }
                else
                {
                    logger.LogError("CourseService:Course with Id: " + id + " doesn't exist");
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
