using ediMvcApi.Models;

namespace ediMvcApi.Repository.UserRepo
{
    public interface IUserRepository
    {
        Task<Response> GetAllUsers();
        Task<Response> GetUserById(int userId);
        Task<Response> GetUserByEmail(string email);
        Task<Response> LoginUser(string username, string password);
        Task<Response> CreateUser(User user);
        Task<Response> UpdateUser(User user);
        Task<Response> DeleteUser(int userId);
    }
}
