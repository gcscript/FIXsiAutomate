using FIXsi.Core.Functions;
using FIXsi.Shared;

namespace FIXsi.UI;

public partial class frm_Main : Form
{
    public frm_Main(string taskId, string taskName, string function)
    {
        InitializeComponent();
        SetFormPosition();
    }

    private void btn_Close_Click(object sender, EventArgs e)
    {
        SendResponse.Error("O aplicativo foi fechado pelo usuário.", ErrorCode.AppClosedByUser);
        Close();
    }
    private async void frm_Main_Load(object sender, EventArgs e)
    {
        await Start();
    }

    private void SetFormPosition()
    {
        try
        {
            Screen screen = Screen.PrimaryScreen!; // Get the primary screen
            Rectangle workingArea = screen.WorkingArea; // Get the working area of the screen
            int x = (workingArea.Width - Width) / 2; // Center horizontally
            int y = workingArea.Bottom - Height - 1; // Align to bottom
            DesktopLocation = new Point(x, y); // Set the location
            TopMost = true; // Make the form topmost
        }
        catch { }
    }

    private static async Task Start()
    {
        await Task.Run(() =>
        {
            RemoveAppResponseFile();
            Settings.RequestFileJsonString = File.ReadAllText(Settings.AppRequestFile);

            if (string.IsNullOrEmpty(Settings.RequestFileJsonString) || string.IsNullOrWhiteSpace(Settings.RequestFileJsonString))
            {
                SendResponse.Error("Request.json é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return;
            }

            #region Check Function
            switch (Settings.Function)
            {
                case "Login":
                    FLogin.Start();
                    break;
                case "PesquisarProcesso":
                    FPesquisarProcesso.Start();
                    break;
                case "IrParaIncluindoNovoCompromisso":
                    FIrParaIncluindoNovoCompromisso.Start();
                    break;
                case "IncluindoNovoCompromisso":
                    FIncluindoNovoCompromisso.Start();
                    break;
                default:
                    SendResponse.Error("Função não encontrada.", ErrorCode.FunctionNotFound);
                    break;
            }
            #endregion
        });
    }

    private static void RemoveAppResponseFile() { if (File.Exists(Settings.AppResponseFile)) { try { File.Delete(Settings.AppResponseFile); } catch { } } }
}