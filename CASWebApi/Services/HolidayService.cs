using CASWebApi.IServices;
using CASWebApi.Models;
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


        public HolidayService(IDbSettings settings)
        {
            DbContext = settings;
        }

        /// <summary>
        /// get single holiday object by given id
        /// </summary>
        /// <param name="holidayId">id of the holiday object</param>
        /// <returns>holiday object with given id</returns>
        public Holiday GetById(string holidayId)
        {
            return DbContext.GetById<Holiday>("holiday", holidayId);
        }

        /// <summary>
        /// get all holiday objects from db
        /// </summary>
        /// <returns>list of all holidays</returns>
        public List<Holiday> GetAll()
        {
            return DbContext.GetAll<Holiday>("holiday");

        }

        /// <summary>
        /// add new holiday object to db
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns>true if added</returns>
        public bool Create(Holiday holiday)
        {
            holiday.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Holiday>("holiday", holiday);
            return res;
        }

        /// <summary>
        /// edit an existing holiday by changing it to a new holiday object with the same id
        /// </summary>
        /// <param name="id">id of the holiday to edit</param>
        /// <param name="holiday">new holiday object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(string id, Holiday holiday)
        {
            return DbContext.Update<Holiday>("holiday", id, holiday);
        }
    
        /// <summary>
        /// remove holiday object with the given id from db
        /// </summary>
        /// <param name="id">id of the holiday to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            bool res = DbContext.RemoveById<Holiday>("holiday", id);
            if (res)
            {
                DbContext.RemoveById<Holiday>("holiday", id);
            }
            return res;

        }
    
    }
}
