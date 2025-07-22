using Api.Domain;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

    public DbSet<Quote> Quotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}