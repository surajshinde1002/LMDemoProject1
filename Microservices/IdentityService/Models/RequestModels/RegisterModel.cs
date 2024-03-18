using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Models.RequestModels
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Username is Required")]
        [StringLength(50, ErrorMessage = "Username cannot be more than 50 characters")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(30, ErrorMessage = "Password cannot be more than 30 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is Required")]
        [StringLength(20, ErrorMessage = "Role cannot be more than 20 characters")]
        public string Role { get; set; }

        [Required(ErrorMessage = "FirstName is Required")]
        [StringLength(40, ErrorMessage = "Firstname cannot be more than 40 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is Required")]
        [StringLength(40, ErrorMessage = "LastName cannot be more than 40 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(20, ErrorMessage = "Email cannot be more than 100 characters")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email format")]
        public string Email { get; set; } = string.Empty;
    }
}
