using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Validation;

namespace TaskManager.Api.Abstractions;

public abstract class MediatorApiController(IMediator mediator) : ControllerBase
{
    protected async Task<IActionResult> ExecuteAsync<TResponse>(IRequest<TResponse> request, Func<TResponse, IActionResult>? responseFunction = null)
    {
        var result = await SendAsync(request);

        return responseFunction?.Invoke(result) ?? Ok(result);
    }

    protected Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        Contract.Requires(request != null, $"{nameof(request)} != null");

        return mediator.Send(request!, HttpContext.RequestAborted);
    }
}
