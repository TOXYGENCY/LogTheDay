using System;
using System.Collections.Generic;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities;

public partial class Page
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? IconLink { get; set; }

    public string Type { get; set; } = null!;

    public int PrivacyType { get; set; }

    public string? CustomCss { get; set; }

    public DateOnly CreationDate { get; set; }

    public DateTime CreationTime { get; set; }

    public DateOnly LastModifiedDate { get; set; }

    public DateTime LastModifiedTime { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual User User { get; set; } = null!;
}
