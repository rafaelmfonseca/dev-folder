namespace DevFolder.Platform;

public interface IProcessCommandHandler
{
    Task RunCommandAsync(string command, string workingDirectory);
}
