using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockCrud.Data;
using MockCrud.Models;

namespace MockCrudTests
{
    class DummyDbInitializer
    {
        public void Seed(BooksContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var books = new Book[]
            {
                new() {BookId=1,Title = "Book1", Genre = "Fiction"},
                new() {BookId=2,Title = "Book2", Genre = "Fiction"}
            };

            foreach (var book in books) context.Books.Add(book);
            context.SaveChanges();
        }
    }
}
