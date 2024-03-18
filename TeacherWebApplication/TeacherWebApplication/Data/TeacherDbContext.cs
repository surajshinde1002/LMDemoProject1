using Microsoft.EntityFrameworkCore;
using TeacherWebApplication.Models.EntityModels;

namespace TeacherWebApplication.Data
{
    public class TeacherDbContext : DbContext
    {
        public TeacherDbContext(DbContextOptions<TeacherDbContext> options) : base(options) 
        { 

        } 

        public DbSet<Teacher> TeacherTable { get; set; }
    }
}
