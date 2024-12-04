using System;
using System.Collections.Generic;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities;

public partial class Tag
{
    public Guid Id { get; set; }

    public Guid PageId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
