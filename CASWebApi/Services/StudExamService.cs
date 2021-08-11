using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using Microsoft.Extensions.Logging;
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
            filterDetails.Match = examId;
            filterDetails.LocalField = "stud_id";
            filterDetails.ForeignField = "_id";
            filterDetails.JoinedField = "JoinedField";

             var list = DbContext.AggregateJoinDocuments<StudExam>(filterDetails);
            return list;
        }
    }
}
