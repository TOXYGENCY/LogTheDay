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

        [HttpPost("auth")] // Из сайта получим логин и уже захешированный пароль для сравнения с бд
        public async Task<IActionResult> AuthenticateAsync(string login, string password_hash)
        {
            await usersService.AuthenticateAsync(login, password_hash);
            return Ok();
        }
    }
}
