using DevFolder.Commands;
using DevFolder.Operations;
using DevFolder.Tests.Builders;
using DevFolder.Tests.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace DevFolder.Tests.Commands;

internal class CloneCommandTests
{
    [Test]
    public async Task ExecuteShouldCallCloneOperationsWithCorrectArguments()
    {
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

        var helper = new DevFolderTestHelper();

        var mockedGitCloneOperation = new Mock<IGitCloneOperation>();

        helper.WithOptionsFile(optionsContent, currentDirectory);
        helper.WithMockGitCloneOperation(mockedGitCloneOperation);

        var cloneCommand = helper.ServiceProvider.GetService<CloneCommand>();

        await cloneCommand.Execute();

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute("git@github.com:dotnet/runtime.git", @"c:\demo\csharp", "dotnet_runtime"), Times.Once());

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute("git@github.com:jbogard/MediatR.git", @"c:\demo\csharp", null), Times.Once());

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute("git@github.com:bitwarden/clients.git", @"c:\demo\angular", "bitwarden_clients"), Times.Once());

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute("git@github.com:storybookjs/storybook.git", @"c:\demo\angular", null), Times.Once());

        mockedGitCloneOperation.Verify(mock => mock.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
    }

    [TestCase("")]
    [TestCase(null)]
    public async Task ExecuteShouldIgnoreCategoriesWithInvalidFolder(string invalidFolderCategory)
    {
        var currentDirectory = @"c:\demo";

        var optionsDefinitionBuilder = new OptionsDefinitionBuilder();

        optionsDefinitionBuilder.WithCategory("csharp", categoryDefinitionBuilder =>
        {
            categoryDefinitionBuilder.WithRepository("git@github.com:dotnet/runtime.git", "dotnet_runtime");
            categoryDefinitionBuilder.WithRepository("git@github.com:jbogard/MediatR.git");
        });

        optionsDefinitionBuilder.WithCategory(invalidFolderCategory, categoryDefinitionBuilder =>
        {
            categoryDefinitionBuilder.WithRepository("git@github.com:bitwarden/clients.git", "bitwarden_clients");
            categoryDefinitionBuilder.WithRepository("git@github.com:storybookjs/storybook.git");
        });

        var optionsContent = optionsDefinitionBuilder.Build();

        var helper = new DevFolderTestHelper();

        var mockedGitCloneOperation = new Mock<IGitCloneOperation>();

        helper.WithOptionsFile(optionsContent, currentDirectory);
        helper.WithMockGitCloneOperation(mockedGitCloneOperation);

        var cloneCommand = helper.ServiceProvider.GetService<CloneCommand>();

        await cloneCommand.Execute();

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute("git@github.com:dotnet/runtime.git", @"c:\demo\csharp", "dotnet_runtime"), Times.Once());

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute("git@github.com:jbogard/MediatR.git", @"c:\demo\csharp", null), Times.Once());

        mockedGitCloneOperation.Verify(mock =>
            mock.Execute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
    }
}