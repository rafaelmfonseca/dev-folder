﻿using DevFolder.Platform;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DevFolder.Operations;

public class GitCloneOperation : IGitCloneOperation
{
    private readonly IProcessCommandHandlerFactory _processCommandHandlerFactory;
    private readonly ILogger<IGitCloneOperation> _logger;

    public GitCloneOperation(
        IProcessCommandHandlerFactory processCommandHandlerFactory,
        ILogger<IGitCloneOperation> logger)
    {
        _processCommandHandlerFactory = processCommandHandlerFactory;
        _logger = logger;
    }

    public async Task Execute(string url, string workingDirectory, string repositoryFolderName = null)
    {
        _logger.LogInformation("Cloning \"{url}\" into \"{workingDirectory}\" with folder: \"{repositoryFolderName}\"...",
            url, workingDirectory, repositoryFolderName);

        var sbCommand = new StringBuilder();

        sbCommand.Append("git clone");

        if (!string.IsNullOrEmpty(url))
        {
            sbCommand.Append(' ');
            sbCommand.Append(url);
        }

        if (!string.IsNullOrEmpty(repositoryFolderName))
        {
            sbCommand.Append(' ');
            sbCommand.Append("\"" + repositoryFolderName + "\"");
        }

        var processCommandHandler = _processCommandHandlerFactory.Create();

        await processCommandHandler.RunCommandAsync(sbCommand.ToString(), workingDirectory);
    }
}
