using System.IO.Abstractions;
using System.Text.Json;

namespace DevFolder.Options;

public class OptionsFile
{
    private readonly string _fileName = "options.json";
    private readonly IFileSystem _fileSystem;

    public OptionsFile(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async Task<OptionsDefinition> Read()
    {
        var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();

        var optionsFilePath = Path.Combine(currentDirectory, _fileName);

        if (_fileSystem.File.Exists(optionsFilePath))
        {
            throw new InvalidOperationException("Options file does not exist!");
        }

        var optionsContent = _fileSystem.File.Open(optionsFilePath, FileMode.Open);

        var options = await JsonSerializer.DeserializeAsync<OptionsDefinition>(optionsContent);

        if (options is null || !options.HasCategories())
        {
            throw new InvalidOperationException("Options file is empty or has no categories!");
        }

        return options;
    }
}
