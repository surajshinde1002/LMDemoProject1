using Microsoft.EntityFrameworkCore;
using UserController.Models.EntityModel;

namespace UserController.data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
