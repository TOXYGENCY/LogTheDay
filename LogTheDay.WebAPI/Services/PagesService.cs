using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;
using LogTheDay.LogTheDay.WebAPI.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace LogTheDay.LogTheDay.WebAPI.Services
{
    public class PagesService : IPagesService
    {
        private readonly IPagesRepository _pagesRepository;
        ILogger<PagesService> logger;
        public PagesService(IPagesRepository pagesRepository, ILogger<PagesService> logger)
        {
            this._pagesRepository = pagesRepository ?? throw new ArgumentNullException(nameof(pagesRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<Result<None>> ChangeInfoAsync(Guid id, string newTitle, string newDescription)
        {
            if (string.IsNullOrEmpty(newTitle) || string.IsNullOrEmpty(newDescription))
            {
                string message = $"Не все аргументы переданы.";
                logger.LogWarning(message);
                return new Result<None>(false, null, message);
            }

            Result<Page> pageRes = await _pagesRepository.GetByIdAsync(id);
            if (!pageRes.Success || pageRes.Content == null)
            {
                return new Result<None>(false, null, pageRes.Message);
            }
            Page? page = pageRes.Content;

            Result<None> nameChangeRes = await _pagesRepository.ChangeInfoAsync(page, newTitle, newDescription);
            if (!nameChangeRes.Success)
            {
                return new Result<None>(false, null, nameChangeRes.Message);
            }
            await _pagesRepository.UpdateLastModDate(page);
            return new Result<None>(true, null, nameChangeRes.Message);
        }
    }
}
