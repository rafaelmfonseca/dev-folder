using DevFolder.Commands;
using DevFolder.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevFolder;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddServices(args);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var runner = serviceProvider.GetService<Runner>();

        try
        {
            await runner.RunAsync();
        }
        catch (Exception ex)
        {
            var _logger = serviceProvider.GetService<ILogger<Program>>();

            _logger.LogError(ex, "Error while executing clone command.");
        }
    }
} 
