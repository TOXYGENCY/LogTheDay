﻿namespace LogTheDay.Controllers.Domain.Entities
{
    public class User // dummy-class for users
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateOnly RegistrationDate { get; set; }
    }
}
