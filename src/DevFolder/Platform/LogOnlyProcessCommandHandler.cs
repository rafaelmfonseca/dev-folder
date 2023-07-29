using Microsoft.Extensions.Logging;

namespace DevFolder.Platform;

public class LogOnlyProcessCommandHandler : IProcessCommandHandler
{
    private readonly ILogger<IProcessCommandHandler> _logger;

    public LogOnlyProcessCommandHandler(ILogger<IProcessCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task RunCommandAsync(string command, string workingDirectory)
    {
        _logger.LogInformation($"Command executed: \"{command}\", workingDirectory: \"{workingDirectory}\"");

        return Task.CompletedTask;
    }
}
