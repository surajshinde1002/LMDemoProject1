using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeacherWebApplication.Data;
using TeacherWebApplication.Models.EntityModels;
using TeacherWebApplication.Models.RequestModels;
using TeacherWebApplication.Models.ResponseModels;
using TeacherWebApplication.Services;

namespace TeacherWebApplication.Controllers
{
    // Controller for managing Teacher-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        //Dependency Injection(Can be used after registering it in the program.cs)
        private readonly ITeacherService _ts;
        private readonly IConfiguration _configuration;
        private readonly TeacherDbContext _db;


        // Constructor injecting dependencies
        public HomeController(ITeacherService teacherService, IConfiguration configuration, TeacherDbContext db)
        {
            _ts = teacherService;
            _configuration = configuration;
            _db = db;
        }


        //1 Endpoint for registering a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]TeacherRegisterModel model)
        {
            //Validating the model 
            TryValidateModel(model);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //If username or email is same then it will send bad request .
            var existingUser = await _db.TeacherTable.FirstOrDefaultAsync(u => u.Email == model.Email || u.Name == model.Name);
            if (existingUser != null)
            {
                if (existingUser.Email == model.Email) // Email already exists
                {
                    return BadRequest("Email already exists, try giving another email.");
                }
                else // Username already exists
                {
                    return BadRequest("Username already exists, try giving a different username.");
                }
            }

            //Getting created teacher
            var result = await _ts.CreateTeacher(model);

            // If user not found
            if (result == null)
            {
                return BadRequest("Username already exists try using another username");
            }
            //Sharing User Details
            return Ok(result);
        }

        //2 Endpoint for getting all teachers list
        [HttpGet("getallteachers")]
        public async Task<IActionResult> GetAll()
        {
            // Get the list of teachers directly
            var result = await _ts.GetAllTeacher();

            // Return the result directly
            return Ok(result);
        }

        //3 Endpoint for getting teacher by ID
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            // Getting result from service
            var result = await _ts.GetTeacherById(id);

            // Return the result directly
            return Ok(result);
        }

        //4 Endpoint for updating teacher information
        [HttpPut("updateteacher")]
        public async Task<IActionResult> Update(Teacher teacher)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service method to update the teacher
            var result = await _ts.UpdateTeacher(teacher);

            // Check if the update was successful
            if (result == null)
            {
                return BadRequest("Failed to update."); // Update unsuccessful
            }

            return Ok(result); //Return updated teacher if the update successful

        }


        //5 Endpoint for deleting a teacher
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Call the service method to delete the teacher
            var result = await _ts.DeleteTeacher(id);

            // Check if the delete was successful
            if (!result)
            {
                return BadRequest("Delete unsuccessful"); // Bad request if the delete was unsuccessful
            }

            return Ok(); // Delete successful
        }

        //6 Endpoint for teacher login
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseModel))]
        public async Task<ActionResult> Login(TeacherLoginModel model)
        {
            //Validating the model
            //TryValidateModel(model);
            if (!ModelState.IsValid)
            {
                return Unauthorized(ModelState);
            }

            //Finding the user with given email and password
            var teacher = _db.TeacherTable.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            //Getting integer result from service
            var result = await _ts.LoginTeacher(model);
            if (result == 2)//Password Doesn't match with the email
            {
                return Unauthorized("Password doesnt match with the Email");
            }
            else if (result == 1)//Email and Password Matched
            {
                //Token generation
                string token = GenerateToken(teacher);

                return Ok(new TokenResponseModel
                {
                    Token = token,
                    Name = teacher.Name,
                    Standard = teacher.Standard,
                    Id = teacher.Id,
                    Email = teacher.Email,
                    Role = teacher.Role,
                    PhoneNumber = teacher.PhoneNumber,
                });
            }
            //No user found with given email and password
            return BadRequest("Wrong Credentials");
            
        }

        // Method to generate authentication token
        [NonAction]
        private string GenerateToken(Teacher user)
        {
            var issuer = _configuration.GetValue<string>("jwt:Issuer");
            var audience = _configuration.GetValue<string>("jwt:Audience");
            var secretKey = _configuration.GetValue<string>("jwt:SecretKey");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name)
            };
            for (int i = 0; i < 5; i++)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "aud" + i));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: null,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
