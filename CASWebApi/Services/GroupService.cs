using CASWebApi.IServices;
using CASWebApi.Models;
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
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public Group GetById(string groupId)
        {
            return DbContext.GetById<Group>("group", groupId);
        }

        public List<Group> GetAll()
        {
            return DbContext.GetAll<Group>("group");

        }

        public Group Create(Group group)
        {
            //book.Id = ObjectId.GenerateNewId().ToString();
            DbContext.Insert<Group>("group", group);
            return group;
        }

        public void Update(string id, Group groupIn) =>
          DbContext.Update<Group>("group", id, groupIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Group>("group", id);
    }
}
