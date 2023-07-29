using Microsoft.Extensions.Logging;

namespace DevFolder.Platform;

public class LinuxProcessCommandHandler : IProcessCommandHandler
{
    private readonly ILogger<IProcessCommandHandler> _logger;

    public LinuxProcessCommandHandler(ILogger<IProcessCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task RunCommandAsync(string command, string workingDirectory)
    {
        throw new NotImplementedException();
    }
}
