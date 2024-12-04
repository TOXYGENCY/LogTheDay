using System;
using System.Collections.Generic;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? AvatarImg { get; set; }

    public DateOnly RegDate { get; set; }

    public TimeOnly RegTime { get; set; }

    public DateOnly LastLoginDate { get; set; }

    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();
}
