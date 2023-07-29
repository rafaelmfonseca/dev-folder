namespace DevFolder.Options;

public class OptionsDefinition
{
    public List<CategoryDefinition> Categories { get; set; }

    public bool HasCategories() => Categories is not null && Categories.Count > 0;
}
