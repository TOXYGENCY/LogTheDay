using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace LogTheDay.LogTheDay.WebAPI.Controllers
{
    [Route("api/v1/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _notesRepository;
        INotesService notesService;

        public NotesController(INotesRepository notesRepository, INotesService notesService)
        {
            this._notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
            this.notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        }


        [HttpPost]
        public async Task<IActionResult> AddNoteAsync(Note note)
        {
            Result<None> creationRes = await _notesRepository.AddAsync(note);
            if (creationRes.Success)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, creationRes.Message);
            }
        }

        //[HttpPost("{id}/infoChange")]
        //public async Task<IActionResult> ChangeInfoAsync(Guid id, string newTitle, string newDescription)
        //{
        //    Result<Note> noteRes = await _notesRepository.GetByIdAsync(id);
        //    if (noteRes.Success) if (noteRes.Content == null) return BadRequest($"Нет {nameof(Note)} с ID: {id}");

        //    Result<None> infoChangeRes = await _notesRepository.ChangeInfoAsync(noteRes.Content, newTitle, newDescription);
        //    if (infoChangeRes.Success)
        //    {
        //        return Ok(infoChangeRes.Message);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, infoChangeRes.Message);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            Result<IEnumerable<Note>> notesArrRes = await _notesRepository.GetAllAsync();
            if (notesArrRes.Success)
            {
                return Ok(notesArrRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, notesArrRes.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Result<Note> noteRes = await _notesRepository.GetByIdAsync(id);
            if (noteRes.Success)
            {
                if (noteRes.Content == null) return BadRequest($"Нет {nameof(Note)} с ID: {id}");
                return Ok(noteRes.Content);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, noteRes.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            Result<None> deletionRes = await _notesRepository.DeleteAsync(id); ;
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
        public async Task<IActionResult> ReplaceNoteAsync(Note note)
        {
            Result<None> replacementRes = await _notesRepository.ReplaceAsync(note);
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
        public IQueryable<Note> GetByODataQuery()
        {
            var odataRes = _notesRepository.GetByODataQuery();
            if (odataRes.Success)
                return odataRes.Content;
            else
                return null;
        }
    }
}
