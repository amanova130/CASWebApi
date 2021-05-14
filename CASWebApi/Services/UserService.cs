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


        public User GetById(string userId)
        {
            return DbContext.GetById<User>("user", userId);
        }

        public List<User> GetAll()
        {
            return DbContext.GetAll<User>("user");

        }

        public User Create(User user)
        {
            user.Id = ObjectId.GenerateNewId().ToString();
            DbContext.Insert<User>("user", user);
            return user;
        }

        public void Update(string id, User userIn) =>
          DbContext.Update<User>("user", id, userIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<User>("user", id);
    }
}
