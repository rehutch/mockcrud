using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockCrud.Models;

namespace MockCrud.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BooksContext context)
        {
            context.Database.EnsureCreated();

            if (context.Books.Any()) return;

            var books = new Book[]
            {
                new() {BookId = 1, Title = "The Great Gatsby", Genre = "Fiction"},
                new() {BookId = 2, Title = "The Russian", Genre = "Non-Fiction"}
            };

            foreach (var book in books) context.Books.Add(book);
            context.SaveChanges();
        }
    }
}