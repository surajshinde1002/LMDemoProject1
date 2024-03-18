namespace UserController.Models.ResponseModel
{
    public class TokenResponseModel
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }

        public int Standard {  get; set; }

        public DateOnly DOB { get; set; }
        public string Email { get; set; }
    }
}
