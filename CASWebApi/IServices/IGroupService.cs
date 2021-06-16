using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IGroupService
    {
        Group GetById(string groupId);
        List<Group> GetAll();
        bool Create(Group group);
        bool Update(string id, Group groupIn);
        bool RemoveById(string id);
    }
}
