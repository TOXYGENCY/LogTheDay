using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // В сервисе прописана реализация бизнес-логики (а это её интерфейс),
    // не связанной с базовыми действиями элементов
    public interface IUsersService
    {
        // Из сайта получим логин и уже захешированный пароль
        Task<bool> AuthenticateAsync(string email, string passwordString);
        Task ChangeNameAsync(Guid id, string newName);
        Task RegisterNewUserAsync(string name, string email, string passwordString);
        Task<bool> UserExistsWithEmailAsync(string email);
        // что-то еще
    }
}
