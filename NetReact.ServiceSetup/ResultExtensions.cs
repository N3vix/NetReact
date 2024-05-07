using Microsoft.AspNetCore.Http;
using Models;

namespace NetReact.ServiceSetup;

public static class ResultExtensions
{
    public static IResult UnpuckResult<TError>(this Result<TError> result)
    {
        return result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(new { Error = result.Error });
    }

    public static IResult UnpuckResult<TValue, TError>(
        this Result<TValue, TError> result,
        Func<TValue, object> valueBuilder = null)
    {
        return result.IsSuccess
            ? Results.Ok(valueBuilder == null
                ? result.Value
                : valueBuilder(result.Value))
            : Results.BadRequest(new { Error = result.Error });
    }
}