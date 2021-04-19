using CASWebApi.IServices;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CASWebApi.Models.DbModels.BookApi;

namespace CASWebApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Books> _books;
        IDbSettings DbContext;

        public BookService(IDbSettings settings)
        {
            DbContext = settings;
            _books = DbContext.database.GetCollection<Books>(nameof(Books));
        }

        public List<Books> Get()
        {
           return DbContext.GetAll<Books>("Books");
        }


        public Books Get(string id) =>
           DbContext.GetById<Books>("Books", id);
      

        public Books Create(Books book)
        {
            book.Id= ObjectId.GenerateNewId().ToString();
            DbContext.Insert<Books>("Books", book);
            return book;
        }

        public void Update(string id, Books bookIn) =>
            DbContext.Update<Books>("Books", id, bookIn);

        public void Remove(Books bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public bool RemoveById(string id) =>
            DbContext.RemoveById<Books>("Books", id);
    }
}
