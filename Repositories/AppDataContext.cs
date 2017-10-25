using LibraryAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Loan> Loans { get; set; }

        public DbSet<Review> Reviews { get; set; }
    }
}    