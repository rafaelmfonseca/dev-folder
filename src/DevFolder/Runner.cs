using CommandLine;
using DevFolder.Commands;
using DevFolder.Verbs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevFolder;

public class Runner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public Runner(
        ILogger<Runner> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task RunAsync(string[] args)
    {
        var result = Parser.Default.ParseArguments<CloneVerb>(args);

        if (result is null)
        {
            _logger.LogInformation("Invalid arguments.");

            return;
        }

        if (result is Parsed<CloneVerb>)
        {
            var cloneCommand = _serviceProvider.GetService<CloneCommand>();

            try
            {
                await cloneCommand.Execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while executing clone command.");
            }
        }
    }
}
