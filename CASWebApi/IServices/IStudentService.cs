using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of students service
    /// </summary>
    public interface IStudentService
    {
        Student GetById(string studentId);
        List<Student> GetAll();
        bool Create(Student student);
         bool InsertManyStudents(List<Student> students);
        bool Update(string id, Student studentIn);
        bool RemoveById(string id);
        List<Student> GetAllStudentsByFaculties(string[] facultyNames);
        List<Student> GetAllStudentsByGroups(string[] groupNames);

        int GetNumberOfStudents();
        int GetNumberOfStudentsByClass(string groupNum);
        List<Student> GetAllStudentsByGroup(string groupName);


    }
}
