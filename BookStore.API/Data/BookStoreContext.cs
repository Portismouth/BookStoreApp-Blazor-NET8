using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Data;

public partial class BookStoreContext : IdentityDbContext<ApiUser>
{
    public BookStoreContext()
    {
    }

    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;database=bookstore;user=sa;password=0zzyG0nz;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(60);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(e => e.Isbn, "UQ__Books__9271CED090C0A4BA").IsUnique();

            entity.Property(e => e.Image).IsRequired();
            entity.Property(e => e.Isbn)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Summary)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasForeignKey(d => d.AuthorId);
        });

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "ce392ebd-fdf9-4d12-a950-75db07abc220"
            },
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = "223f9400-50fc-4e38-9195-1ef87b0fb751"
            }
        );

        var hasher = new PasswordHasher<ApiUser>();

        modelBuilder.Entity<ApiUser>().HasData(
            new ApiUser
            {
                Id = "41be71ef-cc46-4a39-9e3e-4efd48aa052e",
                Email = "admin@bookstore.com",
                NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                UserName = "admin@bookstore.com",
                NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                FirstName = "System",
                LastName = "Admin",
                PasswordHash = hasher.HashPassword(null, "password")
            },
            new ApiUser
            {
                Id = "1c1d9951-3588-4a41-a061-b6b75491f740",
                Email = "user@bookstore.com",
                NormalizedEmail = "USER@BOOKSTORE.COM",
                UserName = "user@bookstore.com",
                NormalizedUserName = "USER@BOOKSTORE.COM",
                FirstName = "System",
                LastName = "User",
                PasswordHash = hasher.HashPassword(null, "password")
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "ce392ebd-fdf9-4d12-a950-75db07abc220",
                UserId = "1c1d9951-3588-4a41-a061-b6b75491f740"
            },
            new IdentityUserRole<string>
            {
                RoleId = "223f9400-50fc-4e38-9195-1ef87b0fb751",
                UserId = "41be71ef-cc46-4a39-9e3e-4efd48aa052e"
            }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
