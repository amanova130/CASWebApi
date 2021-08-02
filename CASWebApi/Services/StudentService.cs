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
    public class StudentService : IStudentService
    {
        private readonly ILogger logger;

        IDbSettings DbContext;

        public StudentService(IDbSettings settings, ILogger<FacultyService> logger)
        {
            DbContext = settings;
            this.logger = logger;

        }

        /// <summary>
        /// get student by given id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns> student object by given id</returns>
        public Student GetById(string studentId)
        {
            logger.LogInformation("StudentService:Getting student by id");

            var student =   DbContext.GetById<Student>("student", studentId);
            if (student == null)
                logger.LogError("StudentService:Cannot get a student with a studentId: " + studentId);
            else
                logger.LogInformation("StudentService:Fetched student data by id ");
            return student;
              
        }

        /// <summary>
        /// get list of all students in db
        /// </summary>
        /// <returns>list of student objects</returns>
        public List<Student> GetAll()
        {
            logger.LogInformation("StudentService:Getting all students");
            var students = DbContext.GetAll<Student>("student");
            if (students == null)
                logger.LogError("StudentService:Cannot get access to students collection in Db");
            else
                logger.LogInformation("StudentService:fetched All students collection data");
            return students;

        }

        public List<Student> GetAllStudentsByGroup(string groupName)
        {
            logger.LogInformation("StudentService:Getting all students by group");
            var students = DbContext.GetListByFilter<Student>("student", "group", groupName);
            if (students == null)
                logger.LogError("StudentService:Cannot get access to students collection in Db");
            else
                logger.LogInformation("StudentService:fetched All students collection data by groupName");
            return students;

        }

        /// <summary>
        /// get total number of students in db
        /// </summary>
        /// <returns>number of students</returns>
        public int GetNumberOfStudents()
        {
            logger.LogInformation("StudentService:Getting count of all student collections");
            int res = DbContext.GetCountOfDocuments<Student>("student");
            logger.LogInformation("StudentService:fetched number of students");
            return res;
        }

        /// <summary>
        /// get total number of students by specific class
        /// </summary>
        /// <param name="groupNum"></param>
        /// <returns>number of students in this class</returns>
        public int GetNumberOfStudentsByClass(string groupNum)
        {
            logger.LogInformation("StudentService:Getting count of students by class");
            int res = DbContext.GetCountOfDocumentsByFilter<Student>("student", "group", groupNum);
            logger.LogInformation("StudentService:fetched number of students in class");
            return res;
        }

        /// <summary>
        /// add new student object to db 
        /// </summary>
        /// <param name="student">student object to add</param>
        /// <returns>true if added</returns>
        public bool Create(Student student)
        {
            logger.LogInformation("StudentService:creating a new student profile : " + student);

            bool res = DbContext.Insert<Student>("student", student);
            if (res)
                logger.LogInformation("StudentService:A new student profile added successfully :" + student);
            else
                logger.LogError("StudentService:Cannot create a student, duplicated id or wrong format");
            return res;
        }

        /// <summary>
        /// add new student object to db 
        /// </summary>
        /// <param name="student">student object to add</param>
        /// <returns>true if added</returns>
        public bool InsertManyStudents(List<Student> students)
        {
            logger.LogInformation("StudentService:insert a list of students : ");

            bool res = DbContext.InsertMany<Student>("student", students);
            if (res)
                logger.LogInformation("StudentService:A new list of students added successfully :");
            else
                logger.LogError("StudentService:Cannot add a list of students, check format");
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
            logger.LogInformation("StudentService:updating an existing student profile with id : " + studentIn.Id);

            bool res = DbContext.Update<Student>("student", id, studentIn);
            if (!res)
                logger.LogError("StudentService:student with Id: " + studentIn.Id + " doesn't exist");
            else
                logger.LogInformation("StudentService:student with Id" + studentIn.Id + "has been updated successfully");

            return res;
        }


        /// <summary>
        /// remove student object with the given id from db
        /// </summary>
        /// <param name="id">id of the student to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("StudentService:deleting a student profile with id : " + id);

            bool res = DbContext.RemoveById<Student>("student", id);
            if (res)
            {
                
                logger.LogInformation("StudentService:a student profile with id : " + id + "has been deleted successfully");
            }
            {
                logger.LogError("StudentService:student with Id: " + id + " doesn't exist");

            }
            return res;
        }
    }
}
