using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    /// <summary>
    /// an interface that contains abstract methods and properties of user service
    /// </summary>
    public interface IUserService
    {
        User GetById(string teacherId);
        List<User> GetAll();
        User Create(User user);
        void Update(string id, User userIn);
        bool RemoveById(string id);
    }
}
