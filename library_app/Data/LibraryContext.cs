using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace library_app.Data;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Borrowing> Borrowings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

   /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=library;User=cf4user;Password=****;MultipleActiveResultSets=True;TrustServerCertificate=True");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Greek_100_CI_AI");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BOOKS__3214EC27EE732F92");

            entity.ToTable("BOOKS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AUTHOR");
            entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("POSITION");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__BOOKS__CATEGORY___398D8EEE");
        });

        modelBuilder.Entity<Borrowing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BORROWIN__3214EC2711A367AD");

            entity.ToTable("BORROWINGS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("BOOK_ID");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("END_DATE");
            entity.Property(e => e.MemberId).HasColumnName("MEMBER_ID");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("START_DATE");

            entity.HasOne(d => d.Book).WithMany(p => p.Borrowings)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BORROWING__BOOK___45F365D3");

            entity.HasOne(d => d.Member).WithMany(p => p.Borrowings)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__BORROWING__MEMBE__44FF419A");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CATEGORI__3214EC2772D13D5F");

            entity.ToTable("CATEGORIES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_NAME");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MEMBERS__3214EC2738CB363C");

            entity.ToTable("MEMBERS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SUBSCRIP__3214EC27F2ED68D8");

            entity.ToTable("SUBSCRIPTION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Alowance).HasColumnName("ALOWANCE");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("END_DATE");
            entity.Property(e => e.MemberId).HasColumnName("MEMBER_ID");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("START_DATE");

            entity.HasOne(d => d.Member).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK__SUBSCRIPT__MEMBE__4222D4EF");
        });

        modelBuilder.Entity<SubscriptionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SUBSCRIP__3214EC270600DEFD");

            entity.ToTable("SUBSCRIPTION_TYPES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Allowance).HasColumnName("ALLOWANCE");
            entity.Property(e => e.Duration).HasColumnName("DURATION");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USERS__3214EC27FB99C996");

            entity.ToTable("USERS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Surname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("SURNAME");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
