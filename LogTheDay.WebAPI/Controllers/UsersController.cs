using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

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
            Result<User> userRes = await _usersRepository.GetByIdAsync(id);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            Result<None> deletionRes = await _usersRepository.DeleteAsync(id); ;
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
        public async Task<IActionResult> ReplaceAsync(User user)
        {
            Result<None> replacementRes = await _usersRepository.ReplaceAsync(user);
            if (replacementRes.Success)
            {
                return Ok(replacementRes.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, replacementRes.Message);
            }
        }
        // TODO: Почитать про безопасность https://learn.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-security-guidance
        [HttpGet("odata")] 
        [EnableQuery]
        public IQueryable<User> GetByODataQuery()
        {
            var odataRes = _usersRepository.GetByODataQuery();
            if (odataRes.Success)
                return odataRes.Content;
            else 
                return null;
        }
    }
}
