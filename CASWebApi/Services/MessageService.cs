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

        /// <summary>
        /// get message by given id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>message object with given id</returns>
        public Message GetById(string messageId)
        {
            return DbContext.GetById<Message>("messages", messageId);
        }

        /// <summary>
        /// get all messages from db
        /// </summary>
        /// <returns>list of messages</returns>
        public List<Message> GetAll()
        {
            return DbContext.GetAll<Message>("messages");

        }
        public List<Message> GetAllByReceiverId(string id)
        {
            return DbContext.GetListByFilter<Message>("messages","receiver_id",id);

        }
        public List<Message> GetAllBySenderId(string id)
        {
            return DbContext.GetListByFilter<Message>("messages", "sender_id", id);

        }

        /// <summary>
        /// add new message object to db
        /// </summary>
        /// <param name="message">message object to add</param>
        /// <returns></returns>
        public bool Create(Message message)
        {
            message.Id= ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Message>("messages", message);
            return res;
        }
        

        /// <summary>
        /// edit an existing message by changing it to a new message object with the same id
        /// </summary>
        /// <param name="id">message to edit</param>
        /// <param name="messageIn">new message object</param>
        /// <returns>true if replaced successfully</returns>
        public void Update(string id, Message messageIn) =>
          DbContext.Update<Message>("messages", id, messageIn);


        /// <summary>
        /// remove message object with the given id from db
        /// </summary>
        /// <param name="id">id of the message to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id) =>
            DbContext.RemoveById<Message>("messages", id);
    }
}
