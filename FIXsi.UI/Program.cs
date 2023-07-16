using FIXsi.Shared;

namespace FIXsi.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (!Directory.Exists(Settings.AppDirectory)) { try { Directory.CreateDirectory(Settings.AppDirectory); } catch { } }
            if (!Directory.Exists(Settings.AppLogDirectory)) { try { Directory.CreateDirectory(Settings.AppLogDirectory); } catch { } }
            if (args.Length != 3) { SendResponse.Error("A quantidade de argumentos é inválida.", "E379269"); return; }

            Settings.TaskId = args[0];
            Settings.TaskName = args[1];
            Settings.Function = args[2];
            if (string.IsNullOrEmpty(Settings.TaskId) || string.IsNullOrWhiteSpace(Settings.TaskId)) { SendResponse.Error("TaskId é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return; }
            if (string.IsNullOrEmpty(Settings.TaskName) || string.IsNullOrWhiteSpace(Settings.TaskName)) { SendResponse.Error("TaskName é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return; }
            if (string.IsNullOrEmpty(Settings.Function) || string.IsNullOrWhiteSpace(Settings.Function)) { SendResponse.Error("Function é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return; }
            if (!File.Exists(Settings.AppRequestFile)) { SendResponse.Error("O arquivo Request.json não foi encontrado.", ErrorCode.FileNotFound); return; }

            Application.Run(new frm_Main(Settings.TaskId, Settings.TaskName, Settings.Function));
        }
    }
}