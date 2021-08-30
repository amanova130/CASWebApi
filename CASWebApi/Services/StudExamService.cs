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
    public class StudExamService : IStudExamService
    {
        private readonly ILogger logger;

        IDbSettings DbContext;
        IGroupService _groupService;
        IUserService _userService;
        IStudentService _studentService;

        public StudExamService(IDbSettings settings, ILogger<StudExamService> logger, IUserService userService, IGroupService groupService, IStudentService studentService)
        {
            DbContext = settings;
            _groupService = groupService;
            _userService = userService;
            this.logger = logger;
            _studentService = studentService;
        }


        /// <summary>
        /// get student by given id
        /// </summary>
        /// <param name="studExamId"></param>
        /// <returns> student object by given id</returns>
        public StudExam GetById(string studExamId)
        {
            logger.LogInformation("StudExamService:Getting student by id");

            var student = DbContext.GetById<StudExam>("stud_exam", studExamId);
            if (student == null)
                logger.LogError("StudExamService:Cannot get a student with a StudExamId: " + studExamId);
            else
                logger.LogInformation("StudExamService:Fetched student data by id ");
            return student;

        }
        /// <summary>
        /// get all grades from studExam table by examId
        /// </summary>
        /// <param name="examID"></param>
        /// <returns></returns>
        public List<StudExam> GetListOfGradesByExamId(string examID)
        {
            logger.LogInformation("Getting list of studExam by examID");
            try
            {
                var listOfGrades = DbContext.GetListByFilter<StudExam>("stud_exam", "exam_id", examID);
                logger.LogInformation("StudExamService:Fetched all grades data by examId ");
                return listOfGrades;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
        }
        /// <summary>
        /// get all studeExam objects by examId
        /// </summary>
        /// <param name="examId"></param>
        /// <returns></returns>
        public List<StudExam> GetStudentsDetailByExamId(string examId)
        {
            LookUpDetails filterDetails = new LookUpDetails();
            filterDetails.CollectionName = "stud_exam";
            filterDetails.CollectionNameFrom = "student";
            filterDetails.MatchField = "exam_id";
            filterDetails.Match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"exam_id", examId}
                            }
                    }
                };
            filterDetails.LocalField = "stud_id";
            filterDetails.ForeignField = "_id";
            filterDetails.JoinedField = "JoinedField";

            try
            {
                var list = DbContext.AggregateJoinDocuments<StudExam>(filterDetails);
                return list;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
        }
        /// <summary>
        /// get all studExam objects by studentId and year
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="year"></param>
        /// <returns>list of studExam objects</returns>

        public List<StudExam> GetGradesByStudentIdAndYear(string studentId, string year)
        {
            LookUpDetails filterDetails = new LookUpDetails();
            filterDetails.CollectionName = "stud_exam";
            filterDetails.CollectionNameFrom = "examination";
            filterDetails.MatchField = "stud_id";
            filterDetails.LocalField = "exam_id";
            filterDetails.ForeignField = "_id";
            filterDetails.JoinedField = "JoinedField";
            filterDetails.Match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"stud_id", studentId},
                                {"year", year}
                            }
                    }
                };
            try
            {
                var list = DbContext.AggregateJoinDocuments<StudExam>(filterDetails);
                return list;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
}
        /// <summary>
        /// function to calculate average to student for every course by year
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="year"></param>
        /// <param name="groupNumber"></param>
        /// <returns>list of courseAvg objects</returns>
        public List<CourseAvg> GetGradesAverage(string studentId, string year, string groupNumber)
        {
            int finalGradeA = 0, finalGradeB = 0, finalGradeC = 0;
            int courseDuration = 0;
            double average = 0;
            List<CourseAvg> totalAvg = new List<CourseAvg>();
            try
            {
                var courses = _groupService.GetGroupByName(groupNumber).courses;
                if (courses.Length > 0)
                {
                    for (int i = 0; i < courses.Length; i++)
                    {
                        courseDuration = finalGradeA = finalGradeB = finalGradeC = 0;
                        average = 0;
                        var grades = GetSemiGradesByStudentIdAndYear(studentId, year, courses[i]);
                        if (grades.Count > 0)
                        {
                            for (int k = 0; k < grades.Count; k++)
                            {
                                if (grades[k].JoinedField[0].Semester == "A")
                                {
                                    finalGradeA = setGrade(grades[k].JoinedField[0].Test_num, grades[k].Grade, ref courseDuration, finalGradeA);
                                }
                                else if (grades[k].JoinedField[0].Semester == "B")
                                {
                                    finalGradeB += setGrade(grades[k].JoinedField[0].Test_num, grades[k].Grade, ref courseDuration, finalGradeB);
                                }
                                else if (grades[k].JoinedField[0].Semester == "C")
                                {
                                    finalGradeC += setGrade(grades[k].JoinedField[0].Test_num, grades[k].Grade, ref courseDuration, finalGradeC);
                                }
                            }
                            average += finalGradeA + finalGradeB + finalGradeC;
                        }
                        if (average > 0)
                        {
                            average /= courseDuration;
                            totalAvg.Add(new CourseAvg(courses[i], average));
                        }
                    }

                }
                return totalAvg;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
        }
        /// <summary>
        /// function to set final grade of course
        /// </summary>
        /// <param name="testNumber"></param>
        /// <param name="grade"></param>
        /// <param name="courseDuration"></param>
        /// <param name="finalGrade"></param>
        /// <returns></returns>
        public int setGrade(string testNumber, int grade, ref int courseDuration, int finalGrade)
        {
            if (testNumber == "A")
            {
                if(finalGrade == 0)
                    finalGrade = grade;
                courseDuration++;
            }
            else if (testNumber == "B")
            {
                if (grade != 0)
                    finalGrade = grade;
            }
            return finalGrade;
        }
        /// <summary>
        /// get list of grade details of student by years and course
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="year"></param>
        /// <param name="course"></param>
        /// <returns>list of gradeDetails object</returns>
        public List<GradeDetails> GetSemiGradesByStudentIdAndYear(string studentId, string year, string course)
        {
            LookUpDetails filterDetails = new LookUpDetails();
            filterDetails.CollectionName = "stud_exam";
            filterDetails.CollectionNameFrom = "examination";
            filterDetails.MatchField = "stud_id";
            filterDetails.LocalField = "exam_id";
            filterDetails.ForeignField = "_id";
            filterDetails.JoinedField = "JoinedField";
            
            filterDetails.Match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"stud_id", studentId},
                                 {"status", true},
                                {"year", year},
                                {"JoinedField.course", course},
                            }
                    }
                };
            var mergeObj = new BsonDocument("$merge", "output");
            var project = new BsonDocument { { "$project",  new BsonDocument
                            {
                                {"_id", 0},
                {"year", 1 },
                {"JoinedField.course", 1 },
                {"JoinedField.semester", 1 },
                {"JoinedField.test_num", 1 },
                {"grade", 1 }
                              
                            } } };
            try
            {
                var list = DbContext.AggregateWithProject<GradeDetails>(filterDetails, project);
                return list;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
        }
        /// <summary>
        /// function to create new studExam object
        /// </summary>
        /// <param name="studExam"></param>
        /// <returns></returns>
        public bool Create(StudExam studExam)
        {
            logger.LogInformation("got a new studExam profile in studexamService: " + studExam);
            try
            {
                bool res = DbContext.Insert<StudExam>("stud_exam", studExam);
                logger.LogInformation("A new studExam profile and his user profile added successfully and got in studExamService" + studExam);
                return res;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
}
        /// <summary>
        /// function to update studExam profile
        /// </summary>
        /// <param name="studExam"></param>
        /// <returns>true if updated,exception otherwise</returns>

        public bool Update(StudExam studExam)
        {
            logger.LogInformation("StudExamService:updating an existing student grade profile with id: " + studExam.Id);
            try
            {
                DbContext.Update<StudExam>("stud_exam", studExam.Id, studExam);
                    logger.LogInformation("StudExam:student with Id" + studExam.Id + "has been updated successfully");
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("StudExamService:got error : " + e);
                throw e;
            }
        }
    }
}
