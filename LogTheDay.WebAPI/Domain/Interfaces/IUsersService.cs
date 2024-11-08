using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // В сервисе прописана реализация бизнес-логики (а это её интерфейс),
    // не связанной с базовыми действиями элементов
    public interface IUsersService
    {
        // Из сайта получим логин и уже захешированный пароль
        Task AuthenticateAsync(string login, string password);
        Task ChangeNameAsync(User user, string NewName);
        Task RegisterNewUserAsync(User user);
        // что-то еще
    }
}
