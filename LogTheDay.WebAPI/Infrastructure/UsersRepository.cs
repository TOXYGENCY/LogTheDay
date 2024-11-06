using LogTheDay.Controllers.Domain.Interfaces;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

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

        public Task AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return this._context.Users.FirstOrDefault(user => user.Id == id);
        }

        public Task<IEnumerable<User>> GetUsersByQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
