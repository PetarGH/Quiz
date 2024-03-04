using System;
using System.Collections.Generic;
using System.Configuration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Domain.Data;

public partial class Dbi477163Context : DbContext
{
    protected readonly IConfiguration Configuration;

    public Dbi477163Context(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public Dbi477163Context()
    {
       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Configuration.GetConnectionString("ContextCS");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }


    public virtual DbSet<IpAnswer> IpAnswers { get; set; }

    public virtual DbSet<IpCategory> IpCategories { get; set; }

    public virtual DbSet<IpQuestion> IpQuestions { get; set; }

    public virtual DbSet<IpQuiz> IpQuizzes { get; set; }

    public virtual DbSet<IpUser> IpUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IpAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ip_answers");

            entity.HasIndex(e => e.QuestionId, "question_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(150)")
                .HasColumnName("ID");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.QuestionId)
                .HasColumnType("int(11)")
                .HasColumnName("question_id");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");

            entity.HasOne(d => d.Question).WithMany(p => p.IpAnswers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("ip_answers_key");
        });

        modelBuilder.Entity<IpCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ip_category");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.HasIndex(e => e.ParentId, "parentkey");

            entity.Property(e => e.Id)
                .HasColumnType("int(50)")
                .HasColumnName("ID");
            entity.Property(e => e.ParentId)
                .HasColumnType("int(50)")
                .HasColumnName("parentID");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("parentkey");
        });

        modelBuilder.Entity<IpQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ip_questions");

            entity.HasIndex(e => e.QuizId, "quiz_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(150)")
                .HasColumnName("ID");
            entity.Property(e => e.QuizId)
                .HasColumnType("int(11)")
                .HasColumnName("quiz_id");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");

            entity.HasOne(d => d.Quiz).WithMany(p => p.IpQuestions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("key");
        });

        modelBuilder.Entity<IpQuiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ip_quizzes");

            entity.HasIndex(e => e.Categoryid, "categoryid");

            entity.HasIndex(e => e.CreatedBy, "created_by");

            entity.Property(e => e.Id)
                .HasColumnType("int(100)")
                .HasColumnName("ID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("int(22)")
                .HasColumnName("categoryid");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("int(255)")
                .HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.IpQuizzes)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("ip_quizzes_ibfk_1");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.IpQuizzes)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("ip_quizzes_ibfk_2");
        });

        modelBuilder.Entity<IpUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ip_users");

            entity.Property(e => e.Id)
                .HasColumnType("int(150)")
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Age).HasColumnType("int(2)");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(255);
            entity.Property(e => e.UserType).HasColumnName("user_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
