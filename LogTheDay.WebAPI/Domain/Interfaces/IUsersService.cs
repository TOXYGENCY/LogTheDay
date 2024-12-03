using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // В сервисе прописана реализация бизнес-логики (а это её интерфейс),
    // не связанной с базовыми действиями элементов
    public interface IUsersService
    {
        // Из сайта получим логин и уже захешированный пароль
        Task<Result<bool>> AuthenticateAsync(string email, string passwordString);
        Task<Result<None>> ChangeNameAsync(Guid id, string newName);
        Task<Result<None>> RegisterNewUserAsync(string name, string email, string passwordString);
        Task<Result<bool>> UserExistsWithEmailAsync(string email);
        // что-то еще
    }
}
