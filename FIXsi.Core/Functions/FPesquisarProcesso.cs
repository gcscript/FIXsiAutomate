using Au;
using FIXsi.Core.Services;
using FIXsi.Shared.Enums;
using FIXsi.Shared.Models;
using FIXsi.Shared;
using System.Text.Json;

namespace FIXsi.Core.Functions;

public static class FPesquisarProcesso
{
    #region WINDOW
    private const string winName = "CP-Pro Mais";
    private const string winClass = "TfrmCProc";
    private const string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? txt_Pesquisa;
    private static elm? btn_LimparPesquisa;
    private static elm? lbl_QuantidadePasta;
    #endregion

    #region MODEL
    private static MPesquisarProcesso? model;
    #endregion

    public static void Start()
    {
        CheckData();
        wait.s(1);
        SetElements();
        wait.s(1);
        Search();
        wait.s(1);
        SendResponse.Success("Processo pesquisado com sucesso.");
    }

    private static void CheckData()
    {
        LogEntry.Info("Verificando dados...");
        
        try
        {
            model = JsonSerializer.Deserialize<MPesquisarProcesso>(Settings.RequestFileJsonString);
        }
        catch
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson); return;
        }

        if (model is null)
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson); return;
        }

        if (string.IsNullOrEmpty(model.NrDoProcesso) || string.IsNullOrWhiteSpace(model.NrDoProcesso))
        {
            SendResponse.Error("Número do Processo é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return;
        }
    }


    private static void SetElements()
    {
        LogEntry.Info("Definindo elementos...");

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

        #region TXT PESQUISA
        try
        {
            LogEntry.Info("Procurando elemento [Pesquisa]...");
            txt_Pesquisa = win.Elm["CLIENT", win.Name, navig: "ch5 ch1 ch1 ch1 ch2 ch1 ch3 ch1 ch1 ch1 ch3 ch1 ch3 ch1 ch1 ch1 ch1 ch1"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Pesquisa] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region BTN LIMPAR PESQUISA
        try
        {
            LogEntry.Info("Procurando botão [Limpar Pesquisa]...");
            btn_LimparPesquisa = win.Elm["CLIENT", win.Name, navig: "ch5 ch1 ch1 ch1 ch2 ch1 ch3 ch1 ch1 ch1 ch3 ch1 ch3 ch1 ch3 ch1"].Find(3);
        }
        catch
        {
            SendResponse.Error("O botão [Limpar Pesquisa] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region LBL QUANTIDADE PASTA
        try
        {
            LogEntry.Info("Procurando elemento [Quantidade de Pastas]...");
            lbl_QuantidadePasta = win.Elm["CLIENT", win.Name, navig: "la fi la fi5"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Quantidade de Pastas] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

    }

    private static void Search()
    {
        LogEntry.Info("Pesquisando processo...");
        LogEntry.Info("Limpando campo [Pesquisa]...");
        try
        {
            LogEntry.Info("Aguardando botão [Limpar Pesquisa] ficar visível...");
            btn_LimparPesquisa!.WaitFor(15, x => x.WndContainer.IsVisible);
            LogEntry.Info("Clicando no botão [Limpar Pesquisa]...");
            btn_LimparPesquisa.MouseClickD();
            LogEntry.Info("Aguardando o elemento [Quantidade de Pastas] ficar com o valor [PASTA 0 DE 0]...");
            lbl_QuantidadePasta!.WaitFor(5, o => Tools.OnlyLettersAndNumbers(o.WndContainer.ControlText) == "PASTA0DE0");
        }
        catch (Exception)
        {
            SendResponse.Error("Falha ao limpar o campo [Pesquisa]", ErrorCode.ElementNotFound);
        }

        LogEntry.Info("Preenchendo campo [Pesquisa]...");

        MElementSettings es = new()
        {
            ElementName = "Pesquisa",
            ElementValue = model!.NrDoProcesso!,
            SetValueMode = EElementValueMode.Clipboard,
            ClearContent = true,
            SetFocusAndSelect = true,
            CheckIfItWasSuccessful = true,
            HitEnterAfter = true,
            CloseUnwantedWindows = false,
            ClickBefore = true
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_Pesquisa!, es))
        {
            SendResponse.Error("Falha ao preencher o campo [Pesquisa]", ErrorCode.ElementNotFound);
        }

        CancellationTokenSource cts = new();
        Task.Run(() => CheckIfProcessExists(cts.Token), cts.Token);

        try
        {
            LogEntry.Info("Aguardando o elemento [Quantidade de Pastas] ter o valor começado com [PASTA 1 DE]...");
            lbl_QuantidadePasta!.WaitFor(15, o => Tools.OnlyLettersAndNumbers(o.WndContainer.ControlText).StartsWith("PASTA1DE"));

            if (Tools.OnlyLettersAndNumbers(lbl_QuantidadePasta.WndContainer.ControlText) == "PASTA1DE1")
            {
                return;
            }
            else
            {
                SendResponse.WriteExcel("O processo possui mais de uma pasta!");
            }
        }
        catch
        {
            SendResponse.Error("Falha ao pesquisar o processo.", ErrorCode.GenericCatchError);
        }
        finally
        {
            cts?.Cancel();
        }
    }

    private static void CheckIfProcessExists(CancellationToken cancellationToken)
    {
        while (true)
        {
            SearchWindow();
            if (cancellationToken.IsCancellationRequested) { return; }
        }

        static void SearchWindow()
        {
            try
            {
                wnd alertWin = wnd.find(1, winName, "TfrmMsg", winProcess);
                alertWin.ActivateL(true);
                elm txt_Mensagem = alertWin.Elm["CLIENT", alertWin.Name, navig: "ch2 ch1 ch1 ch1 ch2 ch1"].Find(1);
                string message = txt_Mensagem.WndContainer.ControlText;

                if (Tools.OnlyLettersAndNumbers(message).Contains("NENHUMREGISTROENCONTRADONAPESQUISA"))
                {
                    alertWin.Close();
                    SendResponse.WriteExcel("Processo não encontrado!");
                }
            }
            catch { }
        }
    }
}