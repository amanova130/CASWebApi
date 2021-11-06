using CASWebApi.IServices;
using CASWebApi.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;
        ITimeTableService _timeTableService;
        IDbSettings DbContext;

        public GroupService(IDbSettings settings, ITimeTableService timeTableService, ILogger<GroupService> logger)
        {
            this.logger = logger;
            _timeTableService = timeTableService;

            DbContext = settings;
        }
     
        /// <summary>
        /// get group object by given id 
        /// </summary>
        /// <param name="groupId">id of the group to find in db</param>
        /// <returns>found group object</returns>
        public Group GetById(string groupId)
        {
            logger.LogInformation("GroupService:Getting group by id");
            try
            {
                Group group = DbContext.GetById<Group>("group", groupId);
                if (group == null)
                    logger.LogError("GroupService:group with given id doesn't exists");
                else
                    logger.LogInformation("GroupService:Fetched group data by id ");
                return group;
            }
            catch(Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }

        public List<Group> GetGroupsByFaculty(string id)
        {
            logger.LogInformation("groupService:Getting all groups by faculty name");
            try
            {
                var groups = DbContext.GetListByFilter<Group>("group", "fac_name", id);
                if (groups == null)
                    logger.LogError("groupService:Cannot get access to groups collection in Db");
                else
                    logger.LogInformation("groupService:fetched All group collection data by faculty name");
                return groups;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// Get group details by groupName
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public Group GetGroupByName(string groupName)
        {
            logger.LogInformation("groupService:Getting all groups by faculty name");
            try
            {
                var group = DbContext.GetDocumentByFilter<Group>("group", "num_group", groupName);
                if (group == null)
                    logger.LogError("group with given name doesn't exists");
                else
                    logger.LogInformation("groupService:fetched All group collection data by faculty name");
                return group;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }



        /// <summary>
        /// get all groups from db
        /// </summary>
        /// <returns>list of groups</returns>
        public List<Group> GetAll()
        {
            logger.LogInformation("groupService:Getting all groups by faculties");
            try
            { 
            var groups = DbContext.GetAll<Group>("group");
            return groups;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// get total number of groups in db
        /// </summary>
        /// <returns>number of groups</returns>
        public int GetNumberOfGroups()
        {
            logger.LogInformation("groupService:Getting count of group collections");
            try
            {
                int res = DbContext.GetCountOfDocuments<Group>("group");
                logger.LogInformation("groupService:fetched number of groups");
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }

            /// <summary>
            /// add new group object to db
            /// </summary>
            /// <param name="group"></param>
            /// <returns>true if added,false otherwise</returns>
            public bool Create(Group group)
             {
            logger.LogInformation("groupService:creating a new group profile : " + group);
            group.Status = true;
            group.Id = ObjectId.GenerateNewId().ToString();
            try
            {
                bool res = DbContext.Insert<Group>("group", group);
                if (res)
                {
                    logger.LogInformation("groupService:A new group profile added successfully :" + group);
                    TimeTable timeTable = new TimeTable();
                    timeTable.CalendarName = group.GroupNumber;
                    timeTable.GroupSchedule = new Schedule[0];
                    timeTable.status = true;
                    timeTable.CalendarId = CalendarService.CreateCalendar(timeTable.CalendarName);
                    if (timeTable.CalendarId != null)
                    {
                        logger.LogInformation("groupService:A new google calendar for a group added successfully :" + group);
                        res = _timeTableService.Create(timeTable);
                    }
                    else
                    {
                        logger.LogError("Failed to create a google calendar");
                        res = false;
                    }
                }
                else
                    logger.LogError("groupService:Cannot create a group, duplicated id or wrong format");
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// edit an existing group by changing it to a new group object with the same id
        /// </summary>
        /// <param name="id">id of the group to edit</param>
        /// <param name="groupIn">new group object</param>
        /// <returns>true if replaced successfully,false otherwise</returns>
        public bool Update(string id, Group groupIn)
        {
            logger.LogInformation("groupService:updating an existing group profile with id : " + groupIn.Id);
            try
            {
                bool res = DbContext.Update<Group>("group", id, groupIn);
                if (!res)
                    logger.LogError("groupService:group with Id: " + groupIn.Id + " doesn't exist");
                else
                    logger.LogInformation("groupService:group with Id" + groupIn.Id + "has been updated successfully");
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }

        /// <summary>
        /// remove group object with the given id from db
        /// </summary>
        /// <param name="id">id of the group to remove</param>
        /// <returns>true if deleted</returns>
        public bool RemoveById(string id,string groupNumber)
        {
            logger.LogInformation("groupService:deleting a group profile with id : " + id);
            try
            {
                bool res = DbContext.RemoveById<Group>("group", id);
                if (res)
                {
                    logger.LogInformation("groupService:a group profile with id : " + id + "has been deleted successfully");
                    res = DbContext.RemoveByFilter<Group>("student", "group", groupNumber) && _timeTableService.RemoveById(groupNumber);
                    if (res)
                    {
                        logger.LogInformation("groupService:a group timetable and all students of this group has been deleted successfully");
                    }
                    else
                    {
                        logger.LogError("Cannot remove group's timeTable or group's students");
                    }
                }
                else
                {
                    logger.LogError("groupService:group with Id: " + id + " doesn't exist");
                }
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("GroupService:got error : " + e);
                throw e;
            }
        }
    }
}
