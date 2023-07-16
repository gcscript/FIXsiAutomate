using System.Text.Json;

namespace FIXsi.Shared;

internal class ResponseResult { public bool Success { get; set; } public string? Message { get; set; } public string? ErrorCode { get; set; } }

public static class SendResponse
{
    /// <summary>
    /// Quando o processo é executado com sucesso.
    /// </summary>
    public static void Success(string message)
    {
        // Quando o processo é executado com sucesso.
        try
        {
            LogEntry.Info(message);
            var result = new ResponseResult { Success = true, Message = "", ErrorCode = "" };
            var json = JsonSerializer.Serialize(result);
            File.WriteAllText(Settings.AppResponseFile, json);
        }
        catch { }
        finally { Environment.Exit(0); }
    }

    /// <summary>
    /// Quando o processo é executado com sucesso e precisa retornar uma mensagem.
    /// </summary>
    /// <param name="message">Mensagem de erro.</param>
    /// <param name="error">Código do erro.</param>	
    public static void Error(string message, string error)
    {
        // Quando acontece um erro e o processo precisa ser reiniciado.
        try
        {
            LogEntry.Error(message);
            var result = new ResponseResult { Success = false, Message = message, ErrorCode = error };
            var json = JsonSerializer.Serialize(result);
            File.WriteAllText(Settings.AppResponseFile, json);
        }
        catch { }
        finally { Environment.Exit(1); }
    }

    /// <summary>
    /// Quando o processo é executado com sucesso e precisa gravar a resposta na planilha do Excel. Exemplo: Processo não existe!
    /// </summary>
    /// <param name="message">Mensagem de erro.</param>
    public static void WriteExcel(string message)
    {
        // Quando o processo é executado e precisa gravar a resposta em uma planilha do Excel.
        // Geralmente é usado quando o processo não existe!
        // Não é um erro, é um aviso!
        try
        {
            LogEntry.Warning(message);
            var result = new ResponseResult { Success = true, Message = message, ErrorCode = "" };
            var json = JsonSerializer.Serialize(result);
            File.WriteAllText(Settings.AppResponseFile, json);
        }
        catch { }
        finally { Environment.Exit(0); }
    }
}

