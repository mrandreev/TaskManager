using Microsoft.Extensions.Logging;
using TaskManager.Application.Messaging.Interfaces;

namespace TaskManager.Application.Messaging.Messages;

public record TaskItemCompletedMessage(int Id, string Name, DateTime CompletedAt);

public class TaskItemCompletedMessageHandler(ILogger<TaskItemCompletedMessageHandler> logger) : IMessageHandler<TaskItemCompletedMessage>
{
    public Task HandleAsync(TaskItemCompletedMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Task completed at {message.CompletedAt} — Id: '{message.Id}', Name: '{message.Name}'");

        return Task.CompletedTask;
    }
}