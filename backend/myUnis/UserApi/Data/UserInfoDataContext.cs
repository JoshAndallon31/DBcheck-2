
using UserApi.Models;

namespace UserApi.Data;
using Microsoft.EntityFrameworkCore;

public class UserInfoDataContext : DbContext
{
    protected readonly IConfiguration _config;
    public DbSet<UserInfos> UserInfos { get; set; }

    public UserInfoDataContext(
        DbContextOptions<UserInfoDataContext> options,
        IConfiguration configuration) : base(options)
    {
        _config = configuration;
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_config
            .GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfos>(p =>
        {
            p.ToTable("UsersInfos");
            p.HasKey(x => x.UserId);
            p.HasIndex(u => u.UserName).IsUnique();
            p.Property(x => x.PasswordSalt).HasMaxLength(255);
        });
    }
}