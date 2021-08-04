using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;


        public TimeTableService(IDbSettings settings, ILogger<FacultyService> logger)
        {
            DbContext = settings;
            this.logger = logger;

        }

        /// <summary>
        /// get timeTable object by given id
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <returns>single timeTable object with given id</returns>
        public TimeTable GetById(string timeTableId)
        {
            logger.LogInformation("TimeTable:Getting TimeTable by id");

            var timeTable = DbContext.GetById<TimeTable>("timeTable", timeTableId);
            if (timeTable == null)
                logger.LogError("TimeTableService:Cannot get a timetable with a Id: " + timeTableId);
            else
                logger.LogInformation("TimeTableService:Fetched timetable data by id ");
            return timeTable;
            
        }

        /// <summary>
        /// get single timeTable by groupName
        /// </summary>
        /// <param name="timeTableName"></param>
        /// <returns>timeTable object of specific group</returns>
        public TimeTable GetByCalendarName(string timeTableName)
        {
            logger.LogInformation("TimeTableService:Getting TimeTable by calendarName");

            var timeTable = DbContext.GetDocumentByFilter<TimeTable>("timeTable", "groupName", timeTableName);
            if (timeTable == null)
                logger.LogError("TimeTableService:Cannot get a timetable with a name: " + timeTableName);
            else
                logger.LogInformation("TimeTableService:Fetched timetable data by timeTableName ");
            return timeTable;
        }

        /// <summary>
        /// get all timeTable object from db
        /// </summary>
        /// <returns></returns>
        public List<TimeTable> GetAll()
        {
            logger.LogInformation("TimeTableService:Getting all timeTables");
            var faculties = DbContext.GetAll<TimeTable>("timeTable");
            if (faculties == null)
                logger.LogError("TimeTableService:Cannot get access to timeTables collection in Db");
            else
                logger.LogInformation("TimeTableService:fetched All timeTables collection data");
            return faculties;
           

        }

       
        /// <summary>
        /// add new timeTable object to db
        /// </summary>
        /// <param name="timeTable">object to add</param>
        /// <returns>true if added</returns>
        public bool Create(TimeTable timeTable)
        {
            logger.LogInformation("TimeTableService:create new timeTable");
            timeTable.status = true;
            if (GetByCalendarName(timeTable.CalendarName) == null)
                timeTable.CalendarId = CalendarService.CreateCalendar(timeTable.CalendarName);

                bool res = DbContext.Insert<TimeTable>("timeTable", timeTable);
            if (res)
                logger.LogInformation("TimeTableService:A new timeTable object added successfully :" + timeTable);
            else
                logger.LogError("TimeTableService:Cannot create a timeTable, duplicated id or wrong format");
            return res;
        }

        /// <summary>
        /// edit an existing timeTable by changing it to a new timeTable object with the same id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeTableIn"></param>
        public bool Update(string id, TimeTable timeTableIn)
        {
            logger.LogInformation("TimeTableService:updating an existing timetable object with id : " + timeTableIn.Id);

            bool res = DbContext.Update<TimeTable>("timeTable", timeTableIn.Id, timeTableIn);
            if (!res)
                logger.LogError("TimeTableService:timeTable with Id: " + timeTableIn.Id + " doesn't exist");
            else
                logger.LogInformation("TimeTableService:timeTable with Id" + timeTableIn.Id + "has been updated successfully");

            return res;
            
        }

        /// <summary>
        /// remove timeTable object with the given id from db
        /// </summary>
        /// <param name="id">id of the timeTable to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("TimeTableService:deleting a timeTable profile with id : " + id);

            bool res = DbContext.RemoveByFilter<TimeTable>("timeTable", "groupName", id) && (CalendarService.DeleteCalendar(id));
            if (res)
            {
                    logger.LogInformation("TimeTableService:a timeTable profile with id : " + id + "has been deleted successfully");
            }
            {
                logger.LogError("TimeTableService:timeTable with Id: " + id + " doesn't exist");
            }
            return res;
        }
    }
}
