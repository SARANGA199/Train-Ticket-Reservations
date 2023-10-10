//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20236014
//   Name  :  Ravindu Yasith T.K.

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
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>

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

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        /// <returns>A list of User objects.</returns>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        /// <summary>
        /// Get a user by their NIC number.
        /// </summary>
        /// <param name="nic">The NIC number of the user to retrieve.</param>
        /// <returns>The User object if found; otherwise, returns NotFound.</returns>
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

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The User object to create.</param>
        /// <returns>The created User object.</returns>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            var result = userService.Create(user);

            return Ok(result);
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="nic">The NIC of the user to update.</param>
        /// <param name="user">The User object with updated information.</param>
        /// <returns>Ok if the user is updated successfully; otherwise, NotFound.</returns>
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

        /// <summary>
        /// Update user status (active/de-active) by NIC.
        /// </summary>
        /// <param name="nic">The NIC of the user to update.</param>
        /// <returns>Return the result as an HTTP response and the status update result.</returns>
        [HttpPatch("active_deactive/{nic}")]
        public ActionResult UpdateStatus(string nic)
        {

            var result = userService.UpdateStatus(nic);
            return Ok(result);
        }

        /// <summary>
        /// Delete a user by NIC.
        /// </summary>
        /// <param name="nic">The NIC of the user to delete.</param>
        /// <returns>ActionResult indicating the deletion result.</returns>
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
        /// <summary>
        /// Authenticate a user by NIC and password.
        /// </summary>
        /// <param name="loginRequest">The login request containing NIC and password.</param>
        /// <returns>The User object if authenticated; otherwise, returns appropriate error messages.</returns>
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
