using DevFolder.Options;

namespace DevFolder.Tests.Builders;

public class OptionsDefinitionBuilder
{
    private readonly OptionsDefinition _optionsDefinition;

    public OptionsDefinitionBuilder()
    {
        _optionsDefinition = new OptionsDefinition()
        {
            Categories = new List<CategoryDefinition>()
        };
    }

    public CategoryDefinitionBuilder WithCategory(string folder, Action<CategoryDefinitionBuilder> configure)
    {
        var categoryBuilder = new CategoryDefinitionBuilder(folder);

        configure(categoryBuilder);

        _optionsDefinition.Categories.Add(categoryBuilder.Build());

        return categoryBuilder;
    }

    public OptionsDefinition Build()
    {
        return _optionsDefinition;
    }
}
