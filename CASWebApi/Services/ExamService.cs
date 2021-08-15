﻿using CASWebApi.IServices;
using CASWebApi.Models;
using CASWebApi.Models.DbModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class ExamService : IExamService
    {
        IDbSettings DbContext;
        IStudentService _studentService;
        IStudExamService _studExamService;


        public ExamService(IDbSettings settings, IStudentService studentService, IStudExamService studExamService)
        {
            _studentService = studentService;
            _studExamService= studExamService;
            DbContext = settings;
        }
       
        /// <summary>
        /// get examination object from db by id
        /// </summary>
        /// <param name="examId">id of the object that we need</param>
        /// <returns>found examination object</returns>
        public Exam GetById(string examId)
        {
            return DbContext.GetById<Exam>("examination", examId);
        }

        /// <summary>
        /// get list of all exams in db
        /// </summary>
        /// <returns>list of examinations</returns>
        public List<Exam> GetAll()
        {
            return DbContext.GetAll<Exam>("examination");

        }

        /// <summary>
        /// a function to add new examination to db
        /// </summary>
        /// <param name="exam">exam object to add</param>
        /// <returns>true if added successfully</returns>
        public bool Create(Exam exam)
        {
            exam.Id = ObjectId.GenerateNewId().ToString();
            var students = _studentService.GetAllStudentsByGroup(exam.Group_num);
            StudExam studExam = new StudExam();
            if (students != null)
            {
                for (int i=0;i<students.Count;i++)
                {
                    studExam.Id = ObjectId.GenerateNewId().ToString();
                    studExam.StudId = students[i].Id;
                    studExam.ExamId = exam.Id;
                    studExam.Year = exam.Year;
                    studExam.Grade = 0;
                    studExam.Status = true;
                   
                    if (!_studExamService.Create(studExam))
                        return false;
                }
            }
           bool res= DbContext.Insert<Exam>("examination", exam);
            return res;
        }

        public List<Exam> GetExamByGroup(string groupNumber, string semester, string year, string testNo)
        {
            var examList = DbContext.GetListByFilter<Exam>("examination", "group_num", groupNumber);
            if (examList != null)
            {
                var filteredExamList = examList.FindAll(exam => exam.Year == year.Trim() && exam.Semester == semester.Trim() && exam.Test_num == testNo.Trim());
                return filteredExamList;
            }
            else
                return null;
        }

        /// <summary>
        /// a function to update existing examination object
        /// </summary>
        /// <param name="id">id of exam to edit</param>
        /// <param name="examIn">exam object to replace the old one</param>
        /// <returns>true if updated</returns>

        public bool Update(string id, Exam examIn) =>
          DbContext.Update<Exam>("examination", id, examIn);

       
        /// <summary>
        /// remove examination by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id) =>
            DbContext.RemoveById<Exam>("examination", id);
    }
}
