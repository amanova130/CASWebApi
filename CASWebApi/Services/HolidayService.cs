using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class HolidayService : IHolidayService
    {
        IDbSettings DbContext;
        private readonly ILogger logger;


        public HolidayService(IDbSettings settings, ILogger<HolidayService> logger)
        {
            this.logger = logger;
            DbContext = settings;
        }

        /// <summary>
        /// get single holiday object by given id
        /// </summary>
        /// <param name="holidayId">id of the holiday object</param>
        /// <returns>holiday object with given id</returns>
        public Holiday GetById(string holidayId)
        {
            logger.LogInformation("HolidayService:Getting holiday by id");
            try
            {
                Holiday holiday = DbContext.GetById<Holiday>("holiday", holidayId);
                if (holiday == null)              
                 logger.LogError("holiday service with given id doesn't exists: ");
                else
                    logger.LogInformation("HolidayService:Fetched holiday data by id ");
                return holiday;
            }
            catch(Exception e)
            {
                logger.LogError("HolidayService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// get all holiday objects from db
        /// </summary>
        /// <returns>list of all holidays</returns>
        public List<Holiday> GetAll()
        {
            logger.LogInformation("HolidayService:Getting all holidays");
            try
            {
                var holidays = DbContext.GetAll<Holiday>("holiday");
                    logger.LogInformation("HolidayService:fetched All holidays collection data");
                return holidays;
            }
            catch (Exception e)
            {
                logger.LogError("HolidayService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// add new holiday object to db
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns>true if added</returns>
        public bool Create(Holiday holiday)
        {
            logger.LogInformation("HolidayService:creating a new holiday profile : " + holiday);

            holiday.Id = ObjectId.GenerateNewId().ToString();
            try
            {
                DbContext.Insert<Holiday>("holiday", holiday);
                    logger.LogInformation("HolidayService:A new holiday profile added successfully :" + holiday); 
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("HolidayService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// edit an existing holiday by changing it to a new holiday object with the same id
        /// </summary>
        /// <param name="id">id of the holiday to edit</param>
        /// <param name="holiday">new holiday object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(string id, Holiday holiday)
        {
            logger.LogInformation("HolidayService:updating an existing holiday profile with id : " + holiday.Id);
            try
            {
                bool res = DbContext.Update<Holiday>("holiday", id, holiday);
                if (!res)
                    logger.LogError("HolidayService:holiday with Id: " + holiday.Id + " doesn't exist");
                else
                    logger.LogInformation("HolidayService:holiday with Id" + holiday.Id + "has been updated successfully");

                return res;
            }
            catch (Exception e)
            {
                logger.LogError("HolidayService:got error : " + e);
                throw e;
            }
        }
    
        /// <summary>
        /// remove holiday object with the given id from db
        /// </summary>
        /// <param name="id">id of the holiday to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("HolidayService:deleting a holiday profile with id : " + id);
            try
            {
                bool res = DbContext.RemoveById<Holiday>("holiday", id);
                if (res)
                    logger.LogInformation("HolidayService:a holiday profile with id : " + id + "has been deleted successfully");
                else
                    logger.LogError("HolidayService:holiday with Id: " + id + " doesn't exist");

                return res;
            }
            catch (Exception e)
            {
                logger.LogError("HolidayService:got error : " + e);
                throw e;
            }
        }
    
    }
}
