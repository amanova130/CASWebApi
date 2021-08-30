using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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
        IGroupService _groupService;
        IStudentService _studentService;
        ITeacherService _teacherService;


        public ReportService(IDbSettings settings, IFacultyService facultyService, IGroupService groupService,IStudentService studentService,
            IExamService examService, ITeacherService teacherService, ILogger<ReportService> logger,IStudExamService studExamService)
        {
            _facultyService = facultyService;
            this.logger = logger;
            DbContext = settings;
            _studExamService = studExamService;
            _examService = examService;
            _groupService = groupService;
            _studentService = studentService;
            _teacherService = teacherService;

        }

        public int setSemesterGrade(int currentGrade,string studentId, Exam exam)
        {
            for (int i = 0; i < exam.JoinedField.Length; i++)
            {
                if (exam.JoinedField[i].StudId == studentId && exam.Test_num == "A")
                {
                    if (currentGrade == 0)
                    {
                        currentGrade = exam.JoinedField[i].Grade;
                    }
                }
                else if (exam.JoinedField[i].StudId == studentId && exam.Test_num == "B")
                {
                    if (exam.JoinedField[i].Grade != 0)
                        currentGrade = exam.JoinedField[i].Grade;
                }
            }
            return currentGrade;
        }
        public int setCourseDuration(int grade1,int grade2,int grade3)
        {
            int duration = 0;
            if(grade1 > 0)
                duration++;
            if (grade2 > 0)
                duration++;
            if (grade3 > 0)
                duration++;
            return duration;
        }
            public List<Average> GetAvgByGroup(string groupName,string year)
            {
            List<Exam> semesterA, semesterB, semesterSummer;
            List<Average> studentsAverage = new List<Average>();
            try
            {
                var group = _groupService.GetGroupByName(groupName);
                var students = _studentService.GetAllStudentsByGroup(groupName);
                for (int i = 0; i < students.Count; i++)
                {
                    studentsAverage.Add(new Average(students[i].Id, students[i].First_name + " " + students[i].Last_name));
                }
                int finalGradeA = 0, finalGradeB = 0, finalGradeSummer = 0;
                int courseDuration = 0;
                double totalAvg = 0;

                for (int i = 0; i < group.courses.Length; i++)
                {
                    courseDuration = 0;
                    LookUpDetails filterDetails = BuildFilter(groupName, year, "A", group.courses[i]);
                    semesterA = DbContext.AggregateJoinDocuments<Exam>(filterDetails);

                    filterDetails = BuildFilter(groupName, year, "B", group.courses[i]);
                    semesterB = DbContext.AggregateJoinDocuments<Exam>(filterDetails);

                    filterDetails = BuildFilter(groupName, year, "C", group.courses[i]);
                    semesterSummer = DbContext.AggregateJoinDocuments<Exam>(filterDetails);
                    for (int j = 0; j < students.Count; j++)
                    {
                        // studentsAverage.Add(new Average(students[j].Id, students[j].First_name + students[j].Last_name, group.courses.Length));

                        totalAvg = 0;
                        courseDuration = finalGradeA = finalGradeB = finalGradeSummer = 0;
                        for (int k = 0; k < semesterA.Count; k++)
                        {
                            finalGradeA = setSemesterGrade(finalGradeA, students[j].Id, semesterA[k]);
                        }

                        for (int k = 0; k < semesterB.Count; k++)
                        {
                            finalGradeB = setSemesterGrade(finalGradeB, students[j].Id, semesterB[k]);
                        }

                        for (int k = 0; k < semesterSummer.Count; k++)
                        {
                            finalGradeSummer = setSemesterGrade(finalGradeSummer, students[j].Id, semesterSummer[k]);
                        }
                        courseDuration = setCourseDuration(finalGradeA, finalGradeB, finalGradeSummer);
                        totalAvg += finalGradeA + finalGradeB + finalGradeSummer;
                        if (totalAvg > 0)
                            totalAvg /= courseDuration;
                        for (int k = 0; k < studentsAverage.Count; k++)
                        {
                            if (studentsAverage[k].Id == students[j].Id)
                            {
                                studentsAverage[k].courseAvg.Add(new CourseAvg(group.courses[i], totalAvg));
                                break;
                            }
                        }

                    }
                }
                return studentsAverage;
            }
            catch(Exception e)
            {
                logger.LogError("ReportService:got error : " + e);
                throw e;
            }
        }

        public List<Average> GetAvgOfAllTeachers(string year)
        {
            double totalAvg = 0;
            int numOfGrades = 0;
            List<Average> teachersAverage = new List<Average>();
            List<Exam> teacherAvg = new List<Exam>();
            try
            {
                var teachers = _teacherService.GetAll();

                for (int i = 0; i < teachers.Count; i++)
                {
                    teachersAverage.Add(new Average(teachers[i].Id, teachers[i].First_name + ' ' + teachers[i].Last_name));

                    for (int j = 0; j < teachers[i].TeachesCourses.Length; j++)
                    {
                        totalAvg = 0;
                        numOfGrades = 0;
                        LookUpDetails filterDetails = BuildFilterByTeacher(teachers[i].Id, year, teachers[i].TeachesCourses[j]);
                        teacherAvg = DbContext.AggregateJoinDocuments<Exam>(filterDetails);
                        for (int k = 0; k < teacherAvg.Count; k++)
                        {
                            for (int l = 0; l < teacherAvg[k].JoinedField.Length; l++)
                            {
                                totalAvg += teacherAvg[k].JoinedField[l].Grade;
                                numOfGrades++;
                            }
                        }
                        if (numOfGrades != 0)
                            totalAvg /= numOfGrades;
                        teachersAverage[teachersAverage.Count - 1].courseAvg.Add(new CourseAvg(teachers[i].TeachesCourses[j], totalAvg));
                    }


                }
                return teachersAverage;
            }
            catch (Exception e)
            {
                logger.LogError("ReportService:got error : " + e);
                throw e;
            }


        }


        public LookUpDetails BuildFilter(string groupName, string year,string semester,string course)
        {
            LookUpDetails filterDetails = new LookUpDetails();
            filterDetails.CollectionName = "examination";
            filterDetails.CollectionNameFrom = "stud_exam";
            //  filterDetails.MatchField = new string[4] { "groupNum", "course", "semester", "year" };
            filterDetails.Match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                          {
                            {"group_num", groupName},
                            {"course",course },
                            {"semester",semester },
                            { "year",year },
                            {"status",true }
                          }
                    }
                };
            filterDetails.LocalField = "_id";
            filterDetails.ForeignField = "exam_id";
            filterDetails.JoinedField = "JoinedField";
            return filterDetails;
        }

        public LookUpDetails BuildFilterByTeacher(string teacherId, string year, string course)
        {
            LookUpDetails filterDetails = new LookUpDetails();
            filterDetails.CollectionName = "examination";
            filterDetails.CollectionNameFrom = "stud_exam";
            //  filterDetails.MatchField = new string[4] { "groupNum", "course", "semester", "year" };
            filterDetails.Match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                          {
                            {"teacher_id", teacherId},
                            {"course",course },
                            { "year",year },
                            {"status",true }
                          }
                    }
                };
            filterDetails.LocalField = "_id";
            filterDetails.ForeignField = "exam_id";
            filterDetails.JoinedField = "JoinedField";
            return filterDetails;
        }

    }
}
