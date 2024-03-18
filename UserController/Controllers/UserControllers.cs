using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserController.data;
using UserController.Models.EntityModel;
using UserController.Models.RequestModel;
using UserController.Models.ResponseModel;
using UserController.Services;

namespace UserController.Controllers
{
    // Controller for managing user-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllers : ControllerBase
    {

        //Dependency Injection(Can be used after registering it in the program.cs)
        private readonly IUserServices _userServices;
        private IConfiguration _configuration;
        private UserDbContext _db;

        // Constructor injecting dependencies
        public UserControllers(IUserServices userServices, IConfiguration configuration, UserDbContext db)
        {
            _userServices = userServices;
            _configuration = configuration;
            _db = db;
        }

        // Endpoint for registering a new user
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterModel model)
        {
            //Validating the model 
            TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //If username or email is same then it will send bad request .
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.Username || u.Roll == model.Roll);
            if (existingUser != null)
            {
                if (existingUser.Email == model.Email) // Email already exists
                {
                    return BadRequest("Email already exists, try giving another email.");
                }
                else if (existingUser.Username == model.Username)  //Username already exists
                {
                    return BadRequest("Username already exists, try giving a different username.");
                }
                else if (existingUser.Roll == model.Roll)
                {
                    return BadRequest("Roll number already exists try giving another roll number .");
                }
            }


            var result = await _userServices.CreateUser(model);

            // If user not found
            if(result == null)
            {
                return BadRequest("Unable to register .");
            }

            //Sharing User Details
            return Ok(result);
            
        }


        // Endpoint for getting all users
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            // Get the list of users directly
            var result = await _userServices.GetAllUsers();

            // Return the result directly
            return Ok(result);
        }


        // Endpoint for getting only active users
        [HttpGet("GetAllActiveUser")]
        public async Task<IActionResult> GetAllActive()
        {
            // Get the result from services directly
            var result = await _userServices.GetAllActiveUsers();

            // Return the result directly
            return Ok(result);
        }


        // Endpoint for getting users by standard
        [HttpGet("GetByStd/{std}")]
        public async Task<IActionResult> GetByStd(int std)
        {
            //Getting result from service
            var result = await _userServices.GetAllByStd(std);

            // Return the result directly
            return Ok(result);

        }


        // Endpoint for getting active users by standard
        [HttpGet("GetActiveByStd/{std}")]
        public async Task<IActionResult> GetActiveByStd(int std)
        {
            // Getting result from service
            var result = await _userServices.GetActiveStdentsByStd(std);

            // Return the result directly
            return Ok(result);
        }

        // Endpoint for getting user by ID
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Getting result from service
            var result = await _userServices.GetById(id);

            // Return the result directly
            return Ok(result);
        }


        // Endpoint for updating user information
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update(User model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service method to update the user
            var result = await _userServices.UpdateUser(model);

            // Check if the update was successful
            if (result == null)
            {
                return BadRequest("Failed to update."); // Update unsuccessful
            }

            return Ok(result); //Return updated user if the update successful
        }


        // Endpoint for deleting a user
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Status sts = new Status();
            // Call the service method to delete the user
            var result = await _userServices.Deleteuser(id);

            // Check if the delete was successful
            if (!result)
            {
                sts.statusCode = 0;
                sts.message = "Delete unsuccessful";
                return BadRequest(sts); // Bad request if the delete was unsuccessful
            }
            sts.statusCode = 1;
            sts.message = "Deleted Successfully";
            return Ok(sts); // Delete successful
        }

        [HttpPatch("Activate/{name}/{id}")]
        public async Task<IActionResult> Activation(string name, int id)
        {
            Status sts = new Status();

            bool flag = await _userServices.Activate(name, id);
            if (!flag)
            {
                sts.statusCode = 0;
                sts.message = "Activation unsuccessful";
                return BadRequest(sts);
            }
            sts.statusCode = 1;
            sts.message = "Activated Successfully";
            return Ok(sts);
        }


        // Endpoint for user login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //Validating the model
            //TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            //Finding the user with given email and password
            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                return BadRequest("Wrong credentials");
            }
            if (user.Password != model.Password)
            {
                return BadRequest("Wrong Password try again with the correct one .");
            }

            //Getting integer result from service
            var result = await _userServices.LoginUser(model);
            if (result == 2)//Password Doesn't match with the email
            {
                return Unauthorized("User is not active .");
            }
            else if (result == 1)//Email and Password Matched
            {
                string token = GenerateToken(user);

                return Ok(new TokenResponseModel
                {
                    Token = token,
                    Username = user.Username,
                    Standard = user.Standard,
                    Id = user.Id,
                    Email = user.Email
                });
            }
            //No user found with given email and password
            return BadRequest("Wrong Credentials");


        }

        // Method to generate authentication token
        [NonAction]
        private string GenerateToken(User user)
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
                new Claim(JwtRegisteredClaimNames.GivenName,user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
