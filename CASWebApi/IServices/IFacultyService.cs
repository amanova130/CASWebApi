using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IFacultyService
    {
        Faculty GetById(string facultyId);
        List<Faculty> GetAll();
        bool Create(Faculty faculty);
        bool Update(string id, Faculty facultyIn);
        bool RemoveById(string id);
        int GetNumberOfFaculties();

    }
}
