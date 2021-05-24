using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class GroupService : IGroupService
    {
        IDbSettings DbContext;

        public GroupService(IDbSettings settings)
        {
            DbContext = settings;
        }
     
        public Group GetById(string groupId)
        {
            return DbContext.GetById<Group>("group", groupId);
        }

        public List<Group> GetAll()
        {
            return DbContext.GetAll<Group>("group");

        }

        public bool Create(Group group)
        {
            group.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Group>("group", group);
            return res;
        }

        public void Update(string id, Group groupIn) =>
          DbContext.Update<Group>("group", id, groupIn);

        public bool RemoveById(string id)
        {
            bool res=DbContext.RemoveById<Group>("group", id);  
            if(res)
             DbContext.RemoveField<Group>("student", "group", id);
            return res;
        }
    }
}
