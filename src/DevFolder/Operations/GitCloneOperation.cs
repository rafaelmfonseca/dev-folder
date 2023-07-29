using DevFolder.Platform;
using Microsoft.Extensions.Logging;

namespace DevFolder.Operations;

public class GitCloneOperation
{
    private readonly IProcessCommandHandler _processCommandHandler;
    private readonly ILogger<GitCloneOperation> _logger;

    public GitCloneOperation(
        IProcessCommandHandler processCommandHandler,
        ILogger<GitCloneOperation> logger)
    {
        _processCommandHandler = processCommandHandler;
        _logger = logger;
    }

    public async Task Execute(string url, string? folder)
    {
        _logger.LogInformation(@$"Cloning ""{url}"" into ""{folder}""...");

        var command = @$"git clone {url} {"\"" + folder + "\"" ?? string.Empty}";

        await _processCommandHandler.RunCommandAsync(command);
    }
}
