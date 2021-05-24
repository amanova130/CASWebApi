using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class TeacherService : ITeacherService
    {
        IDbSettings DbContext;

        public TeacherService(IDbSettings settings)
        {
            DbContext = settings;
        }
 

        public Teacher GetById(string teacherId)
        {
            return DbContext.GetById<Teacher>("teachers", teacherId);
        }

        public List<Teacher> GetAll()
        {
            return DbContext.GetAll<Teacher>("teachers");

        }

        public bool Create(Teacher teacher)
        {
            
            bool res = DbContext.Insert<Teacher>("teachers", teacher);
            return res;
        }

        public void Update(string id, Teacher teacherIn) =>
          DbContext.Update<Teacher>("teachers", id, teacherIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Teacher>("teachers", id);
    }
}
