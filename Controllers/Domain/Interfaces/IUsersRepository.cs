using LogTheDay_WebAPI.Controllers.Domain.Entities;

namespace LogTheDay_WebAPI.Controllers.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> AddUser(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUserById(Guid id);
        Task<IEnumerable<User>> GetUsersByQuery(string query);
        Task DeleteUser(Guid id);

    }
}
