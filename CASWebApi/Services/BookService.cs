using CASWebApi.Models.DbModels;
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

        public BookService(IDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Books>(nameof(Books));
        }

        public List<Books> Get() =>
            _books.Find(book => true).ToList();

        public Books Get(string id) =>
            _books.Find<Books>(book => book.Id == id).FirstOrDefault();

        public Books Create(Books book)
        {

            book.Id= ObjectId.GenerateNewId().ToString();
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Books bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Books bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}
