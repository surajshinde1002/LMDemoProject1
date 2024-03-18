namespace TeacherWebApplication.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public int Standard { get; set; }
    }
}
