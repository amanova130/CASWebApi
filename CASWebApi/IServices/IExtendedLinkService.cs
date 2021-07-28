using CASWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.IServices
{
   public interface IExtendedLinkService
    {
        ExtendedLink GetById(string linkId);
        List<ExtendedLink> GetAll();
        bool Create(ExtendedLink link);
        bool Update(string id, ExtendedLink linkIn);
        bool RemoveById(string id);
    }
}
