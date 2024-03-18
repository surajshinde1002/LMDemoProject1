using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FileHandling.Models.Domain.VideoModels
{
    [Table("Err_VideoTable")]
    public class Video
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Required]
        public string? ResourceName { get; set; }

        public string? ResourceCategory { get; set; }

        public string Subject { get; set; }

        [Required]
        public string? ResourceDescription { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        [Required]
        public int? Standard {  get; set; }

        public string? ResourceVideo { get; set; }

        [DefaultValue(2)]
        public int Flag { get; set; }

        [NotMapped]
        public IFormFile VideoFile { get; set; }
    }
}
