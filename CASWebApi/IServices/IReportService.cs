using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IReportService
    {
        public List<Average> GetAvgByGroup(string groupName, string year);
        public LookUpDetails BuildFilter(string groupName, string year, string semester, string course);
        public int setSemesterGrade(int currentGrade,string studentId, Exam exam);
        public int setCourseDuration(int grade1, int grade2, int grade3);
        public List<Average> GetAvgOfAllTeachers(string year);
        public LookUpDetails BuildFilterByTeacher(string teacherId, string year, string course);



    }
}
