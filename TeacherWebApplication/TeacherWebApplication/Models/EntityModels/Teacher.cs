using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeacherWebApplication.Models.EntityModels
{
    [Table("Err_TeacherTable")]
    public class Teacher
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //email address of user
        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Length must be less than 50 characters")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        // passsword for teacher
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password can't be longer than 30 characters")]
        //[DataType(DataType.Password, ErrorMessage = "Invalid Password")]
        public string Password { get; set; } = string.Empty;

        //phone number of teahcer
        [Required(ErrorMessage = "Mobile No is required")]
        [StringLength(15)]
        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Role can't be longer than 20 characters")]
        public string Role { get; set; } = "Teacher";

        [Required(ErrorMessage = "name is required")]
        [StringLength(40, ErrorMessage = "Must be less than 40 charecters")]
        public string Name { get; set; } = string.Empty;

        public int Standard { get; set; }
    }
}
