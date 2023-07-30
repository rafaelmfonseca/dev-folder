using DevFolder.Options;

namespace DevFolder.Tests.Builders;

public class CategoryDefinitionBuilder
{
    private readonly CategoryDefinition _categoryDefinition;

    public CategoryDefinitionBuilder(string folder, bool ignore)
    {
        _categoryDefinition = new CategoryDefinition
        {
            Folder = folder,
            Ignore = ignore,
            Repositories = new List<RepositoryDefinition>()
        };
    }

    public CategoryDefinitionBuilder WithRepository(string url, string folder = null)
    {
        _categoryDefinition.Repositories.Add(new RepositoryDefinition
        {
            Url = url,
            Folder = folder
        });

        return this;
    }

    public CategoryDefinition Build()
    {
        return _categoryDefinition;
    }
}