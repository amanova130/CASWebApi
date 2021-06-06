using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface ITimeTableService
    {
        TimeTable GetById(string timetableId);
        List<TimeTable> GetAll();
        bool Create(TimeTable timeTable);
        void Update(string id, TimeTable timeTableIn);
        bool RemoveById(string id);

        public TimeTable GetByCalendarName(string timeTableName);
        public bool AddToSchedule(Schedule[] schedule, string calendarName);
        public void deleteEvent(string id);


    }
}
