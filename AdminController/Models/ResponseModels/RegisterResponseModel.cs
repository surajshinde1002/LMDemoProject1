using System.ComponentModel.DataAnnotations;

namespace AdminController.Models.ResponseModels
{
    public class RegisterResponseModel
    {
        public int Id { get; set; } 
         public string Username { get; set; } = string.Empty;


         public string Role { get; set; }

          public string Email { get; set; } = string.Empty;

    }
}
