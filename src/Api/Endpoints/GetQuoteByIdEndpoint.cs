using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class GetQuoteById
{
    public record Query(int id) : IRequest<Response>;
    public record Response(string Content, string Author);

    public class Handler(AppDbContext context) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var quote = await context.Quotes.FindAsync(request.id);
            if (quote == null) return null;
            return new Response(quote.Content, quote.Author);
        }
    }

    public class GetQuoteByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/quotes/{id:int}", async (ISender sender, int id) =>
            {
                var response = await sender.Send(new GetQuoteById.Query(id));
                return (response == null)
                    ? Results.NotFound()
                    : Results.Ok(response);
            });
        }
    }
}