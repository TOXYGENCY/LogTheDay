using System;
using System.Collections.Generic;

namespace LogTheDay.LogTheDay.WebAPI.Domain.Entities;

public partial class Note
{
    public Guid Id { get; set; }

    public Guid PageId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public int? Priority { get; set; }

    public int? Score { get; set; }

    public string? IconLink { get; set; }

    public DateOnly CreationDate { get; set; }

    public TimeOnly CreationTime { get; set; }

    public DateOnly LastModifiedDate { get; set; }

    public DateTime LastModifiedTime { get; set; }

    public string? PrimaryColor { get; set; }

    public string? SecondaryColor { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Page Page { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
