﻿using LogTheDay.LogTheDay.WebAPI.Domain.Entities;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    // Базовые действия над пользователями в отношении базы данных
    public interface IUsersRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetUsersByQueryAsync(string query);
        Task ReplaceUserAsync(User user);
        Task DeleteUserAsync(Guid id);

    }
}
