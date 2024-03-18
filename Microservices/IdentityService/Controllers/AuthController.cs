using IdentityService.Data;
using IdentityService.Models.EntityModels;
using IdentityService.Models.RequestModels;
using IdentityService.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        IdentityDbContext _db;
        private IConfiguration _configuration;
        public AuthController(IdentityDbContext dbContext, IConfiguration configuration ) 
        { 
            _db = dbContext;
            _configuration = configuration;
        }



        [HttpGet]
        public List<UserInfo> GetAll()
        {
            return  _db.Users.ToList();
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            TryValidateModel(model);    
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserInfo userInfo = new UserInfo()
            {
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = "User",
                Username = model.Username
            };
            _db.Users.Add(userInfo);
            await _db.SaveChangesAsync();
            return Created("", new RegisterResponseModel
            {
                FirstName =userInfo.FirstName,
                LastName =userInfo.LastName, 
                Role = userInfo.Role, 
                Email = userInfo.Email,
                Username = userInfo.Username, 
                Id = userInfo.Id
            });

        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]
        public ActionResult Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return Unauthorized(new {message = "Invalid username or password."});
            }
            var user = _db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if(user == null)
            {
                return Unauthorized("Invalid Username or password");
            }
            else
            {
                string token = GenerateToken(user);
                return Ok(new TokenResponseModel
                { 
                    Token = token,
                    Email = user.Email,
                    FirstName = user.FirstName,

                    LastName = user.LastName,
                });
            }
        }

        [NonAction]
        private string GenerateToken(UserInfo user)
        {
            var issuer = _configuration.GetValue<string>("jwt:Issuer");
            var audience = _configuration.GetValue<string>("jwt:Audience");
            var secretkey = _configuration.GetValue<string>("jwt:SecretKey");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(JwtRegisteredClaimNames.GivenName,user.FirstName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
