using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Models
{
    public class Group
    {
        public string GroupId { get; set; }
        public string GroupNumber { get; set; }
        public int NumberOfStudent { get; set; }
        public int AcademicYear { get; set; }
        public int Semester { get; set; }

    }
}
