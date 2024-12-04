using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Controllers
{
    // Контроллер, который слушает API каналы на HTTP запросы, и вызывает нужные методы из PagesSQLRepository
    [Route("api/v1/pages-crud")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly IPagesRepository _pagesRepository;
        // Вставляем репозиторий напрямую - без сервиса
        public PagesController(IPagesRepository pagesRepository)
        {
            _pagesRepository = pagesRepository ?? throw new ArgumentNullException(nameof(pagesRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _pagesRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPageByIdAsync(Guid id)
        {
            try
            {
                var result = await _pagesRepository.GetPageByIdAsync(id);
                if (result == null)
                {
                    return BadRequest($"Нет листа с ID: {id}");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePageAsync(Guid id)
        {
            try
            {
                await _pagesRepository.DeletePageAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPageAsync(Page page)
        {
            try
            {
                await _pagesRepository.AddPageAsync(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ReplacePageAsync(Page page)
        {
            try
            {
                await _pagesRepository.ReplacePageAsync(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
