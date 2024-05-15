using Dapper;
using ediMvcApi.Models;
using ediMvcApi.Repository.UserRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ediMvcApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private Response res = new Response();

        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet("[action]")]
        public async Task<Response> GetAllUser()
        {
            res = await _userRepository.GetAllUsers();
            return res;
        }

        [HttpGet("[action]")]
        public async Task<Response> GetUserByEmail(string email)
        {
            res = await _userRepository.GetUserByEmail(email);
            return res;
        }

        [HttpGet("[action]")]
        public async Task<Response> GetUserById(int userId)
        {
            res = await _userRepository.GetUserById(userId);
            return res;
        }

        [HttpGet("[action]")]
        public async Task<Response> LoginUser(string email, string password)
        {
            res = await _userRepository.LoginUser(email, password);
            return res;
        }

        [HttpPost("[action]")]
        public async Task<Response> CreateUser(User user)
        {
            res = await _userRepository.CreateUser(user);
            return res;
        }

        [HttpPut("[action]")]
        public async Task<Response> UpdateUser(User user)
        {
            res = await _userRepository.UpdateUser(user);
            return res;
        }


        [HttpDelete("[action]")]
        public async Task<Response> DeleteUser(int userId)
        {
            res = await _userRepository.DeleteUser(userId);
            return res;
        }

        //[HttpGet("[action]")]
        //public async Task<ActionResult<User>> LoginUser(string email, string password)
        //{
        //    using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        //    var users = await connection.QueryFirstAsync<User>("select * from t_user where email = @Email and password = @Password", new {Email = email, Password = password});
        //    return Ok(users);
        //}
    }
}
