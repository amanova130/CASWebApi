using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
    public interface IHolidayService
    {
        Holiday GetById(string holidayId);
        List<Holiday> GetAll();
        bool Create(Holiday holiday);
        bool Update(string id, Holiday holidayIn);
        bool RemoveById(string id);
    }
}
