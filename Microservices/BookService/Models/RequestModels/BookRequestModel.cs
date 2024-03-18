using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookService.Models.RequestModels
{
    public class BookRequestModel
    {

        [Required(ErrorMessage = "Title cannot be empty.")]
        [Column("Title"), StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author name cannot be empty.")]
        [Column("Author"), StringLength(200)]
        public string Author { get; set; } = string.Empty;

        public double price { get; set; }

        [Required(ErrorMessage = "Genre field cannot be empty")]
        [StringLength(30)]
        public string Genre { get; set; } = string.Empty;

        public int Pages { get; set; }
    }
}
