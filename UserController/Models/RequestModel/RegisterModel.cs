using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserController.Models.RequestModel
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be more than 50 characters.")]
        public string Username { get; set; } 


        [Required(ErrorMessage = "Email is Required")]
        [StringLength(200, ErrorMessage = "Email cannot be more than 100 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email format")]
        public string Email { get; set; } 


        [Required(ErrorMessage = "Password is Required")]
        [StringLength(200, ErrorMessage = "Password cannot be more than 30 characters")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Standard should be given")]
        public int Standard { get; set; }

        [Required(ErrorMessage = "Roll number should be given")]

        public int Roll { get; set; }
        
        public DateOnly? DOB { get; set; }

   
    }
}
