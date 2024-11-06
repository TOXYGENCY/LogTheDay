﻿using System;
using System.Collections.Generic;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities;

public partial class Note
{
    public Guid Id { get; set; }

    public Guid PageId { get; set; }

    public DateOnly CreationDate { get; set; }

    public TimeOnly CreationTime { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public List<string>? Tags { get; set; }

    public int? Priority { get; set; }

    public string? BgColor { get; set; }

    public string? TextColor { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Page Page { get; set; } = null!;
}