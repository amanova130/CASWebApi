using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger logger;
        IDbSettings DbContext;
        IFacultyService _facultyService;
        IStudExamService _studExamService;
        IExamService _examService;

        public ReportService(IDbSettings settings, IFacultyService facultyService, IExamService examService, ILogger<ReportService> logger,IStudExamService studExamService)
        {
            _facultyService = facultyService;
            this.logger = logger;
            DbContext = settings;
            _studExamService = studExamService;
            _examService = examService;
        }

        public List<Average> GetAvgOfFacultiesByCourse(string courseName, string facId)
        {
            var faculty = DbContext.GetById<Faculty>("faculty", facId);
            Average newAverage = new Average();
            List<Exam> examsByGroup = new List<Exam>();
            List<Average> averageList = new List<Average>();
            double avg = 0, totalAvg = 0;
            var groupsByFaculty = DbContext.GetListByFilter<Group>("group", "fac_name", faculty.FacultyName);
            var filteredGroups = new List<Group>();
            for (int i = 0; i < groupsByFaculty.Count; i++)
            {
                if (groupsByFaculty[i].courses.Contains<string>(courseName))
                    filteredGroups.Add(groupsByFaculty[i]);
            }
            for (int i = 0; i < filteredGroups.Count; i++)
            {
                var students = DbContext.GetListByFilter<Student>("student", "group", filteredGroups[i].GroupNumber);
                for (int j = 0; j < students.Count; j++)
                {

                    avg = 0;

                    //for (int k = 0; k < students[j].Grades.Length; k++)
                    //{
                    //    avg += students[j].Grades[k].Grade;
                    //}
                    //avg /= students[j].Grades.Length;
                    //totalAvg += avg;
                }



                totalAvg /= students.Count;

                newAverage.name = groupsByFaculty[i].GroupNumber;
                newAverage.avg = totalAvg;

                averageList.Add(newAverage);
            }

            return averageList;
        }
    }
}
