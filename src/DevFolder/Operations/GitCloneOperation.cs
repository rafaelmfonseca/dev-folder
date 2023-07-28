using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DevFolder.Operations;

public class GitCloneOperation
{
    private readonly OSPlatform _platform;

    public GitCloneOperation()
    {
        _platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows : OSPlatform.Linux;
    }

    public async Task Execute(string url, string folder)
    {
        if (_platform == OSPlatform.Linux)
        {
            await ExecuteOnLinux(url, folder);
        }
        else
        {
            throw new PlatformNotSupportedException("Windows is not supported yet!");
        }
    }

    private async Task ExecuteOnLinux(string url, string folder)
    {
        Console.WriteLine(@$"Cloning ""{url}"" into ""{folder}"" for Linux...");

        var command = @$"git clone {url} ""{folder}""";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        process.Start();

        await process.WaitForExitAsync();
    }
}
