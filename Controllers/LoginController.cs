using Dot6.API.Crud.Data;
using ManageEmployee.DTO;
using ManageEmployee.Models;
using ManageEmployee.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManageEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyWorldDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public LoginController( MyWorldDbContext dbContext, IConfiguration configuration, IUserService userService)
        {
            this._configuration = configuration;
            this._dbContext = dbContext;
            _userService = userService;
        }

        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAsync()
        {
            var acc = await _dbContext.Account.ToListAsync();
            return Ok(acc);
        }



        [HttpGet("GetInfo"), Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);

            /* var userName = User?.Identity?.Name;
             var userName2 = User.FindFirstValue(ClaimTypes.Name);
             var role = User.FindFirstValue(ClaimTypes.Role);

             return Ok(new { userName, userName2, role});*/
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDTO user)
        {

           //User account= await _dbContext.login.ToListAsync();
            var acc = await _dbContext.Account.FirstOrDefaultAsync(i => i.UserName == user.UserName );
          
          if(acc == null){
          return BadRequest("Account not exist");
          }else if(VerifyPasswordHash(user.Password, acc.PasswordHash, acc.PasswordSalt)){
            string token = CreateToken(acc);
               return Ok(token);
          }else {
            return BadRequest("Wrong Password");
          }
            
                
        }

        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDTO user)
        {

           CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);  
           User u = new User();
           u.UserName = user.UserName;
           u.PasswordHash = passwordHash;
           u.PasswordSalt= passwordSalt;
            _dbContext.Account.Add(u);        
                    await _dbContext.SaveChangesAsync();
                return Ok("Created Successfull");
                
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> 
            {
                new Claim("user", user.UserName),
                new Claim("role", "Admin")
                
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
            (_configuration.GetSection("AppSettings:Token").Value));
             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
             var token = new JwtSecurityToken(
                claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
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
                using( var hmac = new HMACSHA512(passwordSalt))
                {
                      var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               
                        return computedHash.SequenceEqual(passwordHash);
                }
            }
     }
}
