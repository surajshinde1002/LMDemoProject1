using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Models.ResponseModels
{
    public class RegisterResponseModel
    {

        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string Role { get; set; }

        public string FirstName { get; set; } = string.Empty;     
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
