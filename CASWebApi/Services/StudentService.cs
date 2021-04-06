using CASWebApi.IServices;
using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class StudentService : IStudentService
    {
        List<Student> _students = new List<Student>();

        public StudentService()
        {
            for(int i=1; i<=9; i++)
            {
                _students.Add(new Student()
                {
                    StudentId = i,
                    First_name = "Student" + i,
                    Last_name = "Stu"+i
                });

            }
        }
        public List<Student> Delete(int studentId)
        {
            _students.RemoveAll(x => x.StudentId == studentId);
            return _students;
            
        }

        public Student Get(int studentId)
        {
            return _students.SingleOrDefault(x => x.StudentId == studentId);
        }

        public List<Student> Gets()
        {
            return _students;

        }

        public List<Student> Save(Student student)
        {
            _students.Add(student);
            return _students;
        }
    }
}
