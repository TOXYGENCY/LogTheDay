using LogTheDay.Controllers.Domain.Entities;

namespace LogTheDay.Controllers.Domain.Interfaces
{
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
