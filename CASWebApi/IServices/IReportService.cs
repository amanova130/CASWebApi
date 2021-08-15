using CASWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IReportService
    {
        public List<Average> GetAvgOfFacultiesByCourse(string courseName, string facId);

    }
}
