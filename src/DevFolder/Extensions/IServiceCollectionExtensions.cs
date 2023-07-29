using CommandLine;
using DevFolder.Commands;
using DevFolder.Operations;
using DevFolder.Options;
using DevFolder.Platform;
using DevFolder.Verbs;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO.Abstractions;

namespace DevFolder.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection serviceCollection, string[] args)
    {
        serviceCollection.AddScoped<Runner>();
        serviceCollection.AddScoped<IFileSystem, FileSystem>();
        serviceCollection.AddScoped<IGitCloneOperation, GitCloneOperation>();
        serviceCollection.AddScoped<IProcessCommandHandlerFactory, ProcessCommandHandlerFactory>();
        serviceCollection.AddScoped<LinuxProcessCommandHandler>();
        serviceCollection.AddScoped<WindowsProcessCommandHandler>();
        serviceCollection.AddScoped<LogOnlyProcessCommandHandler>();
        serviceCollection.AddScoped<OptionsFile>();
        serviceCollection.AddScoped<CloneCommand>();

        serviceCollection.AddScoped<CommandLineParseResult>(serviceProvider =>
        {
            var result = Parser.Default.ParseArguments<CloneVerb>(args);

            if (result is Parsed<CloneVerb>)
            {
                return CommandLineParseResult.Create(true, result.Value);
            }

            return CommandLineParseResult.CreateNotParsed();
        });

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
