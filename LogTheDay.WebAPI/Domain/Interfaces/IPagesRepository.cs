using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // Базовые действия над пользователями в отношении базы данных

    public interface IPagesRepository
    {
        Task<Result<None>> AddPageAsync(Page page);
        Task<Result<Page>> GetPageByIdAsync(Guid id);
        Task<Result<IEnumerable<Page>>> GetAllAsync();
        //Task<Result<IEnumerable<Page>>> GetPagesByQueryAsync(string query);
        Task<Result<None>> ReplacePageAsync(Page page);
        Task<Result<None>> DeletePageAsync(Guid id);

    }
}
