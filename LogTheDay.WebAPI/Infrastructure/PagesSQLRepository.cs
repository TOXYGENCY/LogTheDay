using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Npgsql;
using System.Data;
using System.Xml.Linq;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure
{
    // Реализация взаимодействия с базой, которая используется из PagesCRUDController
    public class PagesSQLRepository : IPagesRepository
    {
        private readonly IConfiguration _configuration;
        public PagesSQLRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public async Task<Page> GetPageById(Guid id)
        {
            Page? result = null;
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand($"SELECT * FROM public.pages WHERE id = '{id}'", connection)) // не защищено от SQL-инъекций
                {
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        result = new Page
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                            PageType = reader.GetString(reader.GetOrdinal("page_type")),
                            PrivacyType = reader.GetInt16(reader.GetOrdinal("privacy_type")), // поменять на менее требовательный, если получится
                            CustomCss = reader.GetString(reader.GetOrdinal("custom_css"))
                        };
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<Page>> GetPagesByQuery(string sqlQuery)
        {
            var result = new List<Page>();
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        // Создаем и добавляем в результат пользователей (объекты), которых получаем
                        result.Add(new Page
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                            PageType = reader.GetString(reader.GetOrdinal("page_type")),
                            PrivacyType = reader.GetInt16(reader.GetOrdinal("privacy_type")), // поменять на менее требовательный, если получится
                            CustomCss = reader.GetString(reader.GetOrdinal("custom_css"))
                        });
                    }
                }
            }
            return result;
        }

        public async Task DeletePage(Guid id)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                using (var command = new NpgsqlCommand($"DELETE FROM public.pages WHERE id = '{id}'", connection)) // не защищено от SQL-инъекций
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task<IEnumerable<Page>> GetAll()
        {
            return await GetPagesByQuery("SELECT * FROM public.pages");
        }

        public async Task AddPage(Page page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                // не защищено от SQL-инъекций
                using (var command = new NpgsqlCommand(
                    $"INSERT INTO public.pages (id, user_id, privacy_type, custom_css) VALUES ('{Guid.NewGuid()}', '{page.UserId}', '{page.PrivacyType}', '{page.CustomCss}');", connection)) 
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdatePage(Page page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("MainConnectionString")))
            {
                // не защищено от SQL-инъекций
                using (var command = new NpgsqlCommand(
                    $"UPDATE public.pages SET user_id = '{page.UserId}', page_type = '{page.PageType}', privacy_type = '{page.PrivacyType}', custom_css = '{page.CustomCss}' WHERE id = '{page.Id}';", connection))
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
