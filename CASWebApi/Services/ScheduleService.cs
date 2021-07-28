using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class ScheduleService : IScheduleService
    {
        IDbSettings DbContext;

        public ScheduleService(IDbSettings settings)
        {
            DbContext = settings;
        }

        /// <summary>
        /// get single schedule object by given id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>schedule object with given id</returns>
        

        /// <summary>
        /// get all schedule objects from db
        /// </summary>
        /// <returns>list of schedule objects</returns>
        public List<Schedule> GetAll()
        {
            return DbContext.GetAll<Schedule>("event");

        }

        /// <summary>
        /// add new schedule object to array of schedules
        /// </summary>
        /// <param name="groupId">the name of the group where you want to add a new schedule</param>
        /// <param name="newEvent">schedule object to add</param>
        /// <returns>true if added</returns>
        public bool Create(string groupId,Schedule newEvent)
        {
            //newEvent.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.PushElement<Schedule>("timeTable", "schedule",newEvent,groupId, "groupName");
            return res;
        }

        /// <summary>
        /// edit an existing schedule by changing it to a new schedule object with the same id
        /// </summary>
        /// <param name="id">id of the schedule to edit</param>
        /// <param name="eventIn">new schedule object</param>
        public void Update(string id, Schedule eventIn) =>
          DbContext.Update<Schedule>("event", id, eventIn);


        /// <summary>
        /// remove schedule object with the given id from db
        /// </summary>
        /// <param name="eventId">id of the schedule to remove</param>
        /// <param name="groupId">the number of the group in which the schedule is located</param>
        /// <returns></returns>
        public bool RemoveById(string eventId,string groupId)
        {
           
            bool res = DbContext.PullObject<Schedule>("timeTable","schedule",eventId,groupId, "groupName","eventId");
            return true;

        }
    }
}
