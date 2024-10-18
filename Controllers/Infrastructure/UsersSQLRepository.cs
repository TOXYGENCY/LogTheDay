using LogTheDay_WebAPI.Controllers.Domain.Entities;
using LogTheDay_WebAPI.Controllers.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Npgsql;
using System.Xml.Linq;

namespace LogTheDay_WebAPI.Controllers.Infrastructure
{
    public class UsersSQLRepository : IUsersRepository
    {
        private readonly IConfiguration _configuration;
        public UsersSQLRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<User> AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<User>> GetAll()
        {
            var result = new List<User>();
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand("SELECT * FROM public.users", connection))
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
        }


        public async Task<User> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsersByQuery(string query)
        {
            throw new NotImplementedException();
        }
    }
}
