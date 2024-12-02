using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Exceptions;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogTheDay.LogTheDay.WebAPI.Controllers
{
    // Контроллер, который слушает API каналы на HTTP запросы, и вызывает нужные методы из UsersSQLRepository
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        ILogger<UsersController> logger;
        IUsersService usersService;

        public UsersController(IUsersRepository usersRepository, IUsersService usersService, ILogger<UsersController> logger)
        {
            this._usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthenticateAsync(string email, string passwordString)
        {
            try
            {
                return Ok(await usersService.AuthenticateAsync(email, passwordString));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // TODO: избавиться от промежуточных исключений и перейти к передаче ответа как объекта
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUserAsync(string name, string email, string passwordString)
        {
            try
            {
                await usersService.RegisterNewUserAsync(name, email.ToLower(), passwordString);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (EmailTakenException ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{id}/nameChange")]
        public async Task<IActionResult> ChangeNameAsync(Guid id, string newName)
        {
            try
            {
                await usersService.ChangeNameAsync(id, newName);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
                if (result == null) return BadRequest($"Нет пользователя с ID: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("query")] // TODO?: убрать контроллер, потому что он тестовый
        public async Task<IActionResult> GetUsersByQueryAsync(string name = null, string email = null, DateOnly? regDate = null)
        {
            try
            {
                var result = await _usersRepository.GetUsersByQueryAsync(name, email, regDate);
                if (result == null) { return BadRequest($"Нет пользователей с: {name}, {email}, {regDate}"); }
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
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
