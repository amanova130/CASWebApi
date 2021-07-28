using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of messages service
    /// </summary>
    public interface IMessageService
    {
        Message GetById(string messageId);
        List<Message> GetAll();
        public bool Create(Message message);
        void Update(string id, Message messageIn);
        bool RemoveById(string id);
        public List<Message> GetAllByReceiverId(string id);
        public List<Message> GetAllBySenderId(string id);



    }
}
