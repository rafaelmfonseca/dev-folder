using DevFolder.Operations;
using DevFolder.Platform;
using DevFolder.Tests.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace DevFolder.Tests.Operations;

internal class GitCloneOperationTests
{
    [Test()]
    public async Task ExecuteShouldGenerateCorrectGitCommand()
    {
        const string url = "git@github.com:dotnet/runtime.git";
        const string workingDirectory = @"c:\demo";
        const string repositoryFolderName = "dotnet-runtime";

        var helper = new DevFolderTestHelper();

        var mockedProcessCommandHandler = new Mock<IProcessCommandHandler>();
        var mockedProcessCommandHandlerFactory = new Mock<IProcessCommandHandlerFactory>();
        mockedProcessCommandHandlerFactory.Setup(e => e.Create()).Returns(mockedProcessCommandHandler.Object);

        helper.WithMockProcessCommandHandlerFactory(mockedProcessCommandHandlerFactory);

        var cloneCommand = helper.ServiceProvider.GetService<IGitCloneOperation>();

        await cloneCommand.Execute(url, workingDirectory, repositoryFolderName);

        mockedProcessCommandHandler.Verify(mock =>
            mock.RunCommandAsync("git clone git@github.com:dotnet/runtime.git \"dotnet-runtime\"", @"c:\demo"), Times.Once());

        mockedProcessCommandHandler.Verify(mock =>
            mock.RunCommandAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
    }
}
