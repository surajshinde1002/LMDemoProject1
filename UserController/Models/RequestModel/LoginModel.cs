using System.ComponentModel.DataAnnotations;

namespace UserController.Models.RequestModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "username is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Style")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
