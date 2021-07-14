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
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public Schedule GetById(string eventId)
        {
            return DbContext.GetById<Schedule>("event", eventId);
        }

        public List<Schedule> GetAll()
        {
            return DbContext.GetAll<Schedule>("event");

        }

        public bool Create(string groupId,Schedule newEvent)
        {
            //newEvent.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.PushElement<Schedule>("timeTable", "schedule",newEvent,groupId, "groupName");
            return res;
        }

        public void Update(string id, Schedule eventIn) =>
          DbContext.Update<Schedule>("event", id, eventIn);



        public bool RemoveById(string eventId,string groupId)
        {
            //DbContext.GetById<Course>("course",id);
            //bool res = DbContext.PullElement<Schedule>("timeTable","schedule","group",groupId, eventId);
            // return res;
            bool res = DbContext.PullObject<Schedule>("timeTable","schedule",eventId,groupId, "groupName","eventId");
            return true;

        }
    }
}
