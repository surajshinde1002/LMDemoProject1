using AdminController.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace AdminController.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
    }
}
