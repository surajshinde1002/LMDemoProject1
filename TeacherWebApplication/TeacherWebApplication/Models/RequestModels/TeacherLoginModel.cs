using System.ComponentModel.DataAnnotations;

namespace TeacherWebApplication.Models.RequestModels
{
    public class TeacherLoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Password")]
        public string Password { get; set; } = string.Empty;
    }
}
