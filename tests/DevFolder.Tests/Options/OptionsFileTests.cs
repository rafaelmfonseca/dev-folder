using DevFolder.Exceptions;
using DevFolder.Tests.Builders;
using DevFolder.Tests.TestUtilities;
using DevFolder.Options;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;

namespace DevFolder.Tests.Options;

internal class OptionsFileTests
{
    [Test]
    public async Task ReadShouldParseAndReturnValidOptions()
    {
        var currentDirectory = @"c:\demo";

        var optionsDefinitionBuilder = new OptionsDefinitionBuilder();

        optionsDefinitionBuilder.WithCategory("demo", categoryDefinitionBuilder =>
        {
            categoryDefinitionBuilder.WithRepository("git@github.com:dotnet/runtime.git", "dotnet_runtime");
        });

        var optionsContent = optionsDefinitionBuilder.Build();

        var helper = new DevFolderTestHelper();

        helper.WithOptionsFile(optionsContent, currentDirectory);

        var optionsFile = helper.ServiceProvider.GetService<OptionsFile>();

        var options = await optionsFile.Read();

        options.Should().BeEquivalentTo(optionsContent);
    }

    [Test]
    public async Task ReadShouldThrowIfOptionsDoesNotExists()
    {
        var currentDirectory = @"c:\demo";

        var files = new Dictionary<string, MockFileData>();

        var helper = new DevFolderTestHelper();

        helper.WithMockFileSystem(files, currentDirectory);

        var optionsFile = helper.ServiceProvider.GetService<OptionsFile>();

        Func<Task> act = () => optionsFile.Read();

        await act.Should()
            .ThrowAsync<InvalidOptionsException>()
            .WithMessage(@"Options file not found at c:\demo\options.json!");
    }

    [Test]
    public async Task ReadShouldThrowIfOptionsHasNoCategories()
    {
        var currentDirectory = @"c:\demo";

        var optionsDefinitionBuilder = new OptionsDefinitionBuilder();

        var optionsContent = optionsDefinitionBuilder.Build();

        var helper = new DevFolderTestHelper();

        helper.WithOptionsFile(optionsContent, currentDirectory);

        var optionsFile = helper.ServiceProvider.GetService<OptionsFile>();

        Func<Task> act = () => optionsFile.Read();

        await act.Should()
            .ThrowAsync<InvalidOptionsException>()
            .WithMessage(@"Options file at c:\demo\options.json is null or has no categories!");
    }
}
