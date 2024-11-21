using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Services
{
    // В сервисе прописана реализация бизнес-логики
    // не связанной с базовыми действиями элементов

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersService(IUsersRepository usersRepository) {
            this._usersRepository = usersRepository;
        }
        // TODO: хеширование пароля
        public async Task AuthenticateAsync(string login, string password_hash)
        {
            // TODO: сравнить хеш пароля и логина с базой данных 

            throw new NotImplementedException();
        }

        public async Task ChangeNameAsync(Guid id, string NewName)
        {
            // TODO: проверить нет ли такого логина в бд и изменить имя
            throw new NotImplementedException();
        }

        public async Task RegisterNewUserAsync(string Name, string Email, string PasswordHash)
        {
            if (await _usersRepository.UserExistsByName(Name)){
                throw new Exception($"Пользователь с именем \"{Name}\" уже существует. Введите другое имя пользователя.");}
            DateOnly RegDate = DateOnly.FromDateTime(DateTime.Now);

            //var existingUser = _usersRepository.GetUsersByQueryAsync();
            // TODO: проверить нет ли такого логина в бд и добавить
            throw new NotImplementedException();
        }
    }
}
