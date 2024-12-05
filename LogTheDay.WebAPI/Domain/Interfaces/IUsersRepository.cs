using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // Базовые действия над пользователями в отношении базы данных
    public interface IUsersRepository : IRepository<User>
    {
        Task<Result<IEnumerable<User>>> GetByQueryAsync(string name = null, string email = null, DateOnly? regDate = null);
        Task<Result<None>> ChangeUserName(User user, string name);
        Task<Result<None>> UpdateLastLoginDateAsync(User user);

    }
}
