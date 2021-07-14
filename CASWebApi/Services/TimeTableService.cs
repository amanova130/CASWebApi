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


        public TimeTable GetById(string timeTableId)
        {
            return DbContext.GetById<TimeTable>("timeTable", timeTableId);
        }
        public TimeTable GetByCalendarName(string timeTableName)
        {
            return DbContext.GetDocumentByFilter<TimeTable>("timeTable", "groupName", timeTableName) ;
        }

        public List<TimeTable> GetAll()
        {
            return DbContext.GetAll<TimeTable>("timeTable");

        }

        public void deleteEvent(string id)
        {
            //DbContext.PullElement<Schedule>("timeTable", "schedule", id);

        }

        public bool Create(TimeTable timeTable)
        {

            bool res = DbContext.Insert<TimeTable>("timeTable", timeTable);
            return res;
        }
        public bool AddToSchedule(Schedule[] schedule,string calendarName)

        {
            bool isSucceeded = true;
            for (int i = 0; i < schedule.Length && isSucceeded; i++)

                if (!DbContext.PushElement<Schedule>("timeTable", "schedule", schedule[i], calendarName, "groupName"))
                    isSucceeded = false;
            return isSucceeded;
            
        }
        public void Update(string id, TimeTable timeTableIn) =>
          DbContext.Update<TimeTable>("timeTable", id, timeTableIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<TimeTable>("timeTable", id);
    }
}
