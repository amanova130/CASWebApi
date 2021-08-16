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
        public List<StudExam> GetListOfGradesByExamId(string examID)
        {
            logger.LogInformation("Getting list of studExam by examID");
            var listOfGrades = DbContext.GetListByFilter<StudExam>("stud_exam", "exam_id", examID);
            if (listOfGrades == null)
                logger.LogError("StudExamService:Cannot get a grades with a examID: " + examID);
            else
                logger.LogInformation("StudExamService:Fetched all grades data by examId ");
            return listOfGrades;
        }

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

             var list = DbContext.AggregateJoinDocuments<StudExam>(filterDetails);
            return list;
        }

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
            var list = DbContext.AggregateJoinDocuments<StudExam>(filterDetails);
            return list;
        }

        public List<GradeDetails> GetSemiGradesByStudentIdAndYear(string studentId, string year, string semester)
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
                                {"year", year},
                                {"JoinedField.semester", semester},
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
            var list = DbContext.AggregateWithProject<GradeDetails>(filterDetails, project);
            return list;
        }
        public bool Create(StudExam studExam)
        {
            logger.LogInformation("got a new studExam profile in studexamService: " + studExam);
            bool res = DbContext.Insert<StudExam>("stud_exam", studExam);
            if (res)
            {
                logger.LogInformation("A new studExam profile and his user profile added successfully and got in studExamService" + studExam);
            }
            else
                logger.LogError("studExamService:Cannot create a studExam, duplicated id or wrong format");
            return res;
        }

        public bool Update(StudExam studExam)
        {
            logger.LogInformation("StudExamService:updating an existing student grade profile with id: " + studExam.Id);

            bool res = DbContext.Update<StudExam>("stud_exam", studExam.Id, studExam);
            if (!res)
                logger.LogError("StudExamService:student with Id: " + studExam.Id + " doesn't exist");
            else
                logger.LogInformation("StudExam:student with Id" + studExam.Id + "has been updated successfully");
            return res;
        }
    }
}
