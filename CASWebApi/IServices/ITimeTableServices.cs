using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface ITimeTableService
    {
        /// <summary>
        /// an interface that contains abstract methods and properties of timeTable services
        /// </summary>
        TimeTable GetById(string timetableId);
        List<TimeTable> GetAll();
        bool Create(TimeTable timeTable);
        bool Update(string id, TimeTable timeTableIn);
        bool RemoveById(string id);

        public TimeTable GetByCalendarName(string timeTableName);


    }
}
