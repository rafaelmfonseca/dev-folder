using DevFolder.Tests.Builders;
using DevFolder.Tests.TestUtilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DevFolder.Tests;

internal class RunnerTests
{
    [Test]
    public async Task RunShouldCloneAllWithCorrectArguments()
    {
        var args = new[] { "clone", "--logonly" };

        var currentDirectory = @"c:\demo";

        var optionsDefinitionBuilder = new OptionsDefinitionBuilder();

        optionsDefinitionBuilder.WithCategory("csharp", categoryDefinitionBuilder =>
        {
            categoryDefinitionBuilder.WithRepository("git@github.com:dotnet/runtime.git", "dotnet_runtime");
            categoryDefinitionBuilder.WithRepository("git@github.com:jbogard/MediatR.git");
        });

        optionsDefinitionBuilder.WithCategory("angular", categoryDefinitionBuilder =>
        {
            categoryDefinitionBuilder.WithRepository("git@github.com:bitwarden/clients.git", "bitwarden_clients");
            categoryDefinitionBuilder.WithRepository("git@github.com:storybookjs/storybook.git");
        });

        var optionsContent = optionsDefinitionBuilder.Build();

        var helper = new DevFolderTestHelper(args);
        helper.WithOptionsFile(optionsContent, currentDirectory);

        var runner = helper.ServiceProvider.GetService<Runner>();

        Func<Task> act = () => runner.RunAsync();

        await act.Should().NotThrowAsync();
    }
}
