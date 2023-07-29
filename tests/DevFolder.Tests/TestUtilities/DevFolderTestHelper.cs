using DevFolder.Extensions;
using DevFolder.Verbs;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace DevFolder.Tests.TestUtilities;

internal class DevFolderTestHelper
{
    private readonly ServiceCollection _serviceCollection;
    private ServiceProvider _serviceProvider;

    public ServiceProvider ServiceProvider
    {
        get
        {
            _serviceProvider ??= _serviceCollection.BuildServiceProvider();

            return _serviceProvider;
        }
    }

    public DevFolderTestHelper()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddServices();
    }

    public DevFolderTestHelper WithMockFileSystem(Dictionary<string, MockFileData> files, string currentDirectory)
    {
        _serviceCollection.Remove(ServiceDescriptor.Scoped<IFileSystem, FileSystem>());
        _serviceCollection.AddScoped<IFileSystem>((sp) => new MockFileSystem(files, currentDirectory));

        return this;
    }

    public DevFolderTestHelper WithOptionsFile(OptionsDefinition optionsContent, string currentDirectory)
    {
        var files = new Dictionary<string, MockFileData>
        {
            { $@"{currentDirectory}\options.json", new MockFileData(JsonSerializer.Serialize(optionsContent)) }
        };

        WithMockFileSystem(files, currentDirectory);

        return this;
    }
}