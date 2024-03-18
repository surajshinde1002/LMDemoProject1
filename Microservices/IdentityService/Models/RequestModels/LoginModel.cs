using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.RequestModels
{
    public class LoginModel
    {

        [Required(ErrorMessage = "username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
