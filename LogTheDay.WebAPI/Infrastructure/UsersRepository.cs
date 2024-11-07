using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure
{
    // Базовые действия над пользователями в отношении базы данных
    public class UsersRepository : IUsersRepository
    {
        LogTheDayContext _context;
        public UsersRepository()
        {
            this._context = new LogTheDayContext();
        }

        public async Task AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return this._context.Users.FirstOrDefault(user => user.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersByQueryAsync(string query)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
