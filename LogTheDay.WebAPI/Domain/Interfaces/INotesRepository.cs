using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    public interface INotesRepository : IRepository<Note>
    {
        Task<Result<None>> UpdateLastModDate(Note note);
        Task<Result<None>> ChangeInfoAsync(Note note, string newTitle, string newDescription, string newStatus);
    }
}
