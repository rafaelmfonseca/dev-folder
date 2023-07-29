using DevFolder.Verbs;
using Microsoft.Extensions.DependencyInjection;

namespace DevFolder.Platform;

public class ProcessCommandHandlerFactory : IProcessCommandHandlerFactory
{
    private readonly CommandLineParseResult _commandLineParseResult;
    private readonly IServiceProvider _serviceProvider;

    public ProcessCommandHandlerFactory(
        CommandLineParseResult commandLineParseResult,
        IServiceProvider serviceProvider)
    {
        _commandLineParseResult = commandLineParseResult;
        _serviceProvider = serviceProvider;
    }

    public IProcessCommandHandler Create()
    {
        if (_commandLineParseResult.CloneVerbInstance is not null &&
            _commandLineParseResult.CloneVerbInstance.LogOnly)
        {
            return _serviceProvider.GetService<LogOnlyProcessCommandHandler>();
        }

        var platform = Environment.OSVersion.Platform;

        if (platform == PlatformID.Win32NT)
        {
            return _serviceProvider.GetService<WindowsProcessCommandHandler>();
        }
        else if (platform == PlatformID.Unix)
        {
            return _serviceProvider.GetService<LinuxProcessCommandHandler>();
        }
        else
        {
            throw new NotSupportedException($"Platform {platform} is not supported.");
        }
    }
}
