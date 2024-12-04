using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LogTheDay.LogTheDay.WebAPI.Controllers
{
    // Контроллер, который слушает API каналы на HTTP запросы, и вызывает нужные методы из UsersRepository
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        //ILogger<UsersController> logger;
        IUsersService usersService;

        public UsersController(IUsersRepository usersRepository, IUsersService usersService) //, ILogger<UsersController> logger)
        {
            this._usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            //this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthenticateAsync(string email, string passwordString)
        {
            Result<bool> authRes = await usersService.AuthenticateAsync(email, passwordString);
            if (authRes.Success)
            {
                return Ok(authRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, authRes.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUserAsync(string name, string email, string passwordString)
        {
            Result<None> registrationRes = await usersService.RegisterNewUserAsync(name, email.ToLower(), passwordString);
            if (registrationRes.Success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, registrationRes.Message);
            }
        }

        [HttpPost("{id}/nameChange")]
        public async Task<IActionResult> ChangeNameAsync(Guid id, string newName)
        {
            Result<None> nameChangeRes = await usersService.ChangeNameAsync(id, newName);
            if (nameChangeRes.Success)
            {
                return Ok(nameChangeRes.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, nameChangeRes.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            Result<IEnumerable<User>> usersArrRes = await _usersRepository.GetAllAsync();
            if (usersArrRes.Success)
            {
                return Ok(usersArrRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, usersArrRes.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            Result<User> userRes = await _usersRepository.GetUserByIdAsync(id);
            if (userRes.Success)
            {
                if (userRes.Content == null) return BadRequest($"Нет пользователя с ID: {id}");
                return Ok(userRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, userRes.Message);
            }
        }

        [HttpGet("query")] // TODO?: убрать контроллер, потому что он тестовый
        public async Task<IActionResult> GetUsersByQueryAsync(string name = null, string email = null, DateOnly? regDate = null)
        {
            Result<IEnumerable<User>> usersArrRes = await _usersRepository.GetUsersByQueryAsync(name, email, regDate);
            if (usersArrRes.Success)
            {
                if (usersArrRes.Content == null) return BadRequest($"Нет пользователей с: {name}, {email}, {regDate}");
                return Ok(usersArrRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, usersArrRes.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            Result<None> deletionRes = await _usersRepository.DeleteUserAsync(id); ;
            if (deletionRes.Success)
            {
                return Ok(deletionRes.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, deletionRes.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ReplaceUserAsync(User user)
        {
            Result<None> replacementRes = await _usersRepository.ReplaceUserAsync(user);
            if (replacementRes.Success)
            {
                return Ok(replacementRes.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, replacementRes.Message);
            }
        }
    }
}
