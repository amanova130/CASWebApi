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
    public class FacultyService : IFacultyService
    {
        IDbSettings DbContext;

        public FacultyService(IDbSettings settings)
        {
            DbContext = settings;
        }
       
        public Faculty GetById(string facultyId)
        {
            return DbContext.GetById<Faculty>("faculty", facultyId);
        }

        public List<Faculty> GetAll()
        {
            return DbContext.GetAll<Faculty>("faculty");

        }
        public int GetNumberOfFaculties()
        {
            return DbContext.GetCountOfDocuments<Faculty>("faculty");
        }
        public bool Create(Faculty faculty)
        {
            faculty.Id = ObjectId.GenerateNewId().ToString();
            faculty.Status = true;
            bool res=DbContext.Insert<Faculty>("faculty", faculty);
            return res;
        }

        public bool Update(string id, Faculty facultyIn) =>
          DbContext.Update<Faculty>("faculty", id, facultyIn);

        public bool RemoveById(string id)
        {
            bool res=DbContext.RemoveById<Faculty>("faculty", id);
           if(res)
             DbContext.RemoveField<Faculty>("group","fac_id",id);
            return res;
            
        }
        }
    }
