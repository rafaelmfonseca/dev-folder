using DevFolder.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DevFolder;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddServices();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var runner = serviceProvider.GetService<Runner>();

        await runner.RunAsync(args);
    }
} 
