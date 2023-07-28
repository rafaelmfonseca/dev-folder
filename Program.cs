using CommandLine;
using DevFolder.Commands;
using DevFolder.Operations;
using DevFolder.Options;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<CloneVerb>(args);

        if (result is null)
        {
            Console.WriteLine("Invalid arguments");

            return;
        }

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<IFileSystem, FileSystem>();
        serviceCollection.AddScoped<OptionsFile>();
        serviceCollection.AddScoped<CloneCommand>();
        serviceCollection.AddScoped<GitCloneOperation>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        if (result is Parsed<CloneVerb>)
        {
            var cloneCommand = serviceProvider.GetService<CloneCommand>();

            await cloneCommand.Execute();
        }
    }
} 
