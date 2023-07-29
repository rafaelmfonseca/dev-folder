using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DevFolder.Platform;

public class WindowsProcessCommandHandler : IProcessCommandHandler
{
    private readonly ILogger<IProcessCommandHandler> _logger;

    public WindowsProcessCommandHandler(ILogger<IProcessCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task RunCommandAsync(string command, string workingDirectory)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{command}\"",
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();

            var output = process.StandardOutput.ReadToEnd().Trim();
            var error = process.StandardError.ReadToEnd().Trim();

            await process.WaitForExitAsync();

            _logger.LogInformation($"Output: {output}");

            if (process.ExitCode != 0)
            {
                _logger.LogError(error);
            }
            else
            {
                _logger.LogInformation($"Error: {error}");
            }
            _logger.LogInformation($"ExitCode: {process.ExitCode}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error running cmd.exe.");
        }
    }
}
