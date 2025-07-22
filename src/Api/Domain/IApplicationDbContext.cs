using Api.Domain.Entities;
using Api.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain;

public interface IApplicationDbContext
{
    DbSet<Quote> Quotes { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}