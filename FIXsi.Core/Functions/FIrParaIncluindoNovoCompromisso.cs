using Au;
using FIXsi.Shared;

namespace FIXsi.Core.Functions;
public class FIrParaIncluindoNovoCompromisso
{
    #region WINDOW
    private const string winName = "CP-Pro Mais";
    private const string winClass = "TfrmCProc";
    private const string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? btnIncluir;
    private static elm? btnCompromissos;
    #endregion

    public static void Start()
    {
        GoTo();
        SendResponse.Success("Chegou ao [Incluindo Novo Compromisso]");
    }

    private static void GoTo()
    {
        LogEntry.Info("Indo para Incluindo Novo Compromisso...");

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
            #region BUTTON INCLUIR/COMPROMISSOS
            try
            {
                win.ActivateL(true);
                LogEntry.Info($"Procurando botão [Incluir]...");
                btnIncluir = win.Elm["WINDOW", prop: "class=TfrmCompromisso"]["BUTTON", "Incluir*"].Find(3);
                btnIncluir.WaitFor(10, x => x.WndContainer.IsVisible);
                btnIncluir.MouseClickD();
                return;
            }
            catch (Exception)
            {
                try
                {
                    win.ActivateL(true);
                    LogEntry.Error("O botão [Incluir] não foi encontrado!");
                    LogEntry.Info($"Procurando botão [Compromissos]...");
                    btnCompromissos = win.Elm["CLIENT", win.Name, navig: "la fi3 ch2 fi la fi3 ch2 fi la fi2"]["CLIENT", "Compromissos"].Find(3);
                    btnCompromissos.WaitFor(10, x => x.WndContainer.IsVisible);
                    btnCompromissos.MouseClick();
                    continue;
                }
                catch
                {
                    SendResponse.Error("O botão [Compromissos] não foi encontrado!", "E135879");
                }
            }
            #endregion
        }

        SendResponse.Error("Erro ao ir para [Incluindo Novo Compromisso]", "E135879");
    }
}