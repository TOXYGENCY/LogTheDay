using LogTheDay.Controllers.Domain.Interfaces;
using LogTheDay.Controllers.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LogTheDay.Controllers
{
    [Route("api/v1/users-crud")]
    [ApiController]
    public class UsersCRUDController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        // Вставляем репозиторий напрямую - без сервиса
        public UsersCRUDController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _usersRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //public async Task<User> AddUser(User user)
        //{
        //    return Ok(this._usersRepository.AddUser(User));
        //}
        //public async Task<User> GetUserById(Guid id);
        //public async Task<IEnumerable<User>> GetUsersByQuery(string query);
        //public async Task DeleteUser(Guid id);
    }
}
