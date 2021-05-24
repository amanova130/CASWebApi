using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class AdminService : IAdminService
    {
        IDbSettings DbContext;

        public AdminService(IDbSettings settings)
        {
            DbContext = settings;
        }
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public Admin GetById(string adminId)
        {
            return DbContext.GetById<Admin>("admin", adminId);
        }

        public List<Admin> GetAll()
        {
            return DbContext.GetAll<Admin>("admin");

        }

        public bool Create(Admin admin)
        {
            bool res=DbContext.Insert<Admin>("admin", admin);
            return res;
        }

        public bool Update(string id, Admin adminIn) =>
         DbContext.Update<Admin>("admin", id, adminIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Admin>("admin", id);
    }
}
