using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Data;
using WebApp.Models;
using System.Linq;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserDto request)
        {
            LogActivities log = new();
            if(_context.UserDto == null)
            {
                return NotFound();
            }
            var existedAcc = _context.User;
            foreach (var acc in existedAcc.ToList())
            {
                if (acc.UserName == request.UserName)
                {
                    log.User = request.UserName;
                    log.Action = "Sign up";
                    log.Status = "Failed";
                    _context.LogActivities.Add(log);
                    await _context.SaveChangesAsync();
                    return BadRequest("USER ALREADY EXIST");
                }

            }
            User user = new();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.UserName = request.UserName;
            user.Role = request.Role;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.UserDto.Add(request);
            _context.User.Add(user);

            //log activity 
            log.User = request.UserName;
            log.Action = "Sign up";
            log.Status = "Success";
            _context.LogActivities.Add(log);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("filteruser")]
        public async Task<IActionResult> getLogUser(string request)
        {
            var product = _context.LogActivities.Where(l => (l.User.Contains(request)));
            return Ok(product);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserDto request)
        {
            LogActivities log = new();
            var existedAcc = _context.User;
            foreach (var acc in existedAcc.ToList())
            {
                //if (acc.UserName != request.UserName)
                //{
                //    return BadRequest("CANNOT FIND USER");
                //}
                if( acc.UserName == request.UserName)
                {
                    if (!VerifyPasswordHash(request.Password, acc.PasswordHash, acc.PasswordSalt))
                    {
                        log.User = request.UserName;
                        log.Status = "Failed";
                        log.Action = "Login";
                        _context.LogActivities.Add(log);
                        await _context.SaveChangesAsync();
                        return BadRequest("WRONG PASSWORD");
                    }
                    log.User = request.UserName;
                    log.Status = "Success";
                    log.Action = "Login";
                    _context.LogActivities.Add(log);
                    await _context.SaveChangesAsync();
                    string token = CreateToken(acc);
                    return Ok(new{ token } );
                }               
                //string token = CreateToken(request);
            }
            log.User = request.UserName;
            log.Status = "Failed";
            log.Action = "Login";
            _context.LogActivities.Add(log);
            await _context.SaveChangesAsync();
            return BadRequest("CANNOT FIND USER");
        }

        [HttpGet("getuser"), Authorize]
        public ActionResult<string> GetUser()
        {
            var currentUserName = User.FindFirstValue(ClaimTypes.Name);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new {currentUserName, currentUserRole});
        }

        [HttpPut("updateProfile")]
        public async Task<ActionResult> updateProfile(int id, UserDto request)
        {
            if (_context.UserDto == null)
            {
                return NotFound();
            }
            if (id != request.Id)
            {
                return BadRequest();
            }
            User user = new();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.UserName = request.UserName;
            user.Role = request.Role;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Id = request.Id;

            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(request).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }


        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
        };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppToken:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
           
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
