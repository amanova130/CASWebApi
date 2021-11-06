using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;


        public MessageService(IDbSettings settings, ILogger<MessageService> logger)
        {
            this.logger = logger;
            DbContext = settings;
        }

        /// <summary>
        /// get message by given id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>message object with given id</returns>
        public Message GetById(string messageId)
        {
            try
            {
                var message = DbContext.GetById<Message>("messages", messageId);
                if (message != null)
                    logger.LogInformation("MessageService:Fetched message data by id ");
                else
                    logger.LogError("message with given id doesn't exists: ");
                return message;
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// get all messages from db
        /// </summary>
        /// <returns>list of messages</returns>
        public List<Message> GetAll()
        {
            logger.LogInformation("MessageService:Getting all messages");
            try
            {
                var messages = DbContext.GetAll<Message>("messages");
                logger.LogInformation("MessageService:fetched All messages collection data");
                return messages;
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }
        /// <summary>
        /// getting all messages by receiver id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of messages</returns>
        public List<Message> GetAllByReceiverId(string id)
        {
            try
            {
                return DbContext.GetListByFilter<Message>("messages", "receiver_id", id);
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }
        public List<Message> GetAllDeletedBySender(string id)
        {
            try
            {
                var deletedMessages = DbContext.GetDeletedDocumentsByFilter<Message>("messages", "sender_id", id);
                return deletedMessages;
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }
        public List<Message> GetAllBySenderId(string id)
        {
            try
            {
                return DbContext.GetListByFilter<Message>("messages", "sender_id", id);
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// add new message object to db
        /// </summary>
        /// <param name="message">message object to add</param>
        /// <returns></returns>
        public bool Create(Message message)
        {
            var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            client.Credentials = new System.Net.NetworkCredential("casmanagment78@gmail.com", "casmanagment123");

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new System.Net.Mail.MailAddress("casmanagment78@gmail.com");
            string emailReceivers = String.Empty;
            for (int i = 0; i < message.Receiver.Length; i++)
                emailReceivers += message.Receiver[i] + ',';
            emailReceivers = emailReceivers.TrimEnd(',');
            message.Sender = "Admin";
            try
            {
                mailMessage.To.Add(emailReceivers);


                mailMessage.Body = message.Description;

                mailMessage.Subject = message.Subject;

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                client.SendMailAsync(mailMessage);


                message.Id = ObjectId.GenerateNewId().ToString();
                bool res = DbContext.Insert<Message>("messages", message);
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }


        /// <summary>
        /// edit an existing message by changing it to a new message object with the same id
        /// </summary>
        /// <param name="id">message to edit</param>
        /// <param name="messageIn">new message object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(string id, Message messageIn) {
            try
            {
                return DbContext.Update<Message>("messages", id, messageIn);
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }


        /// <summary>
        /// remove message object with the given id from db
        /// </summary>
        /// <param name="id">id of the message to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            try
            {
                return DbContext.RemoveById<Message>("messages", id);
            }
            catch (Exception e)
            {
                logger.LogError("MessageService:got error : " + e);
                throw e;
            }
        }
    }
}
