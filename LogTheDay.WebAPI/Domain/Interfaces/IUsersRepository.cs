using LogTheDay.Controllers.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.Controllers.Domain.Interfaces
{
    // Базовые действия над пользователями в отношении базы данных
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
