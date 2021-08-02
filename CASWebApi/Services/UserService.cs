using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class UserService:IUserService
    {
        IDbSettings DbContext;
        private readonly ILogger logger;



        public UserService(IDbSettings settings, ILogger<UserService> logger)
        {
            DbContext = settings;
            this.logger = logger;

        }

        /// <summary>
        /// get user by given id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user object with given id</returns>
        public User GetById(string userId)
        {
            logger.LogInformation("UserService:Getting user by id");

            var user = DbContext.GetById<User>("user", userId);
            if (user == null)
                logger.LogError("UserService:Cannot get a user with a id: " + userId);
                    
            else
                logger.LogInformation("UserService:Fetched user data by id ");
            return user;
            
        }
        public User getByEmail(string userEmail)
        {
            logger.LogInformation("UserService:Getting user by email");

            var user = DbContext.GetDocumentByFilter<User>("user", "email", userEmail);
      
            if (user == null)
                logger.LogError("UserService:Cannot get a user with an email: " + userEmail);

            else
                logger.LogInformation("UserService:Fetched user data by email ");
            return user;

        }

        /// <summary>
        /// get all users from db
        /// </summary>
        /// <returns>list of users</returns>
        public List<User> GetAll()
        {
            logger.LogInformation("UserService:Getting all users");
            var users = DbContext.GetAll<User>("user");
            if (users == null)
                logger.LogError("UserService:Cannot get access to users collection in Db");
            else
                logger.LogInformation("UserService:fetched All users collection data");
            return users;
           
        }

        /// <summary>
        ///add new user object to db

        /// </summary>
        /// <param name="user"></param>
        /// <returns>created user</returns>
        public bool Create(User user)
        {
            logger.LogInformation("UserService:creating a new user profile : " + user);

            bool res = DbContext.Insert<User>("user", user);
            if (res)
                logger.LogInformation("UserService:A new user profile added successfully :" + user);
            else
                logger.LogError("UserService:Cannot create a user, duplicated id or wrong format");
            return res;
            
            
        }

        /// <summary>
        /// edit an existing user by changing it to a new user object with the same id
        /// </summary>
        /// <param name="id">user to edit</param>
        /// <param name="userIn">new user object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(string id, User userIn)
        {
            logger.LogInformation("userService:updating an existing user profile with id : " + userIn.UserName);

            bool res = DbContext.Update<User>("user", id, userIn); ;
            if (!res)
                logger.LogError("userService:user with Id: " + userIn.UserName + " doesn't exist");
            else
                logger.LogInformation("userService:user with Id" + userIn.UserName + "has been updated successfully");

            return res;
            
        }

        /// <summary>
        /// remove user by id from db
        /// </summary>
        /// <param name="id">id of the user to remove</param>
        /// <returns></returns>
        public bool RemoveById(string id)
        {
            logger.LogInformation("userService:deleting a user profile with id : " + id);

            bool res = DbContext.RemoveById<User>("user", id);
            if (res)
            {
                logger.LogInformation("userService:a user profile with id : " + id + "has been deleted successfully");
            }
            {
                logger.LogError("userService:user with Id: " + id + " doesn't exist");

            }
            return res;
         
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
