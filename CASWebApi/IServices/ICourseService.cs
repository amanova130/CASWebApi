using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
   public interface ICourseService
    {
        Course GetById(string courseId);
        List<Course> GetAll();
        bool Create(Course course);
        bool Update(string id, Course courseIn);
        bool RemoveById(string id);
    }
}
