using BookService.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace BookService.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

    }
}
