﻿using CASWebApi.IServices;
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
            return DbContext.GetById<ExtendedLink>("extended_links", linkId);
        }
        /// <summary>
        /// get all courses from db
        /// </summary>
        /// <returns></returns>
        public List<ExtendedLink> GetAll()
        {
            return DbContext.GetAll<ExtendedLink>("extended_links");

        }

        /// <summary>
        /// create new course object in db
        /// </summary>
        /// <param name="link"></param>
        /// <returns>true if successed</returns>
        public bool Create(ExtendedLink link)
        {
            link.Id = ObjectId.GenerateNewId().ToString();
            bool res = DbContext.Insert<ExtendedLink>("extended_links", link);
            return res;
        }

        /// <summary>
        /// edit an existing course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="linkIn"></param>
        /// <returns>true if successed</returns>
        public bool Update(string id, ExtendedLink linkIn)
        {
            return DbContext.Update<ExtendedLink>("extended_links", id, linkIn);
        }



        /// <summary>
        /// remove course from db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if removed successfully</returns>
        public bool RemoveById(string id)
        {
            bool res = DbContext.RemoveById<ExtendedLink>("extended_links", id);
            return res;
        }
    }
}
