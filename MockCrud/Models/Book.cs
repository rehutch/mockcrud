using System;
using System.Collections.Generic;

#nullable disable

namespace MockCrud.Models
{
    public partial class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public bool CheckedOut { get; set; }
    }
}