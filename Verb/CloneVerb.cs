using CommandLine;

namespace DevFolder.Options;

[Verb("clone", HelpText = "Clone multiple repositories based on the `options.json` in the directory.")]
public sealed class CloneVerb { }