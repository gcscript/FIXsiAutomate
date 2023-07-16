namespace FIXsi.Shared;

public static class Settings
{
    public static string AppDirectory { get; } = "C:\\Apps\\FIXsi Automate";
    public static string AppRequestFile { get; } = $"{AppDirectory}\\Request.json";
    public static string AppResponseFile { get; } = $"{AppDirectory}\\Response.json";
    public static string AppLogDirectory { get; } = Path.Combine(AppDirectory, "Logs");
    public static string Hostname { get; } = System.Net.Dns.GetHostName().Replace("FIX-BOT-", "WKS");
    public static string TaskId { get; set; } = "";
    public static string TaskName { get; set; } = "";
    public static string Function { get; set; } = "";
    public static bool CloseUnwantedWindowsRepeat { get; set; }
    public static int TimeoutToFindWindow { get; } = 60;
    public static string RequestFileJsonString { get; set; } = "";
}
