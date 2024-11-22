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
            if (user == null){throw new ArgumentNullException(nameof(user), "User = null.");}

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
        
        // Универсальный utility метод, пока не решил куда его назначить TODO
        public void ChangeProperty<T>(string propertyName, object value, T subject)
        {
            if (string.IsNullOrEmpty(propertyName)){ throw new ArgumentException("Свойство не указано.", nameof(propertyName)); }

            var propertyToChange = typeof(T).GetProperty(propertyName);
            if (propertyToChange == null || !propertyToChange.CanWrite){
                throw new ArgumentException($"Свойство '{propertyName}' не существует или не может быть изменено.");}

            propertyToChange.SetValue(subject, value);
        }//

        public async Task<IEnumerable<User>> GetUsersByQueryAsync(string name = null, string email = null, DateOnly? regDate = null)
        {
            // Проверка на null в переданных агрументах
            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(email) &&
                !regDate.HasValue)
            {throw new ArgumentNullException("Ни один параметр поиска не указан");}

            // Начинаем с базового запроса к контексту
            IQueryable<User> query = this.context.Users;

            // Условно добавляем фильтры
            if (!string.IsNullOrWhiteSpace(name)){query = query.Where(user => user.Name.ToLower().Contains(name.ToLower()));}
            if (!string.IsNullOrWhiteSpace(email)){query = query.Where(user => user.Email.ToLower().Contains(email.ToLower()));}
            if (regDate.HasValue){query = query.Where(user => user.RegDate == regDate.Value);}

            return await query.ToListAsync(); ;
        }

        // Указываем нужное свойство и его значение для изменения.
        public async Task UpdateUserAsync(Guid id, string propertyName, object value)
        {
            User? user = await this.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"Нет пользователя с id = {id}");
            ChangeProperty(propertyName, value, user);
            await this.context.SaveChangesAsync();
        }

        // TODO: еще не работает
        public async Task ReplaceUserAsync(User new_user)
        {
            if (new_user == null){throw new ArgumentNullException(nameof(new_user));}

            User? current_user = await this.GetUserByIdAsync(new_user.Id);
            if (current_user == null){throw new KeyNotFoundException($"Нет пользователя с id = {new_user.Id}");}

            current_user.Name = new_user.Name;
            current_user.Email = new_user.Email;
            current_user.PasswordHash = new_user.PasswordHash;
            current_user.RegDate = new_user.RegDate;
            //current_user.Pages = new_user.Pages; - это надо поменять
            await this.context.SaveChangesAsync();
        }
    }
}
