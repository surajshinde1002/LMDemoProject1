using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookService.Models.EntityModels
{
    [Table("Books")]
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title cannot be empty.")]
        [Column("Title"),StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage ="Author name cannot be empty.")]
        [Column("Author"),StringLength(200)]
        public string Author { get; set; }= string.Empty;

        [Column("Price")]
        public double Price { get; set; }

        [Required(ErrorMessage ="Genre field cannot be empty")]
        [StringLength(30)]
        public string Genre { get; set; } = string.Empty;

        public int Pages { get; set; }
    }
}
