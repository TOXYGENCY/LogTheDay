using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using LogTheDay.LogTheDay.WebAPI.Infrastructure;
using LogTheDay.LogTheDay.WebAPI.Services;
using Microsoft.AspNetCore.OData.Query;

namespace LogTheDay.LogTheDay.WebAPI.Controllers
{
    // Контроллер, который слушает API каналы на HTTP запросы, и вызывает нужные методы из PagesRepository
    [Route("api/v1/pages")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly IPagesRepository _pagesRepository;
        IPagesService pagesService;

        public PagesController(IPagesRepository pagesRepository, IPagesService pagesService)
        {
            this._pagesRepository = pagesRepository ?? throw new ArgumentNullException(nameof(pagesRepository));
            this.pagesService = pagesService ?? throw new ArgumentNullException(nameof(pagesService));
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewPageAsync(Page page)
        {
            Result<None> creationRes = await _pagesRepository.AddAsync(page);
            if (creationRes.Success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, creationRes.Message);
            }
        }

        [HttpPost("{id}/infoChange")]
        public async Task<IActionResult> ChangeInfoAsync(Guid id, string newTitle, string newDescription)
        {
            Result<Page> pageRes = await _pagesRepository.GetByIdAsync(id);
            if (pageRes.Success) if (pageRes.Content == null) return BadRequest($"Нет {nameof(Page)} с ID: {id}");

            Result<None> infoChangeRes = await _pagesRepository.ChangeInfoAsync(pageRes.Content, newTitle, newDescription);
            if (infoChangeRes.Success)
            {
                return Ok(infoChangeRes.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, infoChangeRes.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            Result<IEnumerable<Page>> pagesArrRes = await _pagesRepository.GetAllAsync();
            if (pagesArrRes.Success)
            {
                return Ok(pagesArrRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, pagesArrRes.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Result<Page> pageRes = await _pagesRepository.GetByIdAsync(id);
            if (pageRes.Success)
            {
                if (pageRes.Content == null) return BadRequest($"Нет {nameof(Page)} с ID: {id}");
                return Ok(pageRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, pageRes.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            Result<None> deletionRes = await _pagesRepository.DeleteAsync(id); ;
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
        public async Task<IActionResult> ReplacePageAsync(Page page)
        {
            Result<None> replacementRes = await _pagesRepository.ReplaceAsync(page);
            if (replacementRes.Success)
            {
                return Ok(replacementRes.Message);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, replacementRes.Message);
            }
        }

        [HttpGet("odata")]
        [EnableQuery]
        public IQueryable<Page> GetByODataQuery()
        {
            var odataRes = _pagesRepository.GetByODataQuery();
            if (odataRes.Success)
                return odataRes.Content;
            else
                return null;
        }
    }
}
