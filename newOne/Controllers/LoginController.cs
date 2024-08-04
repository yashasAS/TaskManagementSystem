using newOne.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using newOne.Repositories.Interfaces;
using System.Security.Claims;

namespace newOne.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUsersRepository _usersRepository;

        public LoginController(IConfiguration config, IUsersRepository usersRepository)
        {
            _config = config;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// authenticate if the user is valid or not
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<User> AuthenticateUser(int id, string password)
        {
            User user = null;
            var ids  = new List<int>() { id };

            user = (await  _usersRepository.GetByUserIds(ids)).First();
            if(user.Password == password)
            {
                return user;
            }

            return user;
        }

        /// <summary>
        /// Generate token for a valid user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials =  new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // defining roles before generationg the token
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role.Trim())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],
                claims: claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// <summary>
        /// user logs in usinf this controller and he gets a jwt token specifing his role in the system 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(int Id, string password)
        {
            var user_ = await AuthenticateUser(Id, password);
            if(user_ != null)
            {
                var token = GenerateToken(user_);
                return Ok(new { token = token });
            }
            return Unauthorized();
        }
    }
}
