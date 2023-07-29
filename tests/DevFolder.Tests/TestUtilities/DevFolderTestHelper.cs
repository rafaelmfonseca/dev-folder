using DevFolder.Extensions;
using DevFolder.Operations;
using DevFolder.Options;
using DevFolder.Platform;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

    public DevFolderTestHelper(string[] args = null)
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddServices(args);
    }

    public DevFolderTestHelper WithMockFileSystem(Dictionary<string, MockFileData> files, string currentDirectory)
    {
        RemoveScoped<IFileSystem, FileSystem>();

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

    public DevFolderTestHelper WithMockGitCloneOperation(Mock<IGitCloneOperation> mockInstance)
    {
        RemoveScoped<IGitCloneOperation, GitCloneOperation>();

        _serviceCollection.AddScoped((sp) => mockInstance.Object);

        return this;
    }

    public DevFolderTestHelper WithMockProcessCommandHandlerFactory(Mock<IProcessCommandHandlerFactory> mockInstance)
    {
        RemoveScoped<IProcessCommandHandlerFactory, ProcessCommandHandlerFactory>();

        _serviceCollection.AddScoped((sp) => mockInstance.Object);

        return this;
    }

    public DevFolderTestHelper RemoveScoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _serviceCollection.Remove(ServiceDescriptor.Scoped<TService, TImplementation>());

        return this;
    }
}