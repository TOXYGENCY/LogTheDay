using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure
{
    public class NotesRepository : INotesRepository
    {
        public Task<Result<None>> AddAsync(Note note)
        {
            throw new NotImplementedException();
        }

        public Task<Result<None>> ChangeInfoAsync(Note note, string newTitle, string newDescription, string newStatus)
        {
            throw new NotImplementedException();
        }

        public Task<Result<None>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Note>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Note>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Result<IQueryable<Note>> GetByODataQuery()
        {
            throw new NotImplementedException();
        }

        public Task<Result<None>> ReplaceAsync(Note replacement)
        {
            throw new NotImplementedException();
        }

        public Task<Result<None>> UpdateLastModDate(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
