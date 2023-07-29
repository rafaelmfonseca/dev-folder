using DevFolder.Commands;
using DevFolder.Operations;
using DevFolder.Platform;
using DevFolder.Verbs;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO.Abstractions;

namespace DevFolder.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<Runner>();
        serviceCollection.AddScoped<IFileSystem, FileSystem>();
        serviceCollection.AddScoped<OptionsFile>();
        serviceCollection.AddScoped<CloneCommand>();
        serviceCollection.AddScoped<GitCloneOperation>();
        serviceCollection.AddScoped<IProcessCommandHandler, LinuxCommandHandler>();

        serviceCollection.AddLogging(builder =>
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            builder.AddSerilog(logger, dispose: true);
        });
    }
}
