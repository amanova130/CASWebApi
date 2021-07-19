using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
   public interface ITeacherService
    {
        /// <summary>
        /// an interface that contains abstract methods and properties of teachers service
        /// </summary>
        Teacher GetById(string teacherId);
        List<Teacher> GetAll();
        bool Create(Teacher teacher);
        bool Update(string id, Teacher teacherIn);
        bool RemoveById(string id);
        int GetNumberOfTeachers();
        List<Teacher> GetTeachersByCourseName(string courseName);


    }
}
