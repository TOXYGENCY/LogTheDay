using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure
{
    // Базовые действия над Userами в отношении базы данных
    public class UsersRepository : IUsersRepository
    {
        public LogTheDayContext context;
        ILogger<UsersRepository> logger;
        public UsersRepository(LogTheDayContext context, ILogger<UsersRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<None>> AddAsync(User user)
        {
            if (user == null)
            {
                string message = $"{nameof(user)} = null.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            try
            {
                this.context.Users.Add(user);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при добавлении {nameof(User)}.");
                return new Result<None>(false, null, $"Ошибка при добавлении {nameof(User)}: {ex.Message}");
            }

            return new Result<None>(true, null, $"{nameof(User)} добавлен.");
        }

        public async Task<Result<User>> GetByIdAsync(Guid id)
        {
            User? user;
            try
            {
                user = await this.context.Users.FirstOrDefaultAsync(user => user.Id == id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при поиске {nameof(User)}.");
                return new Result<User>(false, null, $"Ошибка при поиске {nameof(User)}. {ex.Message}");
            }
            return new Result<User>(true, user);
        }

        public async Task<Result<IEnumerable<User>>> GetAllAsync()
        {
            IEnumerable<User> users;
            try
            {
                users = await this.context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при удалении {nameof(User)}.");
                return new Result<IEnumerable<User>>(false, null, $"Ошибка при удалении {nameof(User)}. {ex.Message}");
            }
            return new Result<IEnumerable<User>>(true, users, $"{nameof(User)}s получены.");
        }

        public async Task<Result<IEnumerable<User>>> GetByQueryAsync(string name = null, string email = null, DateOnly? regDate = null)
        {
            // Проверка на null в переданных агрументах
            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(email) &&
                !regDate.HasValue)
            {
                string message = "Ни один параметр поиска не указан";
                logger.LogWarning(message);
                return new Result<IEnumerable<User>>(false, null, message);
            }
            IQueryable<User> query;

            try
            {
                // Начинаем с базового запроса к контексту
                query = this.context.Users;

                // Условно добавляем фильтры
                if (!string.IsNullOrWhiteSpace(name)) query = query.Where(user => user.Name.ToLower().Equals(name.ToLower()));
                if (!string.IsNullOrWhiteSpace(email)) query = query.Where(user => user.Email.ToLower().Equals(email.ToLower()));
                if (regDate.HasValue) query = query.Where(user => user.RegDate == regDate.Value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при поиске {nameof(User)}.");
                return new Result<IEnumerable<User>>(false, null, $"Ошибка при поиске {nameof(User)}. {ex.Message}");
            }
            return new Result<IEnumerable<User>>(true, await query.ToListAsync(), "Поиск выполнен.");
        }

        //TODO: оптимизировать замену?
        public async Task<Result<None>> ReplaceAsync(User replacementUser)
        {
            if (replacementUser == null)
            {
                string message = $"Не передан объект замены {nameof(User)}.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            User? currentUser;
            // Начинаем транзакцию, чтобы в случае неудачи откатить изменения
            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    Result<User> currentUserRes;
                    currentUserRes = await this.GetByIdAsync(replacementUser.Id);
                    if (!currentUserRes.Success || currentUserRes.Content == null)
                        return new Result<None>(false, null, $"Нет {nameof(User)} с id = {replacementUser.Id}");
                    
                    currentUser = currentUserRes.Content;
                    currentUser.Name = replacementUser.Name;
                    currentUser.Email = replacementUser.Email;
                    currentUser.PasswordHash = replacementUser.PasswordHash;
                    currentUser.AvatarImg = replacementUser.AvatarImg;
                    currentUser.LastLoginDate = replacementUser.LastLoginDate;
                    currentUser.Pages.Clear();
                    foreach (var page in replacementUser.Pages)
                    {
                        currentUser.Pages.Add(page);
                    }
                    await this.context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    logger.LogError(ex.Message);
                    return new Result<None>(false, null, $"Ошибка при замене {nameof(User)}. Откат... {ex.Message}");
                }

                return new Result<None>(true, null, $"{nameof(User)} с id = {replacementUser.Id} заменен.");
            }
        }

        public async Task<Result<None>> ChangeUserName(User user, string name)
        {
            if (string.IsNullOrEmpty(name) || user is null)
                return new Result<None>(false, null, $"Новое имя {nameof(name)} или {nameof(User)} не указан.");

            user.Name = name;

            try
            {
                await this.context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при сохранении имени {nameof(User)}.");
                return new Result<None>(false, null, $"Ошибка при сохранении имени {nameof(User)}. {ex.Message}");
            }
            return new Result<None>(true, null, $"Имя {nameof(User)} заменено на {name}");
        }
        public async Task<Result<None>> DeleteAsync(Guid id)
        {
            Result<User> userRes = await this.GetByIdAsync(id);
            if (!userRes.Success) return new Result<None>(false, null, userRes.Message);

            User? user = userRes.Content;
            if (user == null)
            {
                string message = $"Нет {nameof(User)} с id = {id}";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }
            try
            {
                this.context.Remove(user);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при удалении {nameof(User)}.");
                return new Result<None>(false, null, $"Ошибка при удалении {nameof(User)}. {ex.Message}");
            }

            return new Result<None>(true, null, $"{nameof(User)} удален.");
        }

        public async Task<Result<None>> UpdateLastLoginDateAsync(User user)
        {
            if (user == null)
                return new Result<None>(false, null, $"Не передан {nameof(User)}.");

            user.LastLoginDate = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при обновлении даты входа.");
                return new Result<None>(false, null, $"Ошибка при обновлении даты входа. {ex.Message}");
            }
            return new Result<None>(true, null, $"Дата последнего входа обновлена на {user.LastLoginDate}");
        }

        public Result<IQueryable<User>> GetByODataQuery()
        {
            IQueryable<User> users;
            try
            {
                users = context.Users;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Ошибка при получении {nameof(User)}s из контекста.");
                return new Result<IQueryable<User>>(false, null, $"Ошибка при получении {nameof(User)}s из контекста. {ex.Message}");
            }
            return new Result<IQueryable<User>>(true, users, $"Данные выбранных {nameof(User)}s отправлены.");
        }
    }
}
