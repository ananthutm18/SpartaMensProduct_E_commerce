using System;
using System.IO;

public static class Logger
{
    private static string logFilePath;

    public static void Initialize(string path)
    {
        logFilePath = path;
    }

    public static void LogError(string message, Exception ex)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine("Date: " + DateTime.Now.ToString());
                writer.WriteLine("Message: " + message);
                writer.WriteLine("Exception: " + ex.ToString());
                writer.WriteLine(new string('-', 50));
            }
        }
        catch (Exception loggingEx)
        {
            Console.WriteLine(loggingEx);
        }
    }

    public static void LogInformation(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine("Date: " + DateTime.Now.ToString());
                writer.WriteLine("Message: " + message);
                writer.WriteLine(new string('-', 50));
            }
        }
        catch (Exception loggingEx)
        {
            Console.WriteLine(loggingEx);

        }
    }
}
