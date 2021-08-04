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
        User GetById(string id);
        List<User> GetAll();
        bool Create(User user);
        User getByEmail(string userEmail);
        bool resetPass(string email);
        User checkAuth(User userToCheck);
        string RandomString(int size, bool lowerCase);

        bool Update( User userIn);
        bool RemoveById(string id);

    }
}
