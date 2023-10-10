using EAD_Web_Services.Models.UserModel;
using EAD_Web_Services.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EAD_Web_Services.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService userService;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        [HttpGet("{nic}")]
        public ActionResult<User> Get(string nic)
        {
            var user = userService.Get(nic);

            if (user == null)
            {
                return NotFound($"User with Nic {nic} not found");
            }
            return user;
        }

        [HttpGet("getbyRole/{role}")]
        public ActionResult<List<User>> GetbyRole(string role)
        {
            List<User> allUsers = userService.GetbyRole(role);

            if (allUsers == null)
            {
                return NotFound($"User with {role} not found");
            }
            return allUsers;
        }

        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            var result = userService.Create(user);

            return Ok(result);
        }


        [HttpPut("{nic}")]
        public ActionResult Put(string nic, [FromBody] User user)
        {
            var existingUser = userService.Get(nic);

            if (existingUser == null)
            {
                return NotFound($"User  not found");
            }

            userService.Update(nic, user);
            return Ok("Updated");
        }

        [HttpPatch("active_deactive/{nic}")]
        public ActionResult UpdateStatus(string nic)
        {

            var result = userService.UpdateStatus(nic);
            return Ok(result);
        }

        [HttpDelete("{nic}")]
        public ActionResult Delete(string nic)
        {

            var user = userService.Get(nic);

            if (user == null)
            {
                return NotFound($"User with Nic = {nic} not found");
            }
            userService.Delete(user.Nic);

            return Ok($"User with Nic = {nic} deleted");
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginRequest loginRequest)
        {
            const string verifyPasswordHash = "true";
            const string verifyPasswordDeactivated = "deactivated";
            const string passwordEncrypted = "Encrypted";

            var user = userService.Get(loginRequest.Nic);

            if (user == null)
            {
                return NotFound($"User with Nic {loginRequest.Nic} not found");
            }

            var isPasswordVerified = userService.Login(loginRequest.Nic, loginRequest.Password);

            if (isPasswordVerified == verifyPasswordHash)
            {
                user.Password = passwordEncrypted;
                string token = CreateToken(user);

                var response = new
                {
                    nic = user.Nic,
                    name = user.Name,
                    email = user.Email,
                    user_Role = user.UserRole,
                    isActive = user.IsActive,
                    created = user.CreatedAt,
                    updated = user.UpdatedAt,
                    Token = token
                };

                return Ok(response);
            }
            else if (isPasswordVerified == verifyPasswordDeactivated)
            {
                return BadRequest("deactivated");
            }
            else
            {
                return BadRequest("Incorrect Nic or password");

            }

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nic),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
