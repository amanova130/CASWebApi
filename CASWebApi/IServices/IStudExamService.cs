using System;
using System.Collections.Generic;
using CASWebApi.Models.DbModels;
using System.Linq;
using System.Threading.Tasks;
using CASWebApi.Models;

namespace CASWebApi.IServices
{
    public interface IStudExamService
    {
        public StudExam GetById(string studExamId);
        public List<StudExam> GetStudentsDetailByExamId(string examId);
        public List<StudExam> GetGradesByStudentIdAndYear(string studentId, string year);
        public List<GradeDetails> GetSemiGradesByStudentIdAndYear(string studentId, string year, string semester);
        public List<CourseAvg> GetGradesAverage(string studentId, string year, string groupNumber);
        public bool Update(StudExam studExam);
        public bool Create(StudExam studExam);
    }
}
