using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IAdminService
    {
        Admin GetById(string adminId);
        List<Admin> GetAll();
        bool Create(Admin admin);
        bool Update(string id, Admin adminIn);
        bool RemoveById(string id);
    }
}
