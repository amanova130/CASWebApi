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

        public AdminService(IDbSettings settings, ILogger<TeacherService> logger)
        {
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
            var admin= DbContext.GetById<Admin>("admin", adminId);
            if (admin == null)
            {
                logger.LogError("AdminService:Cannot get access to admin collection in Db");
            }
            logger.LogInformation("AdminService:Fetched admin data by id ");

            return admin;
        }

        /// <summary>
        /// get all admins from db
        /// </summary>
        /// <returns>list of admins</returns>
        public List<Admin> GetAll()
        {
            logger.LogInformation("AdminService:Getting all Admins from adminService");

            var adminList=DbContext.GetAll<Admin>("admin");
            if (adminList != null)
                logger.LogInformation("AdminService:Fetched All admin data");
            else
                logger.LogError("AdminService:Cannot get access to admin collection in Db");
            return adminList;

        }

        /// <summary>
        /// add new admin object to db
        /// </summary>
        /// <param name="admin">admin object to add</param>
        /// <returns>true if added,otherwise false</returns>
        public bool Create(Admin admin)
        {
            logger.LogInformation("got a new admin profile in adminService: " + admin);

            bool res =DbContext.Insert<Admin>("admin", admin);
            if (res)
                logger.LogInformation("A new admin profile added successfully and got in adminService" + admin);
            else
                logger.LogError("AdminService:Cannot create a admin, duplicated id or wrong format");




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
           bool res=DbContext.Update<Admin>("admin", id, adminIn);
            if(!res)
                logger.LogError("AdminService:Admin with Id: " + adminIn.Id + " doesn't exist");
            else
            {
                logger.LogInformation("AdminService:Admin with Id" + adminIn.Id +"has been updated successfully");

            }
            return res;

        }

        /// <summary>
        /// remove admin object with the given id from db
        /// </summary>
        /// <param name="id">id of the admin to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        { 
            bool res=DbContext.RemoveById<Admin>("admin", id);
            if(res)
                logger.LogInformation("AdminService:Admin with Id" + id + "has been deleted successfully");
            else
                logger.LogError("AdminService:Admin with Id: " + id + " doesn't exist");

            return res;

        }
    }
}
