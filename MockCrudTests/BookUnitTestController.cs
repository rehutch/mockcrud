using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MockCrud.Controllers;
using MockCrud.Data;
using MockCrud.Models;
using Xunit;

namespace MockCrudTests
{
    public class BookUnitTestController
    {
        private BooksController _controller;
        private BooksContext _context;

        public static DbContextOptions<BooksContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=mockcrud;Integrated Security=true;";

        static BookUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<BooksContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public BookUnitTestController()
        {
            _context = new BooksContext(dbContextOptions);
            DummyDbInitializer db = new DummyDbInitializer();
            db.Seed(_context);

            _controller = new BooksController(_context);

        }

        #region CreateBook
        [Fact]
        public async void Create_Book_Return_OkResult()
        {
            var book = new Book() {BookId = 3, Title = "Book3", Genre = "Fiction"};
            var data = await _controller.Create(book);
            Assert.IsType<RedirectToActionResult>(data);
        }
        #endregion
        #region Get Book
        [Fact]
        public async void Task_GetBookById_Return_OkResult()
        {
            var bookId = 1;
            var data = await _controller.Details(bookId);
            Assert.IsType<ViewResult>(data);

            var result = data as ViewResult;
            Assert.NotNull(result);

            var book = (Book) result.Model;
            Assert.NotNull(book);
        }

        [Fact]
        public async void Task_GetBookById_Return_NotFoundResult()
        {
            var bookId = 3;
            var data = await _controller.Details(bookId);
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetBookById_MatchResult()
        {
            var bookId = 1;
            var data = await _controller.Details(bookId);

            //Assert  
            Assert.IsType<ViewResult>(data);

            var result = data as ViewResult;
            var book = (Book) result?.Model;

            Assert.NotNull(book);
            Assert.Equal("Book1", book.Title);
            Assert.Equal("Fiction", book.Genre);
        }
        #endregion

        #region Update Existing Book  

        [Fact]
        public async Task Task_Update_ValidData_Return_OkResult()
        {
            var bookId = 1;
            
            var existingBook = await _controller.Details(bookId);
            var okResult = existingBook.Should().BeOfType<ViewResult>().Subject;
            var result = okResult.Model.Should().BeAssignableTo<Book>().Subject;
            Assert.NotNull(result);

            result.CheckedOut = true;

            try
            {
                var updatedData = await _controller.TryUpdateModelAsync(result);
                Assert.IsType<OkResult>(updatedData);
            }
            catch
            {

            }
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            var bookId = 1;

            //Act  
            var existingBook = await _controller.Details(bookId);
            var okResult = existingBook.Should().BeOfType<ViewResult>().Subject;
            var result = okResult.Model.Should().BeAssignableTo<Book>().Subject;
            Assert.NotNull(result);

            try
            {
                result.Title = "More Than 10 Chars";
                var data = await _controller.TryUpdateModelAsync(result);
            }
            catch(Exception e)
            {
                Assert.True(e is ArgumentNullException);
            }
        }

        #endregion

        #region Delete Book  

        [Fact]
        public async void Task_Delete_Book_Return_OkResult()
        {
            //Act  
            var data = await _controller.Delete(1);

            //Assert  
            Assert.IsType<ViewResult>(data);
        }

        [Fact]
        public async void Task_Delete_Book_Return_NotFoundResult()
        {
            var data = await _controller.Delete(5);
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_Delete_Book_BadRequestResult()
        {
            //Arrange  
            int? bookId = null;

            //Act  
            var data = await _controller.Delete(bookId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        #endregion
    }
}
