namespace FIXsi.Shared;

public static class LogEntry
{
    /// <summary>
    /// Grava uma mensagem de Info no arquivo de log.
    /// </summary>
    /// <param name="message">Mensagem a ser gravada.</param>
    public static void Info(string message)
    {
        try
        {
            CheckTaskIdAndTaskName();
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t[Info]\t{message.Trim()}";
            using StreamWriter writer = File.AppendText($"{Settings.AppLogDirectory}\\{Settings.TaskId} - {Settings.TaskName}.txt");
            writer.WriteLine(logMessage);
        }
        catch { }
    }

    /// <summary>
    /// Grava uma mensagem de Warning no arquivo de log.
    /// </summary>
    /// <param name="message">Mensagem a ser gravada.</param>
    public static void Warning(string message)
    {
        try
        {
            CheckTaskIdAndTaskName();
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t[Warning]\t{message.Trim()}";
            using StreamWriter writer = File.AppendText($"{Settings.AppLogDirectory}\\{Settings.TaskId} - {Settings.TaskName}.txt");
            writer.WriteLine(logMessage);
        }
        catch { }
    }

    /// <summary>
    /// Grava uma mensagem de Error no arquivo de log.
    /// </summary>
    /// <param name="message">Mensagem a ser gravada.</param>
    public static void Error(string message)
    {
        try
        {
            CheckTaskIdAndTaskName();
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t[Error]\t{message.Trim()}";
            using StreamWriter writer = File.AppendText($"{Settings.AppLogDirectory}\\{Settings.TaskId} - {Settings.TaskName}.txt");
            writer.WriteLine(logMessage);
        }
        catch { }
    }

    private static void CheckTaskIdAndTaskName()
    {
        if (string.IsNullOrEmpty(Settings.TaskId)
                || string.IsNullOrWhiteSpace(Settings.TaskId)
                || string.IsNullOrEmpty(Settings.TaskName)
                || string.IsNullOrWhiteSpace(Settings.TaskName))
        {
            Settings.TaskId = "0000";
            Settings.TaskName = "FIXsi";
        }
    }
}
