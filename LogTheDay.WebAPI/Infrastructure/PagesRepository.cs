using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure
{
    public class PagesRepository : IPagesRepository

    {
        LogTheDayContext context;
        ILogger<PagesRepository> logger;

        public PagesRepository(LogTheDayContext context, ILogger<PagesRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Result<None>> AddPageAsync(Page page)
        {
            if (page == null)
            {
                string message = $"{nameof(page)} = null.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            try
            {
                this.context.Pages.Add(page);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при добавлении {nameof(Page)}.");
                return new Result<None>(false, null, $"Ошибка при добавлении пользователя. {ex.Message}");
            }

            return new Result<None>(true, null, $"{nameof(Page)} добавлен.");
        }

        public async Task<Result<IEnumerable<Page>>> GetAllAsync()
        {
            IEnumerable<Page> pages;
            try
            {
                pages = await this.context.Pages.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при удалении {nameof(Page)}.");
                return new Result<IEnumerable<Page>>(false, null, $"Ошибка при удалении {nameof(Page)}. {ex.Message}");
            }
            return new Result<IEnumerable<Page>>(true, pages, $"{nameof(Page)}s получены.");
        }

        public async Task<Result<Page>> GetPageByIdAsync(Guid id)
        {
            Page? page;
            try
            {
                page = await this.context.Pages.FirstOrDefaultAsync(page => page.Id == id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при поиске {nameof(Page)}.");
                return new Result<Page>(false, null, $"Ошибка при поиске {nameof(Page)}. {ex.Message}");
            }
            return new Result<Page>(true, page);
        }

        // TODO: не работает, сделать общий метод перебора всех полей
        public async Task<Result<None>> ReplacePageAsync(Page replacementPage)
        {
            if (replacementPage == null)
            {
                string message = $"Не передан объект замены {nameof(Page)}.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            Page? currentPage;
            // Начинаем транзакцию, чтобы в случае неудачи откатить изменения
            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    Result<Page> currentPageRes;
                    currentPageRes = await this.GetPageByIdAsync(replacementPage.Id);
                    if (!currentPageRes.Success || currentPageRes.Content == null)
                        return new Result<None>(false, null, $"Нет {nameof(Page)} с id = {replacementPage.Id}");

                    //currentPage = currentPageRes.Content;
                    //currentPage.Name = replacementPage.Name;
                    //currentPage.Email = replacementPage.Email;
                    //currentPage.PasswordHash = replacementPage.PasswordHash;
                    //currentPage.AvatarImg = replacementPage.AvatarImg;
                    //currentPage.LastLoginDate = replacementPage.LastLoginDate;
                    //currentPage.Pages.Clear();
                    //foreach (var page in replacementPage.Pages)
                    //{
                    //    currentPage.Pages.Add(page);
                    //}
                    await this.context.SaveChangesAsync();
                    //await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    logger.LogError(ex.Message);
                    return new Result<None>(false, null, $"Ошибка при замене {nameof(Page)}. Откат... {ex.Message}");
                }

                return new Result<None>(true, null, $"{nameof(Page)} с id = {replacementPage.Id} заменен.");
            }
        }

        public async Task<Result<None>> DeletePageAsync(Guid id)
        {
            Result<Page> pageRes = await this.GetPageByIdAsync(id);
            if (!pageRes.Success) return new Result<None>(false, null, pageRes.Message);

            Page? page = pageRes.Content;
            if (page == null)
            {
                string message = $"Нет {nameof(Page)} с id = {id}";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            try
            {
                this.context.Remove(page);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при удалении {nameof(Page)}.");
                return new Result<None>(false, null, $"Ошибка при удалении {nameof(Page)}. {ex.Message}");
            }

            return new Result<None>(true, null, $"{nameof(Page)} удален.");
        }

        public async Task<Result<None>> UpdateLastModDate(Page page)
        {
            if (page == null)
                return new Result<None>(false, null, $"Не передан {nameof(Page)}.");

            page.LastModifiedDate = DateOnly.FromDateTime(DateTime.Now);
            page.LastModifiedTime = DateTime.Now;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обновлении даты изменения.");
                return new Result<None>(false, null, $"Ошибка при обновлении даты изменения. {ex.Message}");
            }
            return new Result<None>(true, null, $"Дата последнего изменения обновлена на {page.LastModifiedDate}, {page.LastModifiedTime}");
        }

        public async Task<Result<None>> ChangeInfoAsync(Page page, string newTitle, string newDescription)
        {
            if (string.IsNullOrEmpty(newTitle) || string.IsNullOrEmpty(newDescription) || page is null)
                return new Result<None>(false, null, $"Новое имя {nameof(newTitle)}, описание {nameof(newDescription)} или {nameof(Page)} не указан.");

            page.Title = newTitle;
            page.Description = newDescription;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при сохранении инфо у {nameof(Page)}.");
                return new Result<None>(false, null, $"Ошибка при сохранении инфо у {nameof(Page)}. {ex.Message}");
            }
            return new Result<None>(true, null, $"Имя {nameof(Page)} заменено на {newTitle}, и описание на {newDescription}");
        }

        public async Task<Result<None>> AddNoteAsync(Page page, Note note)
        {
            if (page == null || note == null)
            {
                string message = $"{nameof(note)} = null или {nameof(page)} = null.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            try
            {
                page.Notes.Add(note);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при добавлении {nameof(note)} к {nameof(page)}.");
                return new Result<None>(false, null, $"Ошибка при добавлении {nameof(note)} к {nameof(page)}. {ex.Message}");
            }

            return new Result<None>(true, null, $"{nameof(note)} добавлен к {nameof(Page)}.");
        }

        public async Task<Result<None>> RemoveNoteAsync(Page page, Note note)
        {
            if (page == null || note == null)
            {
                string message = $"{nameof(note)} = null или {nameof(page)} = null.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            try
            {
                page.Notes.Remove(note);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при удалении {nameof(note)} к {nameof(page)}.");
                return new Result<None>(false, null, $"Ошибка при удалении {nameof(note)} к {nameof(page)}. {ex.Message}");
            }

            return new Result<None>(true, null, $"{nameof(note)} удален из {nameof(Page)}.");
        }
    }
}
