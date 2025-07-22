using Api.CustomExceptions;
using Api.Endpoints;
using Api.Extensions; // <-- Import the ResultExtensions
using MediatR;
using SharedKernel;

public static class GetTestUsingResultsPattern
{
    public record Query(int Id) : IRequest<Result<Response>>;
    public record Response(string Message);

    public class Handler : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request.Id % 2 != 0)
            {
                var error = Error.NotFound("Test.NotFound", $"No test object with the odd ID '{request.Id}' was found.");
                return Result.Failure<Response>(error);
            }
            var response = new Response($"Hello #{request.Id}");
            await Task.Delay(10); // Simulate async work
            return Result.Success(response);
        }
    }
}
public class GetTestUsingResultsPatternEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/resulttest/{id:int}", async (ISender sender, int id) =>
        {
            var query = new GetTest.Query(id);

            Result<GetTest.Response> result = await sender.Send(query);

            return result.Match(
                Results.Ok,
                CustomResults.Problem);


        }).WithTags("Test");
    }
}