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

        /// <summary>
        /// get faculty object by given id
        /// </summary>
        /// <param name="facultyId">id of the object that we need</param>
        /// <returns>found faculty object</returns>
       
        public Faculty GetById(string facultyId)
        {
            return DbContext.GetById<Faculty>("faculty", facultyId);
        }

        /// <summary>
        /// get all faculties from db
        /// </summary>
        /// <returns>list of faculties</returns>
        public List<Faculty> GetAll()
        {
            return DbContext.GetAll<Faculty>("faculty");

        }

        /// <summary>
        /// get total number of faculties in db 
        /// </summary>
        /// <returns>integer number of faculties</returns>
        public int GetNumberOfFaculties()
        {
            return DbContext.GetCountOfDocuments<Faculty>("faculty");
        }

        /// <summary>
        /// add new faculty object to db
        /// </summary>
        /// <param name="faculty">faculty object to add</param>
        /// <returns>true if added successfully</returns>
        public bool Create(Faculty faculty)
        {
            faculty.Id = ObjectId.GenerateNewId().ToString();
            faculty.Status = true;
            bool res=DbContext.Insert<Faculty>("faculty", faculty);
            return res;
        }

        /// <summary>
        /// edit existing faculty
        /// </summary>
        /// <param name="id">id of the faculty to edit</param>
        /// <param name="facultyIn">edited faculty's object to replace with the old one</param>
        /// <returns></returns>
        public bool Update(string id, Faculty facultyIn) =>
          DbContext.Update<Faculty>("faculty", id, facultyIn);


        /// <summary>
        /// remove faculty by given id
        /// </summary>
        /// <param name="id">id of the faculty to remove</param>
        /// <returns></returns>
        public bool RemoveById(string id)
        {
            bool res=DbContext.RemoveById<Faculty>("faculty", id);
           if(res)
             DbContext.RemoveField<Faculty>("group","fac_id",id);
            return res;
            
        }
        }
    
    }
