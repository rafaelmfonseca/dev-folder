namespace DevFolder.Verbs;

public class CommandLineParseResult
{
    public bool IsParsed { get; }

    public CloneVerb CloneVerbInstance { get; }

    private CommandLineParseResult(bool isParsed, CloneVerb cloneVerbInstance)
    {
        IsParsed = isParsed;
        CloneVerbInstance = cloneVerbInstance;
    }

    public static CommandLineParseResult Create(bool isParsed, CloneVerb cloneVerbInstance)
    {
        return new(isParsed, cloneVerbInstance);
    }

    public static CommandLineParseResult CreateNotParsed()
    {
        return new(false, null);
    }
}
