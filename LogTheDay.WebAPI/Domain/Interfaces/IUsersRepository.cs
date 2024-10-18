using LogTheDay.Controllers.Domain.Entities;

namespace LogTheDay.Controllers.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task AddUser(User user);
        Task<User> GetUserById(Guid id);
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> GetUsersByQuery(string query);
        Task UpdateUser(User user);
        Task DeleteUser(Guid id);

    }
}
