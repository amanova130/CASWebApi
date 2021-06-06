using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IEventService
    {
        EventTest GetById(string eventId);
        List<EventTest> GetAll();
        bool Create(EventTest newEvent);
        void Update(string id, EventTest eventIn);
        bool RemoveById(string id);
    }
}
