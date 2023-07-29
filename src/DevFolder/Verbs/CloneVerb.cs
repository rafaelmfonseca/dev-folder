using CommandLine;

namespace DevFolder.Verbs;

[Verb("clone", HelpText = "Clone multiple repositories based on the `options.json` in the directory.")]
public sealed class CloneVerb
{
    [Option('l', "logonly",
        Required = false,
        HelpText = "Log the repositories that would be cloned but do not clone them.")]
    public bool LogOnly { get; set; }
}