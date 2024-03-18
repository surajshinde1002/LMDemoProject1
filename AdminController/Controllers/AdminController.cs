using AdminController.Models.EntityModels;
using AdminController.Models.RequestModels;
using AdminController.Models.ResponseModels;
using AdminController.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        private IConfiguration _configuration;
        public AdminController(IAdminServices adminServices, IConfiguration configuration)
        {
            _adminServices = adminServices;
            _configuration = configuration;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _adminServices.GetAllAdmins();
            if(result == null)
            {
                return BadRequest("The database is empty.");
            }
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminServices.CreateAdmin(model);

            if(result == null)
            {
                return BadRequest("Failed to create Admin");
            }
            return Ok(result);
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            var result = await _adminServices.GetAdminById(id);
            if(result == null)
            {
                return BadRequest("User not Found");
            }
            return Ok(result);
        }


        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> Update(Admin model)
        {
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _adminServices.UpdateAdmin(model);

            if(result == null)
            {
                return BadRequest("Failed to update.");
            }
            return Ok(result); 
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _adminServices.DeleteAdmin(id);
            if(!result)
            {
               return  BadRequest("User not found with the given id.");
            }
            return Ok();
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]

        public async Task<IActionResult> Login(LoginModel model)
        {
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return Unauthorized(ModelState);
            }

            var result = await _adminServices.LoginAdmin(model);
            if(result == null)
            {
                return Unauthorized("Invalid Username and Password");
            }

            if (model.Email == result.Email)
            {
                if (model.Password != result.Password)
                {
                    return Unauthorized("Wrong Password");
                }

            }
            else if (model.Password == result.Password)
            {
                if (model.Email != result.Email)
                {
                    return Unauthorized("Wrong Email");
                }
            }
            
                string token = GenerateToken(result);

                return Ok(new TokenResponseModel 
                { 
                    Token = token,
                    Username = result.Username,
                    Id = result.Id,
                    Email = result.Email
                });
            
            

          
        }


        [NonAction]
        private string GenerateToken(Admin admin)
        {
            var issuer = _configuration.GetValue<string>("jwt:Issuer");
            var audience = _configuration.GetValue<string>("jwt:Audience");
            var secretkey = _configuration.GetValue<string>("jwt:SecretKey");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name,admin.Username),
                new Claim(JwtRegisteredClaimNames.Email,admin.Email),
                new Claim(JwtRegisteredClaimNames.Sub,admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,admin.Role),
                new Claim(JwtRegisteredClaimNames.GivenName,admin.Username)
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
