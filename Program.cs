using CommandLine;
using DevFolder.Options;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CloneVerb>(args).MapResult(
            (CloneVerb options) => RunCloneAndReturnExitCode(options),
            (IEnumerable<Error> errors) => 1);
    }

    public static int RunCloneAndReturnExitCode(CloneVerb options)
    {

        return 0;
    }
} 
