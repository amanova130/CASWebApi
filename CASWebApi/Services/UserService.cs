using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;


namespace CASWebApi.Services
{
    public class UserService : IUserService
    {
        IDbSettings DbContext;
        private readonly ILogger logger;
        IMessageService _messageService;




        public UserService(IDbSettings settings, ILogger<UserService> logger, IMessageService messageService)
        {
            DbContext = settings;
            this.logger = logger;
            _messageService = messageService;

        }

        /// <summary>
        /// get user by given id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user object with given id</returns>
        public User GetById(string userId)
        {
            logger.LogInformation("UserService:Getting user by id");
            try
            {
                var user = DbContext.GetById<User>("user", userId);
                if (user == null)
                    logger.LogError("UserService:Cannot get a user with a id: " + userId);

                else
                    logger.LogInformation("UserService:Fetched user data by id ");
                return user;
            }
            catch(Exception e)
            {
                throw e;
            }
            

        }

        /// <summary>
        /// Check Authentication
        /// </summary>
        /// <param name="userToCheck">User object</param>
        /// <returns>If user found return user object, if not null</returns>
        public User checkAuth(User userToCheck)
        {
            bool res;
            var user = GetById(userToCheck.UserName);
            if (user != null)
            {
                logger.LogInformation("Got User");
                try
                {
                    res = BCryptNet.Verify(userToCheck.Password, user.Password);
                    if (res)
                        return user;    
                }
                catch (Exception e)
                {
                    res = false;
                    throw e;
                }
            }
            else
            {
                logger.LogError("Cannot get access to user collection in Db");
            }
            return null;
        }
        /// <summary>
        /// Get User by Email
        /// </summary>
        /// <param name="userEmail"> User email</param>
        /// <returns>User object or null</returns>
        public User getByEmail(string userEmail)
        {
            logger.LogInformation("UserService:Getting user by email");
            try
            {
                var user = DbContext.GetDocumentByFilter<User>("user", "email", userEmail);
                if (user == null)
                    logger.LogError("UserService:Cannot get a user with an email: " + userEmail);

                else
                    logger.LogInformation("UserService:Fetched user data by email ");
                return user;
            }
          catch(Exception e)
            {
                logger.LogError("UserService:Cannot get access to admin collection in Db");
                throw e;
            }          
        }

        /// <summary>
        /// get all users from db
        /// </summary>
        /// <returns>list of users</returns>
        public List<User> GetAll()
        {
            logger.LogInformation("UserService:Getting all users");
            try
            {
                var users = DbContext.GetAll<User>("user");
                logger.LogError("UserService:Cannot get access to users collection in Db");
                return users;
            }
           catch(Exception e)
            {
                logger.LogInformation("UserService:fetched All users collection data");
                throw e;
            }
        }

        /// <summary>
        ///add new user object to db
        /// </summary>
        /// <param name="user"></param>
        /// <returns>created user</returns>
        public bool Create(User user)
        {
            logger.LogInformation("UserService:creating a new user profile : " + user);
            try
            {
                user.Status = true;
                //user.Password = HashPassword(user.Password);
                user.Password = BCryptNet.HashPassword(user.Password);
                bool res = DbContext.Insert<User>("user", user);
                if (res)
                    logger.LogInformation("UserService:A new user profile added successfully :" + user);
                else
                    logger.LogError("UserService:Cannot create a user, duplicated id or wrong format");
                return res;
            }
           catch(Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// edit an existing user by changing it to a new user object with the same id
        /// </summary>
        /// <param name="id">user to edit</param>
        /// <param name="userIn">new user object</param>
        /// <returns>true if replaced successfully</returns>
        public bool Update(User userIn)
        {
            logger.LogInformation("userService:updating an existing user profile with id : " + userIn.UserName);
            var user = GetById(userIn.UserName);
            bool isPwdChanged;
            try
            {
                isPwdChanged = BCryptNet.Verify(userIn.Password, user.Password);
            }
            catch (Exception e)
            {
                isPwdChanged = false;
                throw e;
            }
            if (!isPwdChanged)
                userIn.Password = BCryptNet.HashPassword(userIn.Password);
            else
                userIn.Password = user.Password;

            bool res = DbContext.Update<User>("user", userIn.UserName, userIn); ;
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
            try
            {
                bool res = DbContext.RemoveById<User>("user", id);
                if (res)
                    logger.LogInformation("userService:a user profile with id : " + id + "has been deleted successfully");
                else
                    logger.LogError("userService:user with Id: " + id + " doesn't exist");
                return res;
            }
            catch(Exception e)
            {
                throw e;
            }
            

        }
        /// <summary>
        /// Reset Password if user forgot the password, will send an email to user with a new password
        /// </summary>
        /// <param name="email">User Email</param>
        /// <returns>Result</returns>
        public bool resetPass(string email)
        {
            bool res;
            Student student = new Student();
            Admin admin = new Admin();
            try
            {
                var user = getByEmail(email);
                if (user != null)
                {
                    if (user.Role == "Student")
                        student = DbContext.GetById<Student>("student", user.UserName);
                    else
                        admin = DbContext.GetById<Admin>("admin", user.UserName);
                }
                else
                    return false;
                if ((user.Role == "Student" && student != null) || (user.Role == "Admin" && admin != null))
                {
                    user.Password = RandomString(6, true);
                    Message resetPass = new Message();
                    resetPass.ReceiverNames = new string[1];
                    if (user.Role == "Student")
                        resetPass.ReceiverNames[0] = student.First_name + " " + student.Last_name;
                    else
                        resetPass.ReceiverNames[0] = admin.First_name + " " + admin.Last_name;
                    resetPass.Receiver = new string[1];
                    resetPass.Receiver[0] = email;
                    resetPass.Description = "Following your request, a password reset for the system was performed\n"
                                              + "Your new password is:\n"
                                              + user.Password + "\n"
                                              + "Do not reply to this message.\n"
                                              + "This system message has been sent to you automatically because you have requested a password reset.";
                    resetPass.Subject = " Reset password";
                    resetPass.DateTime = DateTime.Now;
                    resetPass.status = true;
                    res = _messageService.Create(resetPass);
                    if (res)
                    {
                        res = Update(user);
                    }
                }
                else
                    res = false;
                return res;
            }
           catch(Exception e)
            {
                throw e;
            }
           
        }
    
        /// <summary>
        /// Check Entered Password
        /// </summary>
        /// <param name="newPass"></param>
        /// <param name="userId"></param>
        /// <returns>Result</returns>
        public bool checkEnteredPass(string newPass, string userId)
        {
            var user = GetById(userId);
            bool res;
            if(user != null)
            {
                try
                {
                    res = BCryptNet.Verify( newPass, user.Password);
                }
                catch (Exception e)
                {
                    res = false;
                    throw e;
                }
                return res;
            }
            return false;
        }
        
        /// <summary>
        /// Get Random string to password
        /// </summary>
        /// <param name="size">Size of password</param>
        /// <param name="lowerCase"></param>
        /// <returns>new Password</returns>
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
