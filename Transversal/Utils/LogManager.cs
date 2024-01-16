namespace Transversal.Utils;

public static class LogManager
{
    private static readonly object _lockObject = new object();

    public static void SetupLoggingFile()
    {
        try
        {
            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            var logFile = Path.Combine(logPath, "log.txt");
            if (!File.Exists(logFile))
            {
                File.Create(logFile).Dispose();
            }
            // var textWriter = new StreamWriter(logFile, true);
            // Console.SetOut(textWriter);
        }
        catch (Exception e)
        {
            Log.Error("Setting up a Logfile for Logs failed!", e);
        }
    }
    
    public static void WriteToFile(string log)
    {
        try
        {
            lock (_lockObject)
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                var logFile = Path.Combine(logPath, "log.txt");
                File.AppendAllText(logFile, log + "\n");
            }
        }
        catch (Exception e)
        {
            Log.Error("Writing Logs into Logfile failed!", e);
        }
    }
}