using System.ComponentModel.DataAnnotations;

namespace UserController.Models.ResponseModel
{
    public class RegisterResponseModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }


        public string Email { get; set; }

        public int Standard { get; set; }


        public int Roll { get; set; }

        public DateOnly? DOB { get; set; }

    }
}
