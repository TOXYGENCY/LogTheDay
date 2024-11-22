using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // В сервисе прописана реализация бизнес-логики (а это её интерфейс),
    // не связанной с базовыми действиями элементов
    public interface IUsersService
    {
        // Из сайта получим логин и уже захешированный пароль
        Task AuthenticateAsync(string Email, string PasswordHash);
        Task ChangeNameAsync(Guid Id, string NewName);
        Task RegisterNewUserAsync(string Name, string Email, string PasswordHash);
        Task<bool> UserExistsWithEmail(string Email);
        // что-то еще
    }
}
