// Namespaces from your other examples
using Api.Endpoints;
using MediatR;
using SharedKernel;

public static class GetTest
{
    // The query that triggers the operation
    public record Query(int Id) : IRequest<Result<Response>>;

    // The successful response DTO
    public record Response(string Message);

    // The handler containing the actual business logic
    public class Handler : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Simulate a business rule: only even IDs are valid.
            if (request.Id % 2 != 0)
            {
                // ----> USAGE OF THE RESULT PATTERN (FAILURE) <----
                // Instead of returning null or throwing an exception, we return a structured failure Result.
                var error = Error.NotFound("Test.NotFound", $"No test object with the odd ID '{request.Id}' was found.");
                return Result.Failure<Response>(error);
            }

            // The "happy path" or success case
            var response = new Response($"Hello #{request.Id}");

            // ----> USAGE OF THE RESULT PATTERN (SUCCESS) <----
            // We wrap the successful response in a Result object.
            await Task.Delay(10); // Simulate async work
            return Result.Success(response);
        }
    }
}
// Still in your Api.Endpoints namespace
// Assuming the GetTest static class is in the same file or accessible
public class GetTestEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/test/{id:int}", async (ISender sender, int id) =>
        {
            // 1. Create the query from the route parameter
            var query = new GetTest.Query(id);

            // 2. Send to MediatR and get the Result object back
            Result<GetTest.Response> result = await sender.Send(query);

            // 3. Inspect the result and map it to an HTTP response
            if (result.IsFailure)
            {
                // We can now use the structured error information
                return MapErrorToHttpResult(result.Error);
            }

            // On success, return the value inside the Result
            return Results.Ok(result.Value);

        }).WithTags("Test");
    }

    // A helper function to map your custom Error types to HTTP Status Codes
    private static IResult MapErrorToHttpResult(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => Results.NotFound(error),
            ErrorType.Validation => Results.BadRequest(error),
            ErrorType.Conflict => Results.Conflict(error),
            // Default to a 500 internal server error for unhandled problem types
            _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}