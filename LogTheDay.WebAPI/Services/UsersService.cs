using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Services
{
    // В сервисе прописана реализация бизнес-логики
    // не связанной с базовыми действиями элементов

    public class UsersService : IUsersService
    {
        public Task AuthenticateAsync(string login, string password_hash)
        {
            // сравнить хеш пароля и логина с базой данных 

            throw new NotImplementedException();
        }

        public Task ChangeNameAsync(User user, string NewName)
        {
            throw new NotImplementedException();
        }

        public Task RegisterNewUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
