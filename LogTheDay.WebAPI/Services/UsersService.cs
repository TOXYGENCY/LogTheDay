using LogTheDay.LogTheDay.WebAPI.Controllers;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace LogTheDay.LogTheDay.WebAPI.Services
{
    // В сервисе прописана реализация бизнес-логики
    // не связанной с базовыми действиями элементов

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly PasswordHasher<object> hasher;
        ILogger<UsersController> logger;
        public UsersService(IUsersRepository usersRepository, ILogger<UsersController> logger)
        {
            this._usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            hasher = new PasswordHasher<object>();
        }

        // хеширование пароля
        protected string HashPassword(string password)
        {
            return hasher.HashPassword(null, password);
        }
        protected Result<bool> VerifyPassword(string hashedPassword, string providedPasswordString)
        {
            bool resultBool;
            try
            {
                resultBool = hasher.VerifyHashedPassword(null, hashedPassword, providedPasswordString) == PasswordVerificationResult.Success
                ? true : false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new Result<bool>(false, false, ex.Message);
            }
            return new Result<bool>(true, resultBool, "Пароль проверен.");
        }

        public async Task<Result<bool>> AuthenticateAsync(string email, string passwordString)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordString))
            {
                string message = "Данные пусты.";
                logger.LogWarning(message);
                return new Result<bool>(false, false, message);
            }

            Result<bool> emailExistsRes = await this._UserExistsWithEmailAsync(email);
            if (!(emailExistsRes).Success) return new Result<bool>(false, false, emailExistsRes.Message);
            if (!emailExistsRes.Content)
            {
                string message = $"Пользователь с E-mail \"{email}\" не существует.";
                logger.LogWarning(message);
                return new Result<bool>(false, false, message);
            }
            Result<IEnumerable<User>> requestedAccountArrRes = await _usersRepository.GetUsersByQueryAsync(email: email);
            if (!requestedAccountArrRes.Success || requestedAccountArrRes.Content == null)
            {
                return new Result<bool>(false, false, requestedAccountArrRes.Message);
            }
            Result<bool> passCheckRes = this.VerifyPassword(requestedAccountArrRes.Content.First().PasswordHash, passwordString);
            if (!passCheckRes.Success) return new Result<bool>(false, false, passCheckRes.Message);

            return new Result<bool>(true, passCheckRes.Content, "Пароль проверен.");
        }

        public async Task<Result<None>> ChangeNameAsync(Guid id, string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                string message = $"Новое имя не передано.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }

            Result<User> userRes = await _usersRepository.GetUserByIdAsync(id);
            if (!userRes.Success || userRes.Content == null)
            {
                return new Result<None>(false, null, userRes.Message);
            }

            User? user = userRes.Content;

            Result<None> nameChangeRes = await _usersRepository.ChangeUserName(user, newName);
            if (!nameChangeRes.Success)
            {
                return new Result<None>(false, null, nameChangeRes.Message);
            }
            await _usersRepository.UpdateLastLoginDateAsync(user);
            return new Result<None>(true, null, nameChangeRes.Message);
        }

        public async Task<Result<None>> RegisterNewUserAsync(string name, string email, string passwordString)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordString))
            {
                string message = "Either Name is null OR Email is null OR PasswordHash is null";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }

            Result<bool> emailExistsRes = await this._UserExistsWithEmailAsync(email);
            if (!(emailExistsRes).Success) return new Result<None>(false, null, emailExistsRes.Message);
            if (emailExistsRes.Content)
            {
                string message = $"Пользователь с E-mail \"{email}\" уже существует.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }

            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = this.HashPassword(passwordString),
                //RegDate = DateOnly.FromDateTime(DateTime.Now),
                //RegTime = DateTime.Now(),
                //LastLoginDate = DateOnly
                // Pages будет автоматически инициализировано пустым списком
            };
            Result<None> registrationRes = await _usersRepository.AddUserAsync(newUser);
            if (!(registrationRes).Success) return registrationRes;

            return new Result<None>(true, null, $"Пользователь с E-mail \"{email}\" зарегистрирован.");
        }

        public async Task<Result<bool>> UserExistsWithEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                string message = $"{nameof(email)} не передан.";
                logger.LogWarning(message);
                return new Result<bool>(false, false, message);
            }
            return await this._UserExistsWithEmailAsync(email);
        }

        protected async Task<Result<bool>> _UserExistsWithEmailAsync(string email)
        {
            Result<IEnumerable<User>> potentialUserRes = await _usersRepository.GetUsersByQueryAsync(email: email);
            if (!potentialUserRes.Success || potentialUserRes.Content == null)
            {
                return new Result<bool>(false, false, potentialUserRes.Message);
            }
            IEnumerable<User> potentialUser = potentialUserRes.Content;
            return new Result<bool>(true, potentialUser.Any(), $"Проверка пользователя с Email = '{email}' прошла успешно.");
        }
    }
}
