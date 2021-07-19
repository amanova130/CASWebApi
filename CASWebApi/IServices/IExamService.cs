using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
   public interface IExamService
    {
        Exam GetById(string examId);
        List<Exam> GetAll();
        bool Create(Exam exam);
        bool Update(string id, Exam examIn);
        bool RemoveById(string id);
    }
}
