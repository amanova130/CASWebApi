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
    public class EventService : IEventService
    {
        IDbSettings DbContext;

        public EventService(IDbSettings settings)
        {
            DbContext = settings;
        }
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public EventTest GetById(string eventId)
        {
            return DbContext.GetById<EventTest>("event", eventId);
        }

        public List<EventTest> GetAll()
        {
            return DbContext.GetAll<EventTest>("event");

        }

        public bool Create(EventTest newEvent)
        {
            newEvent.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<EventTest>("event", newEvent);
            return res;
        }

        public void Update(string id, EventTest eventIn) =>
          DbContext.Update<EventTest>("event", id, eventIn);



        public bool RemoveById(string id)
        {
            //DbContext.GetById<Course>("course",id);
            bool res = DbContext.RemoveById<EventTest>("events", id);
            //if (res)
            //{
            //    DbContext.PullElement<Event>("faculty", "events", id);
            //    DbContext.PullElement<Event>("group", "events", id);
            //}
            return res;

        }
    }
}
