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

        public Holiday GetById(string holidayId)
        {
            return DbContext.GetById<Holiday>("holiday", holidayId);
        }

        public List<Holiday> GetAll()
        {
            return DbContext.GetAll<Holiday>("holiday");

        }

        public bool Create(Holiday holiday)
        {
            holiday.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Holiday>("holiday", holiday);
            return res;
        }

        public bool Update(string id, Holiday holiday)
        {
            return DbContext.Update<Holiday>("holiday", id, holiday);
        }
    

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
