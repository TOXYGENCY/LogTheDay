using System;
using System.Collections.Generic;
using LogTheDay.LogTheDay.WebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogTheDay.LogTheDay.WebAPI.Infrastructure;

public partial class LogTheDayContext : DbContext
{
    public LogTheDayContext()
    {
    }

    public LogTheDayContext(DbContextOptions<LogTheDayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:MainConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attachments_id_pk");

            entity.ToTable("attachments");

            entity.HasIndex(e => e.NoteId, "fki_note_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ContentLink)
                .HasColumnType("character varying")
                .HasColumnName("content_link");
            entity.Property(e => e.FileType)
                .HasColumnType("character varying")
                .HasColumnName("file_type");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.Type)
                .HasDefaultValueSql("'file'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("type");

            entity.HasOne(d => d.Note).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("note_id_fk");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("note_id_pk");

            entity.ToTable("notes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.CreationTime).HasColumnName("creation_time");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IconLink)
                .HasColumnType("character varying")
                .HasColumnName("icon_link");
            entity.Property(e => e.LastModifiedDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_modified_date");
            entity.Property(e => e.LastModifiedTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_modified_time");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.PrimaryColor)
                .HasColumnType("character varying")
                .HasColumnName("primary_color");
            entity.Property(e => e.Priority)
                .HasDefaultValue(5)
                .HasColumnName("priority");
            entity.Property(e => e.Score)
                .HasDefaultValue(1)
                .HasColumnName("score");
            entity.Property(e => e.SecondaryColor)
                .HasColumnType("character varying")
                .HasColumnName("secondary_color");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.Page).WithMany(p => p.Notes)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("page_id_fk");

            entity.HasMany(d => d.Tags).WithMany(p => p.Notes)
                .UsingEntity<Dictionary<string, object>>(
                    "NotesTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tag_id_fk"),
                    l => l.HasOne<Note>().WithMany()
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("note_id_fk"),
                    j =>
                    {
                        j.HasKey("NoteId", "TagId").HasName("note_tag_pk");
                        j.ToTable("notes_tags");
                        j.IndexerProperty<Guid>("NoteId").HasColumnName("note_id");
                        j.IndexerProperty<Guid>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pages_id_pk");

            entity.ToTable("pages");

            entity.HasIndex(e => e.Id, "fki_user_content_pages_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("creation_date");
            entity.Property(e => e.CreationTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_time");
            entity.Property(e => e.CustomCss)
                .HasColumnType("character varying")
                .HasColumnName("custom_css");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'Description...'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IconLink)
                .HasColumnType("character varying")
                .HasColumnName("icon_link");
            entity.Property(e => e.LastModifiedDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_modified_date");
            entity.Property(e => e.LastModifiedTime)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_modified_time");
            entity.Property(e => e.PrivacyType)
                .HasDefaultValue(0)
                .HasColumnName("privacy_type");
            entity.Property(e => e.Title)
                .HasDefaultValueSql("'Page'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasDefaultValueSql("'plain'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Pages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_id_fk");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_id_pk");

            entity.ToTable("tags");

            entity.HasIndex(e => e.Title, "tags_title_key").IsUnique();

            entity.HasIndex(e => new { e.PageId, e.Title }, "unique_tag").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_id_pk");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AvatarImg)
                .HasColumnType("character varying")
                .HasColumnName("avatar_img");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.LastLoginDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_login_date");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasColumnType("character varying")
                .HasColumnName("password_hash");
            entity.Property(e => e.RegDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("reg_date");
            entity.Property(e => e.RegTime)
                .HasDefaultValueSql("now()")
                .HasColumnName("reg_time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
