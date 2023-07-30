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

        var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();

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

            if (category.Ignore)
            {
                _logger.LogInformation("Category \"{category}\" ignored!", category.Folder);

                continue;
            }

            if (!category.HasRepositories())
            {
                _logger.LogError("Category with folder \"{folder}\" has no repositories!", category.Folder);

                continue;
            }

            _logger.LogInformation("Current category folder: {folder}", category.Folder);

            var categoryDirectory = Path.Combine(currentDirectory, category.Folder);

            if (!_fileSystem.Directory.Exists(categoryDirectory))
            {
                _fileSystem.Directory.CreateDirectory(categoryDirectory);
            }

            foreach (var repository in category.Repositories)
            {
                await _gitCloneOperation.Execute(repository.Url, categoryDirectory, repository.Folder);
            }
        }
    }
}
