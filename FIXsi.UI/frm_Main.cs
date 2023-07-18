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
            Screen screen = Screen.PrimaryScreen!; // Obtém a tela primária
            Rectangle workingArea = screen.WorkingArea; // Obtém a área de trabalho
            int x = (workingArea.Width - Width) / 2; // Centraliza horizontalmente
            int y = workingArea.Bottom - Height - 1; // Centraliza verticalmente
            DesktopLocation = new Point(x, y); // Define a posição do formulário
            TopMost = true; // Coloca o formulário na frente de todos os outros
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
                case "IrParaCadastroDeAndamentoProcessual":
                    FIrParaCadastroDeAndamentoProcessual.Start();
                    break;
                case "CadastroDeAndamentoProcessual":
                    FCadastroDeAndamentoProcessual.Start();
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