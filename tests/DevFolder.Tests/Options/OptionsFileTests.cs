using DevFolder.Exceptions;
using DevFolder.Extensions;
using DevFolder.Verbs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace DevFolder.Tests.Options
{
    internal class OptionsFileTests
    {
        [Test]
        public async Task ReadShouldParseAndReturnValidOptions()
        {
            var currentDirectory = @"c:\demo";

            var optionsContent = new OptionsDefinition()
            {
                Categories = new List<CategoryDefinition>()
                {
                    new CategoryDefinition()
                    {
                        Folder = "demo",
                        Repositories = new List<RepositoryDefinition>()
                        {
                            new RepositoryDefinition()
                            {
                                Url = "git@github.com:dotnet/runtime.git",
                                Folder = "dotnet_runtime"
                            }
                        }
                    }
                }
            };

            var files = new Dictionary<string, MockFileData>
            {
                { @"c:\demo\options.json", new MockFileData(JsonSerializer.Serialize(optionsContent)) }
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddServices();

            serviceCollection.Remove(ServiceDescriptor.Scoped<IFileSystem, FileSystem>());
            serviceCollection.AddScoped<IFileSystem>((sp) => new MockFileSystem(files, currentDirectory));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var optionsFile = serviceProvider.GetService<OptionsFile>();

            var options = await optionsFile.Read();

            options.Should().BeEquivalentTo(optionsContent);
        }

        [Test]
        public async Task ReadShouldThrowIfOptionsDoesNotExists()
        {
            var currentDirectory = @"c:\demo";

            var files = new Dictionary<string, MockFileData>();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddServices();

            serviceCollection.Remove(ServiceDescriptor.Scoped<IFileSystem, FileSystem>());
            serviceCollection.AddScoped<IFileSystem>((sp) => new MockFileSystem(files, currentDirectory));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var optionsFile = serviceProvider.GetService<OptionsFile>();

            Func<Task> act = () => optionsFile.Read();

            await act.Should()
                .ThrowAsync<InvalidOptionsException>()
                .WithMessage(@"Options file not found at c:\demo\options.json!");
        }

        [Test]
        public async Task ReadShouldThrowIfOptionsHasNoCategories()
        {
            var currentDirectory = @"c:\demo";

            var optionsContent = new OptionsDefinition()
            {
                Categories = new List<CategoryDefinition>()
            };

            var files = new Dictionary<string, MockFileData>
            {
                { @"c:\demo\options.json", new MockFileData(JsonSerializer.Serialize(optionsContent)) }
            };

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddServices();

            serviceCollection.Remove(ServiceDescriptor.Scoped<IFileSystem, FileSystem>());
            serviceCollection.AddScoped<IFileSystem>((sp) => new MockFileSystem(files, currentDirectory));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var optionsFile = serviceProvider.GetService<OptionsFile>();

            Func<Task> act = () => optionsFile.Read();

            await act.Should()
                .ThrowAsync<InvalidOptionsException>()
                .WithMessage(@"Options file at c:\demo\options.json is null or has no categories!");
        }
    }
}
