using Au;
using FIXsi.Shared;

namespace FIXsi.Core.Functions;
public class FIrParaCadastroDeAndamentoProcessual
{
    #region WINDOW
    private const string winName = "CP-Pro Mais";
    private const string winClass = "TfrmCProc";
    private const string winProcess = "CProc.exe";

    private const string? win2Name = null;
    private const string win2Class = "#32768";
    private const string win2Process = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static wnd win2;
    private static elm? btnIncluir;
    private static elm? btnAcompanhamentos;
    private static elm? btnIncluirAndamento;
    #endregion

    public static void Start()
    {
        GoTo();
        SendResponse.Success("Chegou ao [Cadastro de Andamento Processual]");
    }

    private static void GoTo()
    {
        LogEntry.Info("Indo para Cadastro de Andamento Processual...");

        #region WINDOW
        try
        {
            LogEntry.Info($"Procurando janela [{winName}]...");
            win = wnd.find(Settings.TimeoutToFindWindow, winName, winClass, winProcess);
            win.ActivateL(true);
        }
        catch (Exception)
        {
            SendResponse.Error($"A janela [{winName}] não foi encontrada.", ErrorCode.WindowNotFound);
        }
        #endregion

        for (int i = 0; i < 6; i++)
        {
            #region BUTTON INCLUIR/ANDAMENTOS
            try
            {
                win.ActivateL(true);
                LogEntry.Info($"Procurando botão [Incluir]...");
                btnIncluir = win.Elm["WINDOW", prop: "class=TfrmAcompanhamento"]["BUTTON", "Incluir"].Find(3);
                btnIncluir.WaitFor(10, x => x.WndContainer.IsVisible);
                //btnIncluir.MouseClick();
                btnIncluir.Invoke();
            }
            catch (Exception)
            {
                try
                {
                    win.ActivateL(true);
                    LogEntry.Error("O botão [Incluir] não foi encontrado!");
                    LogEntry.Info($"Procurando botão [Acompanhamentos]...");
                    btnAcompanhamentos = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi3"]["CLIENT", "Acompanhamentos"].Find(3);
                    btnAcompanhamentos.WaitFor(10, x => x.WndContainer.IsVisible);
                    btnAcompanhamentos.MouseClick();
                    continue;
                }
                catch
                {
                    SendResponse.Error("O botão [Acompanhamentos] não foi encontrado!", "E135879");
                }
            }
            #endregion

            #region WINDOW 2
            try
            {
                LogEntry.Info($"Procurando janela do botão [Incluir]...");
                win2 = wnd.find(3, win2Name, win2Class, win2Process);
                //win.ActivateL(true);
            }
            catch (Exception)
            {
                LogEntry.Error($"A janela do botão [Incluir] não foi encontrada!");
                continue;
            }
            #endregion

            #region BUTTON INCLUIR ANDAMENTO
            try
            {
                LogEntry.Info("Procurando botão [Incluir Andamento]");
                btnIncluirAndamento = win2.Elm["MENUITEM", "Incluir Andamento*"].Find(3);
                btnIncluirAndamento.WaitFor(3, x => x.WndContainer.IsVisible);
                btnIncluirAndamento.Invoke();
                SendResponse.Success("Chegou ao [Cadastro de Andamento Processual]");
            }
            catch (Exception)
            {
                LogEntry.Error("O botão [Incluir Andamento] não foi encontrado!");
                continue;
            }
            #endregion
        }

        SendResponse.Error("Erro ao ir para [Cadastro de Andamento Processual]", "E135879");
    }
}