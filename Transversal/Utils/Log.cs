namespace Transversal.Utils;

using System;

public static class Log
{
    public static void Info(string message)
    {
        LogWithDate("INFO", message);
    }

    public static void Warning(string message)
    {
        LogWithDate("WARNING", message);
    }

    public static void Error(string message)
    {
        LogWithDate("ERROR", message);
    }
    
    public static void Error(string message, Exception exception)
    {
        LogWithDate("ERROR", message + "\n" + exception);
    }

    private static void LogWithDate(string level, string message)
    {
        string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
        Console.WriteLine(log); //also momentarily write to console
        LogManager.WriteToFile(log);
    }
    
    public static void InfoWithThread(string message)
    {
        Info($"Thread-{Thread.CurrentThread.ManagedThreadId} " + message);
    }
}
