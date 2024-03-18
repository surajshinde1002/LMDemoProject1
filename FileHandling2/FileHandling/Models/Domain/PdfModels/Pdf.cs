using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FileHandling.Models.Domain.Pdf
{
    [Table("Err_PdfTable")]
    public class Pdf
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Required]
        public string? ResourceName { get; set; }

        [DefaultValue("PDF")]
        public string? ResourceCategory { get; set; }

        public string Subject { get; set; }

        [Required]
        public string ? ResourceDescription { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        [Required]
        public int? Standard {  get; set; }

        public string? ResourcePdf { get; set; }

        [DefaultValue(2)]
        public int Flag { get; set; } = 2;

        [NotMapped]
        public IFormFile PdfFile { get; set; }


    }
}
