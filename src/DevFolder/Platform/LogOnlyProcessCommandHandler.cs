using Microsoft.Extensions.Logging;

namespace DevFolder.Platform;

public class LogOnlyProcessCommandHandler : IProcessCommandHandler
{
    private readonly ILogger<IProcessCommandHandler> _logger;

    public LogOnlyProcessCommandHandler(ILogger<IProcessCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task RunCommandAsync(string command)
    {
        _logger.LogInformation($"Command executed: {command}");

        return Task.CompletedTask;
    }
}
