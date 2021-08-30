using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of faculties service
    /// </summary>
    public interface IFacultyService
    {
        Faculty GetById(string facultyId);
        List<Faculty> GetAll();
        bool Create(Faculty faculty);
        bool Update(Faculty facultyIn);
        bool RemoveById(string id);
        int GetNumberOfFaculties();
        Faculty GetByFacultyName(string facultyName);


    }
}
