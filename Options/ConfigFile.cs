using DevFolder.Operations;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace DevFolder.Options;

public class ConfigFile
{
    private readonly string _fileName = "options.json";
    private readonly IFileSystem _fileSystem;

    public ConfigFile(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async Task Read()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var optionsPath = Path.Combine(currentDirectory, _fileName);

        if (_fileSystem.File.Exists(optionsPath))
        {
            throw new InvalidOperationException("Options file does not exist!");
        }

        var optionsContent = _fileSystem.File.Open(optionsPath, FileMode.Open);

        var options = await JsonSerializer.DeserializeAsync<OptionsDefinition>(optionsContent);

        if (options is null || !options.HasCategories())
        {
            throw new InvalidOperationException("Options file is empty!");
        }

        var gitCloneOperation = new GitCloneOperation();

        foreach (var category in options.Categories)
        {
            if (category is null || !category.HasRepositories())
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(category.Folder))
            {
                Console.WriteLine("Category folder is empty!");

                continue;
            }

            Console.WriteLine($"Current category folder: {category.Folder}");

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
