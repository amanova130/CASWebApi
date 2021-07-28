using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class TimeTableService : ITimeTableService
    {
        IDbSettings DbContext;

        public TimeTableService(IDbSettings settings)
        {
            DbContext = settings;
        }

        /// <summary>
        /// get timeTable object by given id
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <returns>single timeTable object with given id</returns>
        public TimeTable GetById(string timeTableId)
        {
            return DbContext.GetById<TimeTable>("timeTable", timeTableId);
        }

        /// <summary>
        /// get single timeTable by groupName
        /// </summary>
        /// <param name="timeTableName"></param>
        /// <returns>timeTable object of specific group</returns>
        public TimeTable GetByCalendarName(string timeTableName)
        {
            return DbContext.GetDocumentByFilter<TimeTable>("timeTable", "groupName", timeTableName) ;
        }

        /// <summary>
        /// get all timeTable object from db
        /// </summary>
        /// <returns></returns>
        public List<TimeTable> GetAll()
        {
            return DbContext.GetAll<TimeTable>("timeTable");

        }

       
        /// <summary>
        /// add new timeTable object to db
        /// </summary>
        /// <param name="timeTable">object to add</param>
        /// <returns>true if added</returns>
        public bool Create(TimeTable timeTable)
        {

            bool res = DbContext.Insert<TimeTable>("timeTable", timeTable);
            return res;
        }

        /// <summary>
        /// edit an existing timeTable by changing it to a new timeTable object with the same id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeTableIn"></param>
        public bool Update(string id, TimeTable timeTableIn) =>
          DbContext.Update<TimeTable>("timeTable", timeTableIn.Id, timeTableIn);

        /// <summary>
        /// remove timeTable object with the given id from db
        /// </summary>
        /// <param name="id">id of the timeTable to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            return DbContext.RemoveByFilter<TimeTable>("timeTable", "groupName", id) && (CalendarService.DeleteCalendar(id));
        }
    }
}
