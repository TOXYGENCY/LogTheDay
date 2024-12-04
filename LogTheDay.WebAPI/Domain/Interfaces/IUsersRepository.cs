using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // Базовые действия над пользователями в отношении базы данных
    public interface IUsersRepository
    {
        Task<Result<None>> AddUserAsync(User user);
        Task<Result<User>> GetUserByIdAsync(Guid id);
        Task<Result<IEnumerable<User>>> GetAllAsync();
        Task<Result<IEnumerable<User>>> GetUsersByQueryAsync(string name = null, string email = null, DateOnly? regDate = null);
        Task<Result<None>> ChangeUserName(User user, string name);
        Task<Result<None>> ReplaceUserAsync(User user);
        Task<Result<None>> DeleteUserAsync(Guid id);
        Task<Result<None>> UpdateLastLoginDateAsync(User user);

    }
}
