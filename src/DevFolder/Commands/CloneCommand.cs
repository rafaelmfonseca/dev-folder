using DevFolder.Operations;
using DevFolder.Verbs;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace DevFolder.Commands;

public class CloneCommand
{
    private readonly GitCloneOperation _gitCloneOperation;
    private readonly OptionsFile _optionsFile;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CloneCommand(
        GitCloneOperation gitCloneOperation,
        OptionsFile optionsFile,
        IFileSystem fileSystem,
        ILogger<CloneCommand> logger)
    {
        _gitCloneOperation = gitCloneOperation;
        _optionsFile = optionsFile;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public async Task Execute()
    {
        var options = await _optionsFile.Read();

        if (options is null)
        {
            return;
        }

        foreach (var category in options.Categories)
        {
            if (category is null || !category.HasRepositories())
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(category.Folder))
            {
                _logger.LogError($"Category folder is empty!");

                continue;
            }

            _logger.LogInformation($"Current category folder: {category.Folder}");

            var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();

            var categoryPath = Path.Combine(currentDirectory, category.Folder);

            if (!_fileSystem.Directory.Exists(categoryPath))
            {
                _fileSystem.Directory.CreateDirectory(categoryPath);
            }

            foreach (var repository in category.Repositories)
            {
                var repositoryFolder = !string.IsNullOrEmpty(repository.Folder) ? Path.Combine(categoryPath, repository.Folder) : null;

                await _gitCloneOperation.Execute(repository.Url, repositoryFolder);
            }
        }
    }
}
