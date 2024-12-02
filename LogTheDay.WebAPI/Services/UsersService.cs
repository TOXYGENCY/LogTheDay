using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Exceptions;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LogTheDay.LogTheDay.WebAPI.Services
{
    // В сервисе прописана реализация бизнес-логики
    // не связанной с базовыми действиями элементов

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly PasswordHasher<object> hasher;
        public UsersService(IUsersRepository usersRepository)
        {
            this._usersRepository = usersRepository;
            hasher = new PasswordHasher<object>();
        }
        // хеширование пароля
        protected string HashPassword(string password)
        {
            return hasher.HashPassword(null, password);
        }
        protected bool VerifyPassword(string hashedPassword, string providedPasswordString)
        {
            return hasher.VerifyHashedPassword(null, hashedPassword, providedPasswordString) == PasswordVerificationResult.Success
                ?  true : false;
        }

        public async Task<bool> AuthenticateAsync(string email, string passwordString)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordString)) throw new ArgumentNullException(nameof(email), nameof(passwordString));
            if (!await this.UserExistsWithEmailAsync(email)) throw new KeyNotFoundException(nameof(email));
            IEnumerable<User> requestedAccountArr = await _usersRepository.GetUsersByQueryAsync(email: email);

            return this.VerifyPassword(requestedAccountArr.First().PasswordHash, passwordString);
        }

        public async Task ChangeNameAsync(Guid id, string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentNullException($"Новое имя {nameof(newName)} не передано.");
            User? user = await  _usersRepository.GetUserByIdAsync(id);
            _usersRepository.ChangeUserName(user, newName);
        }

        public async Task RegisterNewUserAsync(string name, string email, string passwordString)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordString))
                throw new ArgumentNullException("Either Name is null OR Email is null OR PasswordHash is null");

            if (await this.UserExistsWithEmailAsync(email))
                throw new EmailTakenException(email);

            DateOnly regDate = DateOnly.FromDateTime(DateTime.Now);
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = this.HashPassword(passwordString),
                RegDate = regDate
                // Pages будет автоматически инициализировано пустым списком
            };
            await _usersRepository.AddUserAsync(newUser);
        }

        public async Task<bool> UserExistsWithEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email не указан.", nameof(email));

            IEnumerable<User> potentialUser = await _usersRepository.GetUsersByQueryAsync(email: email);
            return potentialUser.Any();
        }
    }
}
