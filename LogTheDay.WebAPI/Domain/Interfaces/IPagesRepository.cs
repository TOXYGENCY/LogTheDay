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
        Task<Result<None>> ReplacePageAsync(Page replacementPage);
        Task<Result<None>> DeletePageAsync(Guid id);
        Task<Result<None>> UpdateLastModDate(Page page);
        Task<Result<None>> ChangeInfoAsync(Page page, string newTitle, string newDescription);
        Task<Result<None>> AddNoteAsync(Page page, Note note);
        Task<Result<None>> RemoveNoteAsync(Page page, Note note);

    }
}
