using System;
using System.Collections.Generic;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities;

public partial class Attachment
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string Type { get; set; } = null!;

    public string? FileType { get; set; }

    public Guid NoteId { get; set; }

    public virtual Note Note { get; set; } = null!;
}
