﻿namespace IdentityService.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } =string.Empty;
    }
}
