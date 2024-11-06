using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // Базовые действия над пользователями в отношении базы данных

    public interface IPagesRepository
    {
        Task AddPage(Page page);
        Task<Page> GetPageById(Guid id);
        Task<IEnumerable<Page>> GetAll();
        Task<IEnumerable<Page>> GetPagesByQuery(string query);
        Task UpdatePage(Page page);
        Task DeletePage(Guid id);

    }
}
