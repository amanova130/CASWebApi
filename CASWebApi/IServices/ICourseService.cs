using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of courses service
    /// </summary>
    public interface ICourseService
    {
        Course GetById(string courseId);
        List<Course> GetAll();
        List<string> GetCoursesByFaculty(string facultyName);
        bool Create(Course course);
        bool Update(string id, Course courseIn);
        bool RemoveById(string id);
        int GetNumberOfCourses();

    }
}
