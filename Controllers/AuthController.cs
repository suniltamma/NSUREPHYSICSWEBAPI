using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace NSurePhysicsWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            // Use SP_VALIDATE_USER stored procedure to validate the user.
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SP_VALIDATE_USER";
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    var emailParam = command.CreateParameter();
                    emailParam.ParameterName = "@EMAIL";
                    emailParam.Value = request.Username; // Assuming Username is the email address
                    command.Parameters.Add(emailParam);

                    var passwordParam = command.CreateParameter();
                    passwordParam.ParameterName = "@PASSWORD_MAIL";
                    passwordParam.Value = request.Password;
                    command.Parameters.Add(passwordParam);

                    // Output parameters
                    var userIdParam = command.CreateParameter();
                    userIdParam.ParameterName = "@USERS_ID";
                    userIdParam.DbType = DbType.Int32;
                    userIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(userIdParam);

                    var roleIdParam = command.CreateParameter();
                    roleIdParam.ParameterName = "@ROLE_ID";
                    roleIdParam.DbType = DbType.Int32;
                    roleIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(roleIdParam);

                    var returnValueParam = command.CreateParameter();
                    returnValueParam.ParameterName = "@RETURN_VALUE";
                    returnValueParam.DbType = DbType.Int32;
                    returnValueParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(returnValueParam);

                    await command.ExecuteNonQueryAsync();

                    int returnValue = (int)returnValueParam.Value;

                    // Check the validation outcome
                    if (returnValue != 1)
                    {
                        if (returnValue == 0)
                            return Unauthorized("Invalid password");
                        else if (returnValue == 2)
                            return Unauthorized("Email not found");
                    }

                    int userId = (int)userIdParam.Value;
                    int roleId = (int)roleIdParam.Value;

                    // Map role ID to role name (adjust as needed)
                    string roleName = roleId switch
                    {
                        1 => "Admin",
                        2 => "Student",
                        3 => "Instructor",
                        _ => "Unknown"
                    };

                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim("UserId", userId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in using cookie authentication
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Save user details in session
                    HttpContext.Session.SetString("UserId", userId.ToString());
                    HttpContext.Session.SetString("Role", roleName);

                    // Return login success response; you may store the role in localStorage on the frontend if needed.
                    return Ok(new { message = "Login successful", role = roleName, userId = userId, returnValue = returnValue });
                }
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out" });
        }

        // Optional endpoint to check session status
        [HttpGet("check-session")]
        public IActionResult CheckSession()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Correct claim type for UserId (matches the claim added during login)
                var userIdClaim = User.FindFirst("UserId");
                // Correct claim type for Role (using ClaimTypes.Role)
                var roleClaim = User.FindFirst(ClaimTypes.Role);

                if (userIdClaim != null && roleClaim != null)
                {
                    return Ok(new
                    {
                        userId = userIdClaim.Value,
                        role = roleClaim.Value
                    });
                }
                else
                {
                    return Unauthorized(new { message = "User claims are missing." });
                }
            }

            return Unauthorized(new { message = "Authentication failed." });
        }
    }
        public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
