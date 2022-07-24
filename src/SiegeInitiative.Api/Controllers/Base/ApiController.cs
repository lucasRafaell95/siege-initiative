using Microsoft.AspNetCore.Mvc;
using SiegeInitiative.DataContracts.OperationResult;
using SiegeInitiative.DataContracts.OperationResult.Base;
using System.Runtime.CompilerServices;

namespace SiegeInitiative.Api.Controllers.Base;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private readonly ILogger logger;

    protected ApiController(ILogger logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected virtual async Task<IActionResult> ExecuteRequestAsync<TResult>(Func<Task<Result<TResult>>> request,
                                                                             [CallerMemberName] string methodName = default)
    {
        Result<TResult>? result = default;

        try
        {
            result = await request.Invoke();

            if (!result.HasError)
                return Ok(result);

            if (HasBusinessError(result))
                return BadRequest(result);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, methodName, new
            {
                Request,
                Method = methodName
            });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, result);
    }

    private static bool HasBusinessError<TResult>(Result<TResult> result)
        => result.Messages.Any(_ => _.MessageType.Equals(MessageType.BusinessError));
}