using ClubApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubApi.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly EntityInterceptor _entityInterceptor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, EntityInterceptor entityInterceptor) : base(options)
    {
        _entityInterceptor = entityInterceptor;
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ClientType> ClientTypes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<AccessLog> AccessLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_entityInterceptor);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}