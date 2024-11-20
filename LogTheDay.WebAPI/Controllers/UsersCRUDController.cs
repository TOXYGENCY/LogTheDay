using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Controllers
{
    // Контроллер, который слушает API каналы на HTTP запросы, и вызывает нужные методы из UsersSQLRepository
    [Route("api/v1/users-crud")]
    [ApiController]
    public class UsersCRUDController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        ILogger<UsersCRUDController> logger;

        // Вставляем репозиторий напрямую - без сервиса IUsersService usersService
        public UsersCRUDController(IUsersRepository usersRepository, ILogger<UsersCRUDController> logger)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _usersRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            try
            {
                var result = await _usersRepository.GetUserByIdAsync(id);
                if (result == null){return BadRequest($"Нет пользователя с ID: {id}");}
                return Ok(result);
            }
            catch (Exception ex)
            {
                {
                    logger.LogError(ex, "error");
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet("query")] // TODO: убрать контроллер, потому что он тестовый
        public async Task<IActionResult> GetUsersByQueryAsync(string name = null, string email = null, DateOnly? regDate = null)
        {
            try
            {
                var result = await _usersRepository.GetUsersByQueryAsync(name, email, regDate);
                if (result == null){return BadRequest($"Нет пользователей с: {name}, {email}, {regDate}");}
                return Ok(result);
            }
            catch (Exception ex)
            {
                {
                    logger.LogError(ex, "error");
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            try
            {
                await _usersRepository.DeleteUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                {
                    logger.LogError(ex, "error");
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(User user)
        {
            try
            {
                await _usersRepository.AddUserAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ReplaceUserAsync(User user)
        {
            try
            {
                await _usersRepository.ReplaceUserAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
