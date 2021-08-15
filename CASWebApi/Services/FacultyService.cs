using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        IDbSettings DbContext;

        public FacultyService(IDbSettings settings, ILogger<FacultyService> logger)
        {
            DbContext = settings;
            this.logger = logger;

        }

        /// <summary>
        /// get faculty object by given id
        /// </summary>
        /// <param name="facultyId">id of the object that we need</param>
        /// <returns>found faculty object</returns>

        public Faculty GetById(string facultyId)
        {
            logger.LogInformation("FacultyService:Getting faculty by id");

            var faculty = DbContext.GetById<Faculty>("faculty", facultyId);
            if(faculty == null)
                logger.LogError("FacultyService:Cannot get a faculty with a facId: " + facultyId);
            else
                logger.LogInformation("FacultyService:Fetched faculty data by id ");
            return faculty;



        }

        /// <summary>
        /// get all faculties from db
        /// </summary>
        /// <returns>list of faculties</returns>
        public List<Faculty> GetAll()
        {
            logger.LogInformation("FacultyService:Getting all faculties");
            var faculties= DbContext.GetAll<Faculty>("faculty");
            if (faculties == null)
                logger.LogError("FacultyService:Cannot get access to faculties collection in Db");
            else
                logger.LogInformation("FacultyService:fetched All faculties collection data");
            return faculties;

        }

        /// <summary>
        /// get total number of faculties in db 
        /// </summary>
        /// <returns>integer number of faculties</returns>
        public int GetNumberOfFaculties()
        {
            logger.LogInformation("FacultyService:Getting count of faculty collections");
            int res = DbContext.GetCountOfDocuments<Faculty>("faculty");
            logger.LogInformation("FacultyService:fetched number of faculties");
            return res;
           
        }

        /// <summary>
        /// add new faculty object to db
        /// </summary>
        /// <param name="faculty">faculty object to add</param>
        /// <returns>true if added successfully</returns>
        public bool Create(Faculty faculty)
        {
            logger.LogInformation("FacultyService:creating a new faculty profile : " + faculty);

            faculty.Id = ObjectId.GenerateNewId().ToString();
            faculty.Status = true;
            bool res=DbContext.Insert<Faculty>("faculty", faculty);
            if (res)
                logger.LogInformation("FacultyService:A new faculty profile added successfully :" + faculty);
            else
                logger.LogError("FacultyService:Cannot create a faculty, duplicated id or wrong format");
            return res;
        }

        /// <summary>
        /// edit existing faculty
        /// </summary>
        /// <param name="id">id of the faculty to edit</param>
        /// <param name="facultyIn">edited faculty's object to replace with the old one</param>
        /// <returns></returns>
        public bool Update(string id, Faculty facultyIn)
        {
            logger.LogInformation("facultyService:updating an existing faculty profile with id : " + facultyIn.Id);

            bool res = DbContext.Update<Faculty>("faculty", id, facultyIn);
            if (!res)
                logger.LogError("facultyService:faculty with Id: " + facultyIn.Id + " doesn't exist");
            else
                logger.LogInformation("facultyService:faculty with Id" + facultyIn.Id + "has been updated successfully");

            return res;
     
        }

        /// <summary>
        /// remove faculty by given id
        /// </summary>
        /// <param name="id">id of the faculty to remove</param>
        /// <returns></returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("facultyService:deleting a faculty profile with id : " + id);

            bool res =DbContext.RemoveById<Faculty>("faculty", id);
            if (res)
            {
                DbContext.RemoveField<Faculty>("group", "fac_id", id);
                logger.LogInformation("facultyService:a faculty profile with id : " + id + "has been deleted successfully");
            }
            {
                logger.LogError("facultyService:faculty with Id: " + id + " doesn't exist");

            }
            return res;
            
        }

       
        

    }

}
