using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Abstractions;
using TaskManager.Application.TaskItems.Commands;
using TaskManager.Application.TaskItems.Queries;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskItemsController(IMediator mediator) : MediatorApiController(mediator)
{
    [HttpGet]
    public Task<IActionResult> GetAllAsync() => ExecuteAsync(new TaskItemsQuery());

    [HttpPost]
    public Task<IActionResult> CreateAsync([FromBody] TaskItemCreateCommand command) => ExecuteAsync(command);

    [HttpPut("{taskId:int}")]
    public Task<IActionResult> UpdateAsync(int taskId, [FromBody] TaskItemUpdateCommand command) => ExecuteAsync(new TaskItemUpdateCommand(taskId, command.Name, command.Description));

    [HttpPut("{taskId:int}/start")]
    public Task<IActionResult> StartAsync(int taskId) => ExecuteAsync(new TaskItemStartCommand(taskId));

    [HttpPut("{taskId:int}/complete")]
    public Task<IActionResult> CompleteAsync(int taskId) => ExecuteAsync(new TaskItemCompleteCommand(taskId));
}
