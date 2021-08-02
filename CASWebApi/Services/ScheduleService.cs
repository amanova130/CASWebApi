using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;


        public ScheduleService(IDbSettings settings)
        {
            DbContext = settings;
            this.logger = logger;

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
            logger.LogInformation("ScheduleService:Getting all Schedule objects");
            var schedules = DbContext.GetAll<Schedule>("faculty");
            if (schedules == null)
                logger.LogError("ScheduleService:Cannot get access to Schedule collection in Db");
            else
                logger.LogInformation("ScheduleService:fetched All Schedule objects collection data");
            return schedules;
           
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
            if (res)
                logger.LogInformation("ScheduleService:A new schedule object added successfully :" + newEvent);
            else
                logger.LogError("ScheduleService:Cannot create a schedule object, duplicated id or wrong format");
            return res;
        }

        /// <summary>
        /// edit an existing schedule by changing it to a new schedule object with the same id
        /// </summary>
        /// <param name="id">id of the schedule to edit</param>
        /// <param name="eventIn">new schedule object</param>
        public bool Update(string id, Schedule eventIn)
        {
            logger.LogInformation("ScheduleService:updating an existing schedule object with id : " + id);

            bool res = DbContext.Update<Schedule>("event", id, eventIn);
            if (!res)
                logger.LogError("ScheduleService:event with Id: " + eventIn.EventId + " doesn't exist");
            else
                logger.LogInformation("ScheduleService:event with Id" + eventIn.EventId + "has been updated successfully");

            return res;
            
        }


        /// <summary>
        /// remove schedule object with the given id from db
        /// </summary>
        /// <param name="eventId">id of the schedule to remove</param>
        /// <param name="groupId">the number of the group in which the schedule is located</param>
        /// <returns></returns>
        public bool RemoveById(string eventId,string groupId)
        {
            logger.LogInformation("ScheduleService:deleting a schedule object with id : " + eventId);

            bool res = DbContext.PullObject<Schedule>("timeTable","schedule",eventId,groupId, "groupName","eventId");

            if (res)
            {
                logger.LogInformation("ScheduleService:a schedule profile with id : " + eventId + "has been deleted successfully");
            }
            {
                logger.LogError("ScheduleService:schedule with Id: " + eventId + " doesn't exist");

            }
            return res;

        }
    }
}
