using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Punch> Punches { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(a => a.Id);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.FullName).HasMaxLength(200);
            entity.Property(u => u.Email).HasMaxLength(150);
        });

        modelBuilder.Entity<Punch>(entity =>
        {
            entity.ToTable("Punches");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(p => p.Timestamp).HasColumnName("timestamp").IsRequired();
            entity.Property(p => p.Type).HasColumnName("type").IsRequired();
            entity.Property(p => p.Latitude).HasColumnName("latitude");
            entity.Property(p => p.Longitude).HasColumnName("longitude");
            entity.Property(p => p.Address).HasColumnName("address");
            entity.Property(p => p.ManuallyEdited).HasColumnName("manually_edited").HasDefaultValue(false);
            entity.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(p => p.User)
                  .WithMany()
                  .HasForeignKey(p => p.UserId);
        });
    }
}
