using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly ILogger logger;
        IDbSettings DbContext;
        IUserService _userService;
        IMessageService _messageService;

        public AdminService(IDbSettings settings, IUserService userService,IMessageService messageService, ILogger<AdminService> logger)
        {
            _userService = userService;
            _messageService = messageService;
            this.logger = logger;
            DbContext = settings;
        }

        /// <summary>
        /// get admin by given id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>admin object with given id</returns>
        public Admin GetById(string adminId)
        {
            logger.LogInformation("AdminService:Getting course by id");
            try
            {
                var admin = DbContext.GetById<Admin>("admin", adminId);
                if (admin == null)
                {
                    logger.LogError("AdminService:object with given id doesn't exists in db");
                }
                else
                logger.LogInformation("AdminService:Fetched admin data by id ");

                return admin;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// get all admins from db
        /// </summary>
        /// <returns>list of admins</returns>
        public List<Admin> GetAll()
        {
            logger.LogInformation("AdminService:Getting all Admins from adminService");
            try
            {
                var adminList = DbContext.GetAll<Admin>("admin");
                    logger.LogInformation("AdminService:Fetched All admin data");        
                return adminList;
            }
            catch(Exception e)
            {
                logger.LogError("AdminService:Cannot get access to admin collection in Db");
                throw e;
            }

        }

        /// <summary>
        /// add new admin object to db
        /// </summary>
        /// <param name="admin">admin object to add</param>
        /// <returns>true if added,otherwise false</returns>
        /// 
        private bool sendEmailToNewAdmin(User user, string adminName)
        {
            Message message = new Message();
            message.Description = String.Format("Dear {0},\n Welcome to our college!\n" +
                                                  "Your authorization details: " +
                                                  "User name: {1}" +
                                                  "Password:{2}", adminName, user.UserName, user.Password);
            message.Subject = "Authorization Details";
            message.Receiver = new String[] { user.Email };
            message.DateTime = DateTime.Now;
            message.status = true;
            message.ReceiverNames = new string[] { adminName };
            try
            {
                _messageService.Create(message);
            }
            catch(Exception e)
            {
                throw e;
            }
            return true;
        }
        /// <summary>
        /// add new admin object to db
        /// </summary>
        /// <param name="admin">object to add</param>
        /// <returns>true if added</returns>
        public bool Create(Admin admin)
        {
            logger.LogInformation("got a new admin profile in adminService: " + admin);
            admin.Status = true;
            bool res;
            try
            {
                 res = DbContext.Insert<Admin>("admin", admin);
            }
            catch (Exception e)
            {
                if (e is MongoWriteException)
                    throw new Exception(String.Format("Admin with Id: {0} already exists", admin.Id), e);
                throw e;
            }
            if (res)
            {
                User _user = new User();
                _user.ChangePwdDate = DateTime.Now.AddYears(1).ToString("MM/dd/yyyy");
                _user.UserName = admin.Id;
                _user.Password = admin.Birth_date.Replace("-", "");
                _user.Role = "Admin";
                try
                {
                    res = _userService.Create(_user);
                    sendEmailToNewAdmin(_user, admin.First_name + " " + admin.Last_name);
                    logger.LogInformation("A new admin profile and his user profile added successfully and got in adminService" + admin);
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return res;
        }

        /// <summary>
        /// edit an existing admin object by changing it to a new admin object with the same id
        /// </summary>
        /// <param name="id">admin to edit</param>
        /// <param name="adminIn">new admin object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(string id, Admin adminIn)
        {

            try
            {
                bool res = DbContext.Update<Admin>("admin", id, adminIn);
                if (!res)
                    logger.LogError("AdminService:Admin with Id: " + adminIn.Id + " doesn't exist");
                else
                {
                    logger.LogInformation("AdminService:Admin with Id" + adminIn.Id + "has been updated successfully");
                    var user = _userService.GetById(adminIn.Id);
                    if (user.Email != adminIn.Email)
                    {
                        user.Email = adminIn.Email;
                        res = _userService.Update(user);
                        if (res)
                            logger.LogInformation("AdminService:user profile with Id" + adminIn.Id + "has been updated successfully");
                        else
                            logger.LogError("AdminService:User with Id: " + adminIn.Id + " doesn't exist");
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// remove admin object with the given id from db
        /// </summary>
        /// <param name="id">id of the admin to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            try { 
            bool res=DbContext.RemoveById<Admin>("admin", id);
            if (res)
            {
                logger.LogInformation("AdminService:Admin with Id" + id + "has been deleted successfully");
                res = _userService.RemoveById(id);
                if(res)
                {
                    logger.LogInformation("AdminService:User with Id" + id + "has been deleted successfully");
                }
            }
            else
                logger.LogError("AdminService:Admin with Id: " + id + " doesn't exist");

            return res;
        }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
