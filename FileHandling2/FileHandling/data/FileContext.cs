using FileHandling.Models.Domain.ImageModels;
using FileHandling.Models.Domain.Pdf;
using FileHandling.Models.Domain.VideoModels;
using Microsoft.EntityFrameworkCore;

namespace FileHandling.data
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {

        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Pdf> Pdfs { get; set; }
    }
}
