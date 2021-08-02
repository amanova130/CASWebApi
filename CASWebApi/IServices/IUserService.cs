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
        bool Create(User user);
        User getByEmail(string userEmail);
        string RandomString(int size, bool lowerCase);

        bool Update(string id, User userIn);
        bool RemoveById(string id);
    }
}
