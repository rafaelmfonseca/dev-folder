namespace DevFolder.Verbs;

public class CategoryDefinition
{
    public string Folder { get; set; }

    public List<RepositoryDefinition> Repositories { get; set; }

    public bool HasRepositories() => Repositories is not null && Repositories.Any();
}
