namespace CheckSoftwareIsInstalled;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var cli = new Cli.CommandLineInterface();
        cli.Run();
    }
}