namespace LogTheDay.LogTheDay.WebAPI.Domain.Exceptions
{
    public class EmailTakenException : Exception
    {
        public EmailTakenException(string Email) : base($"Пользователь с E-mail \"{Email}\" уже существует.") { }
    }
}
