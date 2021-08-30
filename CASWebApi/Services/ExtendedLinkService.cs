using CASWebApi.IServices;
using CASWebApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASWebApi.Services
{
    public class ExtendedLinkService : IExtendedLinkService
    {
        IDbSettings DbContext;

        public ExtendedLinkService(IDbSettings settings)
        {
            DbContext = settings;
        }
        /// <summary>
        /// get course object by id
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns>found course object</returns>
        public ExtendedLink GetById(string linkId)
        {
            try
            {
                return DbContext.GetById<ExtendedLink>("extended_links", linkId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// get all courses from db
        /// </summary>
        /// <returns></returns>
        public List<ExtendedLink> GetAll()
        {
            try
            { 
            return DbContext.GetAll<ExtendedLink>("extended_links");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// create new course object in db
        /// </summary>
        /// <param name="link"></param>
        /// <returns>true if successed</returns>
        public bool Create(ExtendedLink link)
        {
            try
            {
                link.Id = ObjectId.GenerateNewId().ToString();
                bool res = DbContext.Insert<ExtendedLink>("extended_links", link);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// edit an existing course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="linkIn"></param>
        /// <returns>true if successed</returns>
        public bool Update(string id, ExtendedLink linkIn)
        {
            try
            {
                return DbContext.Update<ExtendedLink>("extended_links", id, linkIn);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            try
            {
                return DbContext.RemoveById<ExtendedLink>("extended_links", id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
