using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
   public interface ITeacherService
    {
        Teacher GetById(string teacherId);
        List<Teacher> GetAll();
        bool Create(Teacher teacher);
        bool Update(string id, Teacher teacherIn);
        bool RemoveById(string id);
    }
}
