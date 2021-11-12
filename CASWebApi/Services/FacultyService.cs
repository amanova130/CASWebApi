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
            try
            {
                var faculty = DbContext.GetById<Faculty>("faculty", facultyId);
                if (faculty == null)
                    logger.LogError("FacultyService:Cannot get a faculty with a facId: " + facultyId);
                else
                    logger.LogInformation("FacultyService:Fetched faculty data by id ");
                return faculty;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public Faculty GetByFacultyName(string facultyName)
        {
            logger.LogInformation("FacultyService:Getting faculty by id");
            try
            {
                var faculty = DbContext.GetDocumentByFilter<Faculty>("faculty", "fac_name", facultyName);

                if (faculty == null)
                    logger.LogError("FacultyService:Cannot get a faculty with a facName: " + facultyName);
                else
                    logger.LogInformation("FacultyService:Fetched faculty data by id ");
                return faculty;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// get all faculties from db
        /// </summary>
        /// <returns>list of faculties</returns>
        public List<Faculty> GetAll()
        {
            logger.LogInformation("FacultyService:Getting all faculties");
            try
            {
                var faculties = DbContext.GetAll<Faculty>("faculty");
                return faculties;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// get total number of faculties in db 
        /// </summary>
        /// <returns>integer number of faculties</returns>
        public int GetNumberOfFaculties()
        {
            logger.LogInformation("FacultyService:Getting count of faculty collections");
            try
            {
                int res = DbContext.GetCountOfDocuments<Faculty>("faculty");
                logger.LogInformation("FacultyService:fetched number of faculties");
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
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
            try
            {
                 return DbContext.Insert<Faculty>("faculty", faculty);           
            }
            catch (Exception e)
            {
                logger.LogError("FacultyService:Cannot create a faculty");
                throw e;
            }
        }

        /// <summary>
        /// edit existing faculty
        /// </summary>
        /// <param name="id">id of the faculty to edit</param>
        /// <param name="facultyIn">edited faculty's object to replace with the old one</param>
        /// <returns></returns>
        public bool Update(Faculty facultyIn)
        {
            logger.LogInformation("facultyService:updating an existing faculty profile with id : " + facultyIn.Id);
            try
            {
                bool res = DbContext.Update<Faculty>("faculty", facultyIn.Id, facultyIn);
                if (!res)
                    logger.LogError("facultyService:faculty with Id: " + facultyIn.Id + " doesn't exist");
                else
                    logger.LogInformation("facultyService:faculty with Id" + facultyIn.Id + "has been updated successfully");

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// remove faculty by given id
        /// </summary>
        /// <param name="id">id of the faculty to remove</param>
        /// <returns></returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("facultyService:deleting a faculty profile with id : " + id);
            try
            {
                bool res = DbContext.RemoveById<Faculty>("faculty", id);    
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("facultyService:got error: "+e);
                throw e;
            }
        }

       
        

    }

}
