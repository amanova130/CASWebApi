using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class TeacherService : ITeacherService
    {
        IDbSettings DbContext;
        private readonly ILogger logger;


        public TeacherService(IDbSettings settings, ILogger<FacultyService> logger)
        {
            DbContext = settings;
            this.logger = logger;
        }
 
        /// <summary>
        /// get teacher by id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>teacher's object with given id</returns>
        public Teacher GetById(string teacherId)
        {
            logger.LogInformation("TeacherService:Getting teacher by id");

            var teacher = DbContext.GetById<Teacher>("teachers", teacherId);
            if (teacher == null)
                logger.LogError("TeacherService:Cannot get a teacher with a Id: " + teacherId);
            else
                logger.LogInformation("TeacherService:Fetched teacher data by id ");
            return teacher;

           
        }

        /// <summary>
        /// get list of teachers by given courseName
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns>list of teachers</returns>
        public List<Teacher> GetTeachersByCourseName(string courseName)
        {
            logger.LogInformation("TeacherService:Getting list of teacher by courseName");

            var teachers = DbContext.GetListByFilter<Teacher>("teachers", "teachesCourses", courseName);
            if (teachers == null)
                logger.LogError("TeacherService:Cannot get a list of  teachers with a courseName: " + courseName);
            else
                logger.LogInformation("TeacherService:Fetched list of teachers by courseName ");
            return teachers;
            
        }


        /// <summary>
        /// get all teachers from db
        /// </summary>
        /// <returns>list of teachers</returns>
        public List<Teacher> GetAll()
        {
            logger.LogInformation("TeacherService:Getting all Teachers");
            var teachers =DbContext.GetAll<Teacher>("teachers");

            if (teachers == null)
                logger.LogError("TeacherService:Cannot get access to teachers collection in Db");
            else
                logger.LogInformation("TeacherService:fetched All teachers collection data");
            return teachers;


        }

        /// <summary>
        /// get total number of teachers in db
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfTeachers()
        {
            logger.LogInformation("TeacherService:Getting count of teacher collections");
            int res = DbContext.GetCountOfDocuments<Teacher>("teachers");
            logger.LogInformation("TeacherService:fetched number of teachers");
            return res;
        }

        /// <summary>
        /// add new teacher object to db
        /// </summary>
        /// <param name="teacher">teacher object to add</param>
        /// <returns>true if added</returns>
        public bool Create(Teacher teacher)
        {
            teacher.Status = true;
            if (teacher.Image == null || teacher.Image == "")
                teacher.Image = "Resources/Images/noPhoto.png";
            bool res =  DbContext.Insert<Teacher>("teachers", teacher);
            if (res)
                logger.LogInformation("TeacherService:A new teacher profile added successfully :" + teacher);
            else
                logger.LogError("TeacherService:Cannot create a teachers, duplicated id or wrong format");
            return res;
        }

        /// <summary>
        /// edit an existing teacher by changing it to a new teacher object with the same id
        /// </summary>
        /// <param name="id">id of the teacher to edit</param>
        /// <param name="teacherIn">new teacher object</param>
        /// <returns></returns>
        public bool Update(string id, Teacher teacherIn)
        {
            logger.LogInformation("TeacherService:updating an existing teacher profile with id : " + teacherIn.Id);
            bool res = DbContext.Update<Teacher>("teachers", id, teacherIn);
            if (!res)
                logger.LogError("TeacherService:teacher with Id: " + teacherIn.Id + " doesn't exist");
            else
                logger.LogInformation("TeacherService:teacher with Id" + teacherIn.Id + "has been updated successfully");
            return res;
        }

        /// <summary>
        /// remove teacher by given id
        /// </summary>
        /// <param name="id">id of the teacher to remove</param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            var deleted = DbContext.RemoveById<Teacher>("teachers", id);
            if (deleted)
            {
                logger.LogInformation("TeacherService:a teacher profile with id : " + id + "has been deleted successfully");
            }
            {
                logger.LogError("TeacherService:teacher with Id: " + id + " doesn't exist");
            }
            return deleted;
        }
            
    }
}
