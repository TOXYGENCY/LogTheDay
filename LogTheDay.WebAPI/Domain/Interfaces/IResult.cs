namespace LogTheDay.LogTheDay.WebAPI.Domain.Interfaces
{
    public interface IResult<T>
    {
        public bool Success { get; }
        public T? Content { get; }
        public string? Message { get; }
    }
}
