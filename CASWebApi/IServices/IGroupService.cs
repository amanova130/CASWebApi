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
        Group Create(Group group);
        void Update(string id, Group groupIn);
        bool RemoveById(string id);
    }
}
