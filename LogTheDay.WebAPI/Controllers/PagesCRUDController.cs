using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.Controllers
{
    // Контроллер, который слушает API каналы на HTTP запросы, и вызывает нужные методы из PagesSQLRepository
    [Route("api/v1/pages-crud")]
    [ApiController]
    public class PagesCRUDController : ControllerBase
    {
        private readonly IPagesRepository _pagesRepository;
        // Вставляем репозиторий напрямую - без сервиса
        public PagesCRUDController(IPagesRepository pagesRepository)
        {
            _pagesRepository = pagesRepository ?? throw new ArgumentNullException(nameof(pagesRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _pagesRepository.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPageById(Guid id)
        {
            try
            {
                var result = await this._pagesRepository.GetPageById(id);
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
        public async Task<IActionResult> DeletePage(Guid id)
        {
            try
            {
                await this._pagesRepository.DeletePage(id);
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
        public async Task<IActionResult> AddPage(Page page)
        {
            try
            {
                await _pagesRepository.AddPage(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePage(Page page)
        {
            try
            {
                await _pagesRepository.UpdatePage(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
