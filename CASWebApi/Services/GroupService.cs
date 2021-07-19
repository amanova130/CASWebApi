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
     
        /// <summary>
        /// get group object by given id 
        /// </summary>
        /// <param name="groupId">id of the group to find in db</param>
        /// <returns>found group object</returns>
        public Group GetById(string groupId)
        {
            return DbContext.GetById<Group>("group", groupId);
        }


        /// <summary>
        /// get all groups from db
        /// </summary>
        /// <returns>list of groups</returns>
        public List<Group> GetAll()
        {
            return DbContext.GetAll<Group>("group");

        }

        /// <summary>
        /// get total number of groups in db
        /// </summary>
        /// <returns>number of groups</returns>
        public int GetNumberOfGroups()
        {
            return DbContext.GetCountOfDocuments<Group>("group");
        }

        /// <summary>
        /// add new group object to db
        /// </summary>
        /// <param name="group"></param>
        /// <returns>true if added,false otherwise</returns>
        public bool Create(Group group)
        {
            group.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<Group>("group", group);
            return res;
        }

        /// <summary>
        /// edit an existing group by changing it to a new group object with the same id
        /// </summary>
        /// <param name="id">id of the group to edit</param>
        /// <param name="groupIn">new group object</param>
        /// <returns>true if replaced successfully,false otherwise</returns>
        public bool Update(string id, Group groupIn)
        {
            return DbContext.Update<Group>("group", id, groupIn);
        }

        /// <summary>
        /// remove group object with the given id from db
        /// </summary>
        /// <param name="id">id of the group to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id)
        {
            bool res=DbContext.RemoveById<Group>("group", id);  
            if(res)
                 DbContext.RemoveByFilter<Group>("student", "group", id);
            return res;
        }
    }
}
