using NLog;

namespace InPostAPIEmulator;

class Program
{
    private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    
    static async Task Main(string[] args)
    {
        var config = new NLog.Config.LoggingConfiguration();
        
        var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "server.log" };
        var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
        
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
        config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
        
        LogManager.Configuration = config;

        string host;
        ushort port;

        host = args.Length > 0 ? args[0] : "localhost";
        port = args.Length > 1 ? ushort.Parse(args[1]) : (ushort)8000;
        
        Logger.Info("starting up...");
        await Server.RunServer(host, port);
    }
}