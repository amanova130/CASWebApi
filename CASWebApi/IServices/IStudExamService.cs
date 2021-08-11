using System;
using System.Collections.Generic;
using CASWebApi.Models.DbModels;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IStudExamService
    {
        public List<StudExam> GetStudentsDetailByExamId(string examId);
    }
}
