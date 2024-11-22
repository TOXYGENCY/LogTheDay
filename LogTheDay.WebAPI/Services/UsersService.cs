using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Exceptions;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LogTheDay.LogTheDay.WebAPI.Services
{
    // В сервисе прописана реализация бизнес-логики
    // не связанной с базовыми действиями элементов

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersService(IUsersRepository usersRepository)
        {
            this._usersRepository = usersRepository;
        }
        // TODO: хеширование пароля
        public async Task AuthenticateAsync(string Email, string PasswordHash)
        {
            // TODO: думаю над архитектурой приложения и метода 

            throw new NotImplementedException();
        }

        public async Task ChangeNameAsync(Guid Id, string NewName)
        {
            if (string.IsNullOrEmpty(NewName)) throw new ArgumentNullException($"Новое имя {nameof(NewName)} не передано.");
            await _usersRepository.UpdateUserAsync(Id, "Name", NewName);
        }

        public async Task RegisterNewUserAsync(string Name, string Email, string PasswordHash)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(PasswordHash))
                throw new ArgumentNullException("Either Name is null OR Email is null OR PasswordHash is null");

            if (await this.UserExistsWithEmail(Email))
                throw new EmailTakenException(Email);

            DateOnly RegDate = DateOnly.FromDateTime(DateTime.Now);
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Email = Email,
                PasswordHash = PasswordHash,
                RegDate = RegDate
                // Pages будет автоматически инициализировано пустым списком
            };
            await _usersRepository.AddUserAsync(newUser);
        }

        public async Task<bool> UserExistsWithEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentException("Email не указан.", nameof(Email));

            IEnumerable<User> potentialUser = await _usersRepository.GetUsersByQueryAsync(email: Email);
            return potentialUser.Any();
        }
    }
}
