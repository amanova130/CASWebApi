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
    public class MessageService : IMessageService
    {
        IDbSettings DbContext;

        public MessageService(IDbSettings settings)
        {
            DbContext = settings;
        }

        public Message GetById(string messageId)
        {
            return DbContext.GetById<Message>("messages", messageId);
        }

        public List<Message> GetAll()
        {
            return DbContext.GetAll<Message>("messages");

        }

        public bool Create(Message message)
        {
            message.Id= ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Message>("messages", message);
            return res;
        }

        public void Update(string id, Message messageIn) =>
          DbContext.Update<Message>("messages", id, messageIn);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Message>("messages", id);
    }
}
