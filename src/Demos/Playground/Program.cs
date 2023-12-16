using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using LiteLog.Logging;
using LiteLog.Logging.Loggers;
using Nez;

namespace Playground;

public static class Program
{
    public static string WorkingDirectory => AppDomain.CurrentDomain.BaseDirectory;

    [STAThread]
    static void Main()
    {
        LoggerFactory.DefaultLoggers = new List<ILoggerFrontend>
            {
                new ConsoleLogger(),
                new DiagnosticsLogger(),
                new FileLogger(),
                new MemoryLogger()
            };

        var logger = LoggerFactory.GetLogger("Kernel");

        var asm = Assembly.GetEntryAssembly();
        var version = asm?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "UNKNOWN";
        var asmVer = asm?.GetName()?.Version?.ToString() ?? "UNKNOWN";

        logger.Info("\n\n" +
                    $" =======================================================\n" +
                    $" = Starting Firebat\n" +
                    $" = Startup Time: {DateTimeOffset.Now:O}\n" +
                    $" = Working Directory: {WorkingDirectory}\n" +
                    $" = Assembly Version: {asmVer}\n" +
                    $" = Game Version: {version}\n" +
                    $" = Entry Assembly: \"{asm?.FullName}\"\n" +
                    $" = Storage Root: \"{Storage.GetStorageRoot()}\"\n" +
                    $" =======================================================\n");

        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            logger.Critical(ex?.ToString() ?? "UNKNOWN EXCEPTION");
        };

        Directory.SetCurrentDirectory(WorkingDirectory);

        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        using var game = new GameRoot();
        game.Run();

        logger.Info("Game Closed");
    }
}