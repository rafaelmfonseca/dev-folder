using DevFolder.Operations;
using DevFolder.Verbs;
using System.IO.Abstractions;

namespace DevFolder.Commands;

public class CloneCommand
{
    private readonly OptionsFile _optionsFile;
    private readonly IFileSystem _fileSystem;

    public CloneCommand(OptionsFile optionsFile, IFileSystem fileSystem)
    {
        _optionsFile = optionsFile;
        _fileSystem = fileSystem;
    }

    public async Task Execute()
    {
        var options = await _optionsFile.Read();

        var gitCloneOperation = new GitCloneOperation();

        foreach (var category in options.Categories)
        {
            if (category is null || !category.HasRepositories())
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(category.Folder))
            {
                Console.WriteLine("Found an empty category folder!");

                continue;
            }

            Console.WriteLine($"Current category folder: {category.Folder}");

            var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();

            var categoryPath = Path.Combine(currentDirectory, category.Folder);

            if (!_fileSystem.Directory.Exists(categoryPath))
            {
                _fileSystem.Directory.CreateDirectory(categoryPath);
            }

            foreach (var repository in category.Repositories)
            {
                var repositoryFolder = !string.IsNullOrEmpty(repository.Folder) ? Path.Combine(categoryPath, repository.Folder) : categoryPath;

                await gitCloneOperation.Execute(repository.Url, repositoryFolder);
            }
        }
    }
}
