using CASWebApi.IServices;
using CASWebApi.Models;
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

        public TeacherService(IDbSettings settings)
        {
            DbContext = settings;
        }
 
        /// <summary>
        /// get teacher by id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>teacher's object with given id</returns>
        public Teacher GetById(string teacherId)
        {
            return DbContext.GetById<Teacher>("teachers", teacherId);
        }

        /// <summary>
        /// get list of teachers by given courseName
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns>list of teachers</returns>
        public List<Teacher> GetTeachersByCourseName(string courseName)
        {
            return DbContext.GetListByFilter<Teacher>("teachers", "teachesCourses", courseName);
        }


        /// <summary>
        /// get all teachers from db
        /// </summary>
        /// <returns>list of teachers</returns>
        public List<Teacher> GetAll()
        {
            return DbContext.GetAll<Teacher>("teachers");

        }

        /// <summary>
        /// get total number of teachers in db
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfTeachers()
        {
            return DbContext.GetCountOfDocuments<Teacher>("teachers");
        }

        /// <summary>
        /// add new teacher object to db
        /// </summary>
        /// <param name="teacher">teacher object to add</param>
        /// <returns>true if added</returns>
        public bool Create(Teacher teacher)
        {
            teacher.Status = true;
            bool res = DbContext.Insert<Teacher>("teachers", teacher);
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

            return DbContext.Update<Teacher>("teachers", id, teacherIn);
        }

        /// <summary>
        /// remove teacher by given id
        /// </summary>
        /// <param name="id">id of the teacher to remove</param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            var deleted = DbContext.RemoveById<Teacher>("teachers", id);
            return deleted;
        }
            
    }
}
