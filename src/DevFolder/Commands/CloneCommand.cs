using DevFolder.Operations;
using DevFolder.Options;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace DevFolder.Commands;

public class CloneCommand
{
    private readonly IGitCloneOperation _gitCloneOperation;
    private readonly OptionsFile _optionsFile;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public CloneCommand(
        IGitCloneOperation gitCloneOperation,
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

        foreach (var category in options.Categories)
        {
            if (category is null)
            {
                _logger.LogError($"Found a category \"null\"!");

                continue;
            }

            if (string.IsNullOrWhiteSpace(category.Folder))
            {
                _logger.LogError($"Found a category with property \"folder\" empty!");

                continue;
            }

            if (!category.HasRepositories())
            {
                _logger.LogError($"Category with folder \"{category.Folder}\" has no repositories!");

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
                var repositoryFolder = !string.IsNullOrEmpty(repository.Folder) ?
                    Path.Combine(categoryPath, repository.Folder) : categoryPath;

                await _gitCloneOperation.Execute(repository.Url, repositoryFolder);
            }
        }
    }
}
