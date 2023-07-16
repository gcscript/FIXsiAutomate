using Au;
using FIXsi.Core.Services;
using FIXsi.Shared.Enums;
using FIXsi.Shared.Models;
using FIXsi.Shared;
using System.Text.Json;

namespace FIXsi.Core.Functions;
public class FIncluindoNovoCompromisso
{
    #region WINDOW
    private const string winName = "Incluindo novo compromisso";
    private const string winClass = "TfrmCadastroCompromisso";
    private const string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? cmb_Tipo;
    private static elm? cmb_Subtipo;
    private static elm? txt_DtPublicacao;
    private static elm? txt_Descricao;
    private static elm? chk_DiaInteiro;
    private static elm? txt_HorarioInicio;
    private static elm? btn_SalvarFechar;
    #endregion

    #region MODELS
    private static MIncluindoNovoCompromisso? model;
    #endregion

    public static void Start()
    {
        CheckData();
        wait.s(1);
        SetElements();
        wait.s(1);
        SetTipo();

        if (!string.IsNullOrEmpty(model!.Subtipo) && !string.IsNullOrWhiteSpace(model!.Subtipo))
        {
            wait.s(1);
            SetSubtipo();
        }

        if (!string.IsNullOrEmpty(model!.DtPublicacao) && !string.IsNullOrWhiteSpace(model!.DtPublicacao))
        {
            wait.s(1);
            SetDtPublicacao();
        }

        wait.s(1);
        SetDescricao();

        if (model!.DiaInteiro)
        {
            wait.s(1);
            SetDiaInteiro();
        }

        wait.s(1);
        SendResponse.Success("Novo compromisso incluído com sucesso.");
    }

    private static void CheckData()
    {
        LogEntry.Info("Verificando dados...");

        try
        {
            model = JsonSerializer.Deserialize<MIncluindoNovoCompromisso>(Settings.RequestFileJsonString);
        }
        catch
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson); return;
        }

        if (model is null)
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson); return;
        }

        if (string.IsNullOrEmpty(model.Tipo) || string.IsNullOrWhiteSpace(model.Tipo))
        {
            SendResponse.Error("Tipo é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return;
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

        #region CMD TIPO
        try
        {
            LogEntry.Info("Procurando elemento [Tipo]...");
            cmb_Tipo = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch6 fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Tipo] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region CMB SUBTIPO
        try
        {
            LogEntry.Info("Procurando elemento [Subtipo]...");
            cmb_Subtipo = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch5 fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Subtipo] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region TXT DTPUBLICACAO
        try
        {
            LogEntry.Info(message: "Procurando elemento [Data de Publicação]...");
            txt_DtPublicacao = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch9 fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Data de Publicação] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region TXT DESCRICAO
        try
        {
            LogEntry.Info("Procurando elemento [Descrição]...");
            txt_Descricao = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 ch3 fi3 ch2 fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Descrição] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region CHK DIA INTEIRO
        try
        {
            LogEntry.Info("Procurando elemento [Dia Inteiro]...");
            chk_DiaInteiro = win.Elm["CLIENT", win.Name, navig: "ch3 fi3 la fi ch8 fi"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Dia Inteiro] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region TXT HORARIO INICIO
        try
        {
            LogEntry.Info("Procurando elemento [Horário de Início]...");
            txt_HorarioInicio = win.Elm["CLIENT", win.Name, navig: "ch3 ch1 ch1 ch1 ch4 ch1 ch9 ch1 ch4 ch1 ch1 ch1"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Horário de Início] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region BTN SALVAR E FECHAR
        try
        {
            LogEntry.Info("Procurando botão [Salvar e Fechar]...");
            btn_SalvarFechar = win.Elm["BUTTON", "Salvar e Fechar", "class=TcxButton"].Find(3);
        }
        catch
        {
            SendResponse.Error("O botão [Salvar e Fechar] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

    }

    private static void SetTipo()
    {
        LogEntry.Info("Definindo valor do elemento [Tipo]...");
        MElementSettings es = new()
        {
            ElementName = "Tipo",
            ElementValue = model!.Tipo!,
            CheckIfItWasSuccessful = true,
            CheckIfTheApplicationCrashedCtrlF2 = true,
            ClearContent = true,
            ClickBefore = true,
            HitEnterAfter = true,
            SetFocusAndSelect = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.ElementSetTextComboboxMode1(win, cmb_Tipo!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Tipo].", "E745334");
        }
    }

    private static void SetSubtipo()
    {
        LogEntry.Info("Definindo valor do elemento [Subtipo]...");
        MElementSettings es = new()
        {
            ElementName = "Subtipo",
            ElementValue = model!.Subtipo!,
            CheckIfItWasSuccessful = true,
            CheckIfTheApplicationCrashedCtrlF2 = true,
            ClearContent = true,
            ClickBefore = true,
            HitEnterAfter = true,
            SetFocusAndSelect = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.ElementSetTextComboboxMode1(win, cmb_Subtipo!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Subtipo].", "E901750");
        }
    }

    private static void SetDtPublicacao()
    {
        LogEntry.Info("Definindo valor do elemento [DtPublicacao]...");
        MElementSettings es = new()
        {
            ElementName = "DtPublicacao",
            ElementValue = model!.DtPublicacao!,
            CheckIfItWasSuccessful = true,
            ClearContent = true,
            ClickBefore = true,
            SetFocusAndSelect = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_DtPublicacao!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [DtPublicacao].", "E219303");
        }
    }

    private static void SetDescricao()
    {
        LogEntry.Info("Definindo valor do elemento [Descricao]...");
        MElementSettings es = new()
        {
            ElementName = "Descricao",
            ElementValue = model!.Descricao!,
            CheckIfItWasSuccessful = true,
            ClearContent = true,
            ClickBefore = true,
            SetFocusAndSelect = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_Descricao!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Descricao].", "E219303");
        }
    }

    private static void SetDiaInteiro()
    {
        for (int i = 0; i < 6; i++)
        {
            LogEntry.Info("Definindo valor do elemento [Dia Inteiro]...");
            try
            {
                if (!txt_HorarioInicio!.IsInvisible)
                {
                    chk_DiaInteiro!.WaitFor(5, x => x.WndContainer.IsVisible);
                    chk_DiaInteiro.MouseClick();
                }

                txt_HorarioInicio.WaitFor(3, x => !x.WndContainer.IsVisible);

                return;
            }
            catch
            {
                LogEntry.Error("Erro ao definir valor do elemento [Dia Inteiro]!");
                continue;
            }
        }

        SendResponse.Error("Não foi possível definir o valor do elemento [Dia Inteiro].", "E605135");
    }

    private static void SaveAndClose()
    {
        for (int i = 0; i < 6; i++)
        {
            LogEntry.Info("Clicando no botão [Salvar e Fechar]...");
            try
            {
                btn_SalvarFechar!.WaitFor(5, x => x.WndContainer.IsVisible);
                btn_SalvarFechar.MouseClickD();

                try
                {
                    win.WaitForClosed(10);
                    return;
                }
                catch
                {
                    LogEntry.Error($"Erro ao fechar janela [{winName}]!");
                    continue;
                }
            }
            catch
            {
                LogEntry.Error("Erro ao clicar no botão [Salvar e Fechar]!");
                try
                {
                    win.WaitForClosed(10);
                    return;
                }
                catch
                {
                    LogEntry.Error($"Erro ao fechar janela [{winName}]!");
                    continue;
                }
            }
        }

        SendResponse.Error($"Não foi possível salvar o novo compromisso.", "E351508");
    }
}
