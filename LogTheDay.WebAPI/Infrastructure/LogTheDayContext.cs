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
            entity.Property(e => e.BgColor)
                .HasMaxLength(20)
                .HasDefaultValueSql("'rgb(28 28 28)'::character varying")
                .HasColumnName("bg_color");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.CreationTime).HasColumnName("creation_time");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.Priority)
                .HasDefaultValue(5)
                .HasColumnName("priority");
            entity.Property(e => e.Tags)
                .HasColumnType("character varying[]")
                .HasColumnName("tags");
            entity.Property(e => e.TextColor)
                .HasMaxLength(20)
                .HasDefaultValueSql("'rgb(255 255 255)'::character varying")
                .HasColumnName("text_color");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.Page).WithMany(p => p.Notes)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("page_id_fk");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pages_id_pk");

            entity.ToTable("pages");

            entity.HasIndex(e => e.Id, "fki_user_content_pages_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CustomCss)
                .HasColumnType("character varying")
                .HasColumnName("custom_css");
            entity.Property(e => e.PageType)
                .HasDefaultValueSql("'plain'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("page_type");
            entity.Property(e => e.PrivacyType)
                .HasDefaultValue(0)
                .HasColumnName("privacy_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Pages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_id_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_id_pk");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasColumnType("character varying")
                .HasColumnName("password_hash");
            entity.Property(e => e.RegDate).HasColumnName("reg_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
