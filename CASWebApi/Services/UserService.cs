using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class UserService:IUserService
    {
        IDbSettings DbContext;
        

        public UserService(IDbSettings settings)
        {
            DbContext = settings;
        }

        /// <summary>
        /// get user by given id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user object with given id</returns>
        public User GetById(string userId)
        {
            return DbContext.GetById<User>("user", userId);
        }

        /// <summary>
        /// get all users from db
        /// </summary>
        /// <returns>list of users</returns>
        public List<User> GetAll()
        {
            return DbContext.GetAll<User>("user");
        }

        /// <summary>
        ///add new user object to db

        /// </summary>
        /// <param name="user"></param>
        /// <returns>created user</returns>
        public User Create(User user)
        {
            DbContext.Insert<User>("user", user);
            return user;
        }

        /// <summary>
        /// edit an existing user by changing it to a new user object with the same id
        /// </summary>
        /// <param name="id">user to edit</param>
        /// <param name="userIn">new user object</param>
        /// <returns>true if replaced successfully</returns>
        public void Update(string id, User userIn) =>
          DbContext.Update<User>("user", id, userIn);

        /// <summary>
        /// remove user by id from db
        /// </summary>
        /// <param name="id">id of the user to remove</param>
        /// <returns></returns>
        public bool RemoveById(string id) =>
            DbContext.RemoveById<User>("user", id);
    }
}
