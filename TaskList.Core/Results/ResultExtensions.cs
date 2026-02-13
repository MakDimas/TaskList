using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskList.Core.Results;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return result.ErrorType switch
        {
            ResultErrorType.NotFound =>
                new NotFoundObjectResult(result.Error),

            ResultErrorType.Validation =>
                new BadRequestObjectResult(result.Error),

            ResultErrorType.Forbidden =>
                new ObjectResult(result.Error)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                },

            _ =>
                new BadRequestObjectResult(result.Error)
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return result.ErrorType switch
        {
            ResultErrorType.NotFound =>
                new NotFoundObjectResult(result.Error),

            ResultErrorType.Validation =>
                new BadRequestObjectResult(result.Error),

            ResultErrorType.Forbidden =>
                new ObjectResult(result.Error)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                },

            _ =>
                new BadRequestObjectResult(result.Error)
        };
    }
}
