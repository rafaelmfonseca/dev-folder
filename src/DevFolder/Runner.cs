using DevFolder.Commands;
using DevFolder.Exceptions;
using DevFolder.Verbs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace DevFolder;

public class Runner
{
    private readonly CommandLineParseResult _commandLineParseResult;
    private readonly IServiceProvider _serviceProvider;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public Runner(
        ILogger<Runner> logger,
        IServiceProvider serviceProvider,
        CommandLineParseResult commandLineParseResult,
        IFileSystem fileSystem)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _commandLineParseResult = commandLineParseResult;
        _fileSystem = fileSystem;
    }

    public async Task RunAsync()
    {
        if (!_commandLineParseResult.IsParsed)
        {
            throw new InvalidEnvArgumentsException("Invalid environment arguments.");
        }

        _logger.LogInformation("Current folder: \"{currentDirectory}\".",
            _fileSystem.Directory.GetCurrentDirectory());

        if (_commandLineParseResult.CloneVerbInstance is not null)
        {
            var cloneCommand = _serviceProvider.GetService<CloneCommand>();

            await cloneCommand.Execute();
        }
    }
}
