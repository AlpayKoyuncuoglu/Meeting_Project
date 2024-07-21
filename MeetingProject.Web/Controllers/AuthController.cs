using MeetingProject.Context;
using MeetingProject.Model.Dtos;
using MeetingProject.Model.Entities;
using MeetingProject.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtAuthExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //[HttpPost("register")]
        //public IActionResult Register([FromBody] User userRegister)
        //{
        //    if (_context.Users.Any(u => u.Username == userRegister.Username))
        //    {
        //        return BadRequest("Username already exists");
        //    }

        //    var user = new User
        //    {
        //        Username = userRegister.Username,
        //        Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password) // Şifreyi hashleyin
        //    };

        //    _context.Users.Add(user);
        //    _context.SaveChanges();


        //    return Ok("User registered successfully");
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto userRegister)
        {
            if (_context.Users.Any(u => u.Username == userRegister.Username))
            {
                return BadRequest("Username already exists");
            }

            // Profil resmini kaydedin ve yolunu alın
            //string profilePicturePath = null;
            //if (userRegister.ProfilePicture != null)
            //{
            //    var filePath = Path.Combine("wwwroot/uploads", userRegister.ProfilePicture.FileName);
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await userRegister.ProfilePicture.CopyToAsync(stream);
            //    }
            //    profilePicturePath = "/uploads/" + userRegister.ProfilePicture.FileName;
            //}

            var user = new User
            {
                Username = userRegister.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password), // Şifreyi hashleyin
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName,
                Email = userRegister.Email,
                PhoneNumber = userRegister.PhoneNumber,
                //ProfilePicture = profilePicturePath
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var emailService = HttpContext.RequestServices.GetService<IEmailService>();
            if (emailService == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Email service is not available.");
            }

            string subject = "Welcome to Our Application!";
            string message = $"Hello {user.FirstName},<br><br>Thank you for registering.<br><br>Best regards,<br>Our Team";

            await emailService.SendEmailAsync(user.Email, subject, message);

            return Ok(new { success = true, message = "User registered successfully" });
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == userLogin.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
            {
                return Unauthorized();
            }

            var tokenString = GenerateJWT(user.Username);
            return Ok(new { Token = tokenString });
        }

        private string GenerateJWT(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
