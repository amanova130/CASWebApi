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
    public class FacultyService : IFacultyService
    {
        IDbSettings DbContext;

        public FacultyService(IDbSettings settings)
        {
            DbContext = settings;
        }
        //public List<Student> Delete(int studentId)
        //{
        //    _students.RemoveAll(x => x.StudentId == studentId);
        //    return _students;

        //}

        public Faculty GetById(string facultyId)
        {
            return DbContext.GetById<Faculty>("faculty", facultyId);
        }

        public List<Faculty> GetAll()
        {
            return DbContext.GetAll<Faculty>("faculty");

        }

        public bool Create(Faculty faculty)
        {
            faculty.Id = ObjectId.GenerateNewId().ToString();
            bool res=DbContext.Insert<Faculty>("faculty", faculty);
            return res;
        }

        public void Update(string id, Faculty facultyIn) =>
          DbContext.Update<Faculty>("faculty", id, facultyIn);

        // public void Remove(Student studentIn) =>
        //_books.DeleteOne(book => book.Id == studentIn.Id);

        public bool RemoveById(string id)
        {
            bool res=DbContext.RemoveById<Faculty>("faculty", id);
           if(res)
             DbContext.RemoveField<Faculty>("group","fac_id",id);
            return res;
            
        }
        }
    }
