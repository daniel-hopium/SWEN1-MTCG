namespace Transversal.Utils;

using System;

public static class Logger
{
    public static void LogInfo(string message)
    {
        Log("INFO", message);
    }

    public static void LogWarning(string message)
    {
        Log("WARNING", message);
    }

    public static void LogError(string message)
    {
        Log("ERROR", message);
    }

    private static void Log(string level, string message)
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}");
    }
}
