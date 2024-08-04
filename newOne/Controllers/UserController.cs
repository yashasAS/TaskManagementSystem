using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using newOne.Models;
using newOne.Repositories.Interfaces;

namespace newOne.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    /// Authorize only admin to access users
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUsersRepository _usersRepository;

        public UserController (IConfiguration config, IUsersRepository usersRepository)
        {
            _config = config;
            _usersRepository = usersRepository;
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> getUserById([FromQuery] IEnumerable<int> ids)
        {
            if(!ids.Any() || ids.Any(i => i <= 0) || ids.Count() == 0)
            {
                return BadRequest("Incorrect user ids");
            }

            var users = await  _usersRepository.GetByUserIds(ids);

            if(!users.Any() || users.Count() == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpDelete("deleteUsers/userIds/{userId}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Incorrect user id");
            }

            try
            {
                await _usersRepository.DeleteUser(id);
                return Ok();
            }
            catch(HttpRequestException ex)
            {
                // this if return appropriate status code task is not in DB
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                throw;
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreteUser([FromBody] User User)
        {
            await _usersRepository.AddUser(User);
            return Ok();
        }

        [HttpGet("getUsersByRole/role/{role}")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Incorrect role");
            }

            var users = await _usersRepository.GetUserByRole(role);

            if (!users.Any() || users.Count() == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("getUsersByTeam/team/{team}")]
        public async Task<IActionResult> GetUsersByTeam(string team)
        {
            if (string.IsNullOrWhiteSpace(team))
            {
                return BadRequest("Incorrect team");
            }

            var users = await _usersRepository.GetUserByTeam(team);

            if (!users.Any() || users.Count() == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> GetUsersByTeam([FromBody] User user)
        {
            await _usersRepository.UpdateUser(user);
            return Ok();
        }
    }
}
