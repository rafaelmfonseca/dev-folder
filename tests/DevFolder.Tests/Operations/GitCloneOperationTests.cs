using DevFolder.Operations;
using DevFolder.Platform;
using DevFolder.Tests.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace DevFolder.Tests.Operations;

internal class GitCloneOperationTests
{
    [TestCase("git@github.com:dotnet/runtime.git", @"c:\demo", "git clone git@github.com:dotnet/runtime.git \"c:\\demo\"")]
    [TestCase("git@github.com:dotnet/runtime.git", @"", "git clone git@github.com:dotnet/runtime.git")]
    [TestCase("git@github.com:dotnet/runtime.git", null, "git clone git@github.com:dotnet/runtime.git")]
    public async Task ExecuteShouldGenerateCorrectGitCommand(string url, string folder, string command)
    {
        var helper = new DevFolderTestHelper();

        var mockedProcessCommandHandler = new Mock<IProcessCommandHandler>();
        var mockedProcessCommandHandlerFactory = new Mock<IProcessCommandHandlerFactory>();
        mockedProcessCommandHandlerFactory.Setup(e => e.Create()).Returns(mockedProcessCommandHandler.Object);

        helper.WithMockProcessCommandHandlerFactory(mockedProcessCommandHandlerFactory);

        var cloneCommand = helper.ServiceProvider.GetService<IGitCloneOperation>();

        await cloneCommand.Execute(url, folder);

        mockedProcessCommandHandler.Verify(mock =>
            mock.RunCommandAsync(command), Times.Once());

        mockedProcessCommandHandler.Verify(mock =>
            mock.RunCommandAsync(It.IsAny<string>()), Times.Exactly(1));
    }
}
