using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of schedule service
    /// </summary>
    public interface IScheduleService
    {
        List<Schedule> GetAll();
        bool Create(string groupId,Schedule newEvent);
        bool Update(string id, Schedule eventIn);
        bool RemoveById(string eventId, string groupId);
        public Schedule GetEvent(string groupId, string id);

    }
}
