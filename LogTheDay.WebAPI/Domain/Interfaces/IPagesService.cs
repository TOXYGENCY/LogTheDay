using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    public interface IPagesService
    {
        Task<Result<None>> ChangeInfoAsync(Guid id, string newTitle, string newDescription);

    }
}
