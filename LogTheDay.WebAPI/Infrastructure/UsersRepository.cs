using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User = null.");
            }

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            User? user = await this.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Нет пользователя с id = {id}");
            }
            this.context.Remove(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this.context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return this.context.Users.FirstOrDefault(user => user.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersByQueryAsync(string query)
        {
            throw new NotImplementedException();
        }

        // Указываем нужное свойство и его значение для изменения.
        public async Task UpdateUserAsync(Guid id, string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Свойство не указано.", nameof(propertyName));
            }

            User? user = await this.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Нет пользователя с id = {id}");
            }

            // Получаем свойство по имени и устанавливаем новое значение
            var propertyToChange = typeof(User).GetProperty(propertyName);

            if (propertyToChange == null || !propertyToChange.CanWrite)
            {
                throw new ArgumentException($"Свойство '{propertyName}' не существует или не может быть изменено.");
            }

            propertyToChange.SetValue(user, value);
            await this.context.SaveChangesAsync();
        }
        // еще не работает
        public async Task ReplaceUserAsync(User new_user)
        {
            if (new_user == null)
            {
                throw new ArgumentNullException(nameof(new_user));
            }

            User? current_user = await this.GetUserByIdAsync(new_user.Id);
            if (current_user == null)
            {
                throw new KeyNotFoundException($"Нет пользователя с id = {new_user.Id}");
            }
            current_user.Name = new_user.Name;
            current_user.Email = new_user.Email;
            current_user.PasswordHash = new_user.PasswordHash;
            current_user.RegDate = new_user.RegDate;
            //current_user.Pages = new_user.Pages; - это надо поменять
            await this.context.SaveChangesAsync();
        }
    }
}
