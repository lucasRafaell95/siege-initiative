using Microsoft.AspNetCore.Mvc;
using SiegeInitiative.DataContracts.OperationResult;
using SiegeInitiative.DataContracts.OperationResult.Base;
using System.Runtime.CompilerServices;

namespace SiegeInitiative.Api.Controllers.Base;

/// <summary>
/// Base class to Api Controller
/// </summary>
public abstract class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> logger;

    protected ApiController(ILogger<ApiController> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected virtual async Task<IActionResult> ExecuteRequestAsync<TResult>(Func<Task<Result<TResult>>> request,
                                                                             [CallerMemberName] string methodName = default)
    {
        Result<TResult>? result;

        try
        {
            result = await request.Invoke();

            return result.Sucess
                ? Ok(result)
                : BadRequest(result);
        }
        catch (Exception ex)
        {
            result = new UnexpectedResult<TResult>();

            this.logger.LogError(methodName, new
            {
                Exception = ex,
                Request,
                Method = methodName
            });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, result);
    }
}