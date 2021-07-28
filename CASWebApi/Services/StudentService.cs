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

        /// <summary>
        /// get student by given id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns> student object by given id</returns>
        public Student GetById(string studentId)
        {
            return DbContext.GetById<Student>("student", studentId);
            
        }

        /// <summary>
        /// get list of all students in db
        /// </summary>
        /// <returns>list of student objects</returns>
        public List<Student> GetAll()
        {
             return DbContext.GetAll<Student>("student");
        }

        public List<Student> GetAllStudentsByGroup(string groupName)
        {
            return DbContext.GetListByFilter<Student>("student", "group",groupName);

        }

        /// <summary>
        /// get total number of students in db
        /// </summary>
        /// <returns>number of students</returns>
        public int GetNumberOfStudents()
        {
            return DbContext.GetCountOfDocuments<Student>("student");
        }

        /// <summary>
        /// get total number of students by specific class
        /// </summary>
        /// <param name="groupNum"></param>
        /// <returns>number of students in this class</returns>
        public int GetNumberOfStudentsByClass(string groupNum)
        {
            return DbContext.GetCountOfDocumentsByFilter<Student>("student", "group", groupNum);
        }

        /// <summary>
        /// add new student object to db 
        /// </summary>
        /// <param name="student">student object to add</param>
        /// <returns>true if added</returns>
        public bool Create(Student student)
        {
           bool res= DbContext.Insert<Student>("student", student);
            return res;
        }

        /// <summary>
        /// edit an existing student by changing it to a new student object with the same id
        /// </summary>
        /// <param name="id">id of the student to edit</param>
        /// <param name="studentIn"></param>
        /// <returns>true is updated</returns>
        public bool Update(string id, Student studentIn)
        {
            return DbContext.Update<Student>("student", id, studentIn);
        }


        /// <summary>
        /// remove student object with the given id from db
        /// </summary>
        /// <param name="id">id of the student to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
           return DbContext.RemoveById<Student>("student", id);     
        }
    }
}
