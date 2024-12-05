using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<Result<None>> AddAsync(T subject);
        Task<Result<IEnumerable<T>>> GetAllAsync();
        Task<Result<T>> GetByIdAsync(Guid id);
        Task<Result<None>> ReplaceAsync(T replacement);
        Task<Result<None>> DeleteAsync(Guid id);
        Result<IQueryable<T>> GetByODataQuery();

    }
}
