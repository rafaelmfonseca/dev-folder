﻿using DevFolder.Exceptions;
using System.IO.Abstractions;
using System.Text.Json;

namespace DevFolder.Options;

public class OptionsFile
{
    private readonly static JsonSerializerOptions _deserializerOptions =
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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

        if (!_fileSystem.File.Exists(optionsFilePath))
        {
            throw new InvalidOptionsException($"Options file not found at \"{optionsFilePath}\"!");
        }

        var optionsContent = _fileSystem.File.Open(optionsFilePath, FileMode.Open);

        var options = await JsonSerializer.DeserializeAsync<OptionsDefinition>(optionsContent, _deserializerOptions);

        if (options is null)
        {
            throw new InvalidOptionsException($"Options file at \"{optionsFilePath}\" is null!");
        }

        return options;
    }
}
