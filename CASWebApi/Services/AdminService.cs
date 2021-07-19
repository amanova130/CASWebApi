using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class AdminService : IAdminService
    {
        IDbSettings DbContext;

        public AdminService(IDbSettings settings)
        {
            DbContext = settings;
        }

        /// <summary>
        /// get admin by given id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>admin object with given id</returns>
        public Admin GetById(string adminId)
        {
            return DbContext.GetById<Admin>("admin", adminId);
        }

        /// <summary>
        /// get all admins from db
        /// </summary>
        /// <returns>list of admins</returns>
        public List<Admin> GetAll()
        {
            return DbContext.GetAll<Admin>("admin");

        }

        /// <summary>
        /// add new admin object to db
        /// </summary>
        /// <param name="admin">admin object to add</param>
        /// <returns>true if added,otherwise false</returns>
        public bool Create(Admin admin)
        {
            bool res=DbContext.Insert<Admin>("admin", admin);
            return res;
        }

        /// <summary>
        /// edit an existing admin object by changing it to a new admin object with the same id
        /// </summary>
        /// <param name="id">admin to edit</param>
        /// <param name="adminIn">new admin object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(string id, Admin adminIn) =>
         DbContext.Update<Admin>("admin", id, adminIn);

        /// <summary>
        /// remove admin object with the given id from db
        /// </summary>
        /// <param name="id">id of the admin to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id) =>
            DbContext.RemoveById<Admin>("admin", id);
    }
}
