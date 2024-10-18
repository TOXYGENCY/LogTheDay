﻿using LogTheDay.Controllers.Domain.Entities;
using LogTheDay.Controllers.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Npgsql;
using System.Data;
using System.Xml.Linq;

namespace LogTheDay.Controllers.Infrastructure
{
    // Реализация взаимодействия с базой, которая используется из UsersCRUDController
    public class UsersSQLRepository : IUsersRepository
    {
        private readonly IConfiguration _configuration;
        public UsersSQLRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /* (WIP) Общий метод для запросов к базе
        private async Task<IDataReader> ReachToDb(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

            var result = new List<User>(); // Лист user, потому что репозиторий только про юзеров
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Создаем и добавляем в результат пользователей (объекты), которых получаем
                        result.Add(new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                            RegistrationDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("reg_date"))) // Предположим, что это DateTime
                        });
                    }
                }
            }
            return result;

        }*/


        public async Task<User> GetUserById(Guid id)
        {
            User? result = null;
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand($"SELECT * FROM public.users WHERE id = '{id}'", connection)) // не защищено от SQL-инъекций
                {
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        result = new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                            RegistrationDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("reg_date")))
                        };
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<User>> GetUsersByQuery(string sqlQuery)
        {
            var result = new List<User>();
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Создаем и добавляем в результат пользователей (объекты), которых получаем
                        result.Add(new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                            RegistrationDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("reg_date")))
                        });
                    }
                }
            }
            return result;
        }

        public async Task DeleteUser(Guid id)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand($"DELETE FROM public.users WHERE id = '{id}'", connection)) // не защищено от SQL-инъекций
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task<IEnumerable<User>> GetAll()
        {
            return await GetUsersByQuery("SELECT * FROM public.users");
        }

        public async Task AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                // не защищено от SQL-инъекций
                using (var command = new NpgsqlCommand(
                    $"INSERT INTO public.users (id, name, email, password_hash, reg_date) VALUES ('{Guid.NewGuid()}', '{user.Name}', '{user.Email}', '{user.PasswordHash}', CURRENT_DATE);", connection)) 
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                // не защищено от SQL-инъекций
                using (var command = new NpgsqlCommand(
                    $"UPDATE public.users SET name = '{user.Name}', email = '{user.Email}', password_hash = '{user.PasswordHash}', reg_date = '{user.RegistrationDate}' WHERE id = '{user.Id}';", connection))
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}