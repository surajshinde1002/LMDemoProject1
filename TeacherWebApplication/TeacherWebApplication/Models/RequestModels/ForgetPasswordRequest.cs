using System.ComponentModel.DataAnnotations;

namespace TeacherWebApplication.Models.RequestModels
{
    public class ForgetPasswordRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Length must be less than 50 characters")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;
    }
}
