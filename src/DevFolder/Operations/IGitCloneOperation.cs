namespace DevFolder.Operations;

public interface IGitCloneOperation
{
    Task Execute(string url, string workingDirectory, string repositoryFolderName = null);
}
