using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

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
        try
        {
            using var process = new Process();

            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = $"-c \"{command}\"";
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                _logger.LogError(e.Data);
            };

            process.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                _logger.LogInformation(e.Data);
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            _logger.LogInformation("ExitCode: {exitCode}", process.ExitCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error running /bin/bash.");
        }
    }
}
