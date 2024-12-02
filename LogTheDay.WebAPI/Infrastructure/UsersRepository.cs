using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql.TypeMapping;
using System.Reflection;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure
{
    // Базовые действия над пользователями в отношении базы данных
    public class UsersRepository : IUsersRepository
    {
        LogTheDayContext context;
        public UsersRepository(LogTheDayContext context)
        {
            this.context = context;
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "User = null.");

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            User? user = await this.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"Нет пользователя с id = {id}");
            this.context.Remove(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this.context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await this.context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public void ChangeUserName(User user, string name)
        {
            if (string.IsNullOrEmpty(name) || user is null)
                throw new ArgumentException($"Новое имя {nameof(name)} или пользователь {nameof(user)} не указано.");

            user.Name = name;
        }

        // TODO: сделать через OData
        public async Task<IEnumerable<User>> GetUsersByQueryAsync(string name = null, string email = null, DateOnly? regDate = null)
        {
            // Проверка на null в переданных агрументах
            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(email) &&
                !regDate.HasValue)
            { throw new ArgumentNullException("Ни один параметр поиска не указан"); }

            // Начинаем с базового запроса к контексту
            IQueryable<User> query = this.context.Users;

            // Условно добавляем фильтры
            if (!string.IsNullOrWhiteSpace(name)) { query = query.Where(user => user.Name.ToLower().Equals(name.ToLower())); }
            if (!string.IsNullOrWhiteSpace(email)) { query = query.Where(user => user.Email.ToLower().Equals(email.ToLower())); }
            if (regDate.HasValue) { query = query.Where(user => user.RegDate == regDate.Value); }

            return await query.ToListAsync(); ;
        }

        public async Task ReplaceUserAsync(User newUser)
        {
            // Начинаем транзакцию, чтобы в случае неудачи откатить изменения
            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (newUser == null) throw new ArgumentNullException(nameof(newUser));

                    User? currentUser = await this.GetUserByIdAsync(newUser.Id);
                    if (currentUser == null) throw new KeyNotFoundException($"Нет пользователя с id = {newUser.Id}");

                    currentUser.Name = newUser.Name;
                    currentUser.Email = newUser.Email;
                    currentUser.PasswordHash = newUser.PasswordHash;
                    //currentUser.RegDate = newUser.RegDate;
                    currentUser.Pages.Clear(); 
                    foreach (var page in newUser.Pages)
                    {
                        currentUser.Pages.Add(page);
                    }
                    await this.context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
