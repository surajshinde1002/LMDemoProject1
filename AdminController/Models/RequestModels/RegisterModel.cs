using System.ComponentModel.DataAnnotations;

namespace AdminController.Models.RequestModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is Required")]
        [StringLength(50, ErrorMessage = "Username cannot be more than 50 characters")]
        public string Username { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Role is Required")]
        [StringLength(20, ErrorMessage = "Role cannot be more than 20 characters")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(200, ErrorMessage = "Email cannot be more than 100 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(2000, ErrorMessage = "Password cannot be more than 30 characters")]
        [DataType(DataType.Password,ErrorMessage ="Invalid Password")]
        public string Password { get; set; } = string.Empty;

    }
}
