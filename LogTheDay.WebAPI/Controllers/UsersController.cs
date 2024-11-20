using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
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
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        //TODO: доделать вообще все - пока что тут все наугад
        [HttpPost("auth")] 
        public async Task<IActionResult> AuthenticateAsync(string login, string PasswordHash)
        {
            await usersService.AuthenticateAsync(login, PasswordHash);
            return Ok();
        }

        [HttpPost("reg")]
        public async Task<IActionResult> RegisterNewUserAsync(string Name, string Email, string PasswordHash)
        {
            try
            {
                await usersService.RegisterNewUserAsync(Name, Email.ToLower(), PasswordHash);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{id}/ChangeName")]
        public async Task<IActionResult> ChangeNameAsync(Guid id, string NewName)
        {
            try
            {
                await usersService.ChangeNameAsync(id, NewName);
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
