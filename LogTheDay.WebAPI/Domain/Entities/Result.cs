using LogTheDay.LogTheDay.WebAPI.Domain.Interfaces;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities
{
    // Класс нужен для контроля результатов исполнения методов,
    // а точнее, избавление от излишней обработки ожидаемых (частых) ошибок,

    // Пример: вместо ArgumentNullException("Поле пустое"), возвращать Result<None>(false, null,  "Поле пустое")
    public class Result<T> : IResult<T>
    {
        public bool Success { get; }
        public T? Content { get; }
        public string? Message { get; }
        public Result(bool success, T? content, string? message = null)
        {
            Success = success;
            Content = content;
            Message = message;
        }
    }
}
