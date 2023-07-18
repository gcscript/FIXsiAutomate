using Au;
using FIXsi.Core.Services;
using FIXsi.Shared.Enums;
using FIXsi.Shared.Models;
using FIXsi.Shared;
using System.Text.Json;

namespace FIXsi.Core.Functions;
public class FCadastroDeAndamentoProcessual
{
    #region WINDOW
    private const string winName = "Cadastro de andamento processual";
    private const string winClass = "TfrmProcAcompIncAlt";
    private const string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? txt_Data;
    private static elm? cmb_Tipo;
    private static elm? cmb_Subtipo;
    private static elm? txt_Descricao;
    private static elm? txt_ObservacoesSobreOAndamento;
    private static elm? btn_SalvarFechar;
    #endregion

    #region MODELS
    private static MCadastroDeAndamentoProcesssual? model;
    #endregion

    public static void Start()
    {
        LogEntry.Info("Iniciando função [Cadastro de andamento processual]...");

        CheckData();
        wait.s(1);
        SetElements();

        if (!string.IsNullOrEmpty(model!.Data) && !string.IsNullOrWhiteSpace(model!.Data))
        {
            wait.s(1);
            SetData();
        }

        wait.s(1);
        SetTipo();

        if (!string.IsNullOrEmpty(model!.Subtipo) && !string.IsNullOrWhiteSpace(model!.Subtipo))
        {
            wait.s(1);
            SetSubtipo();
        }

        if (!string.IsNullOrEmpty(model!.Descricao) && !string.IsNullOrWhiteSpace(model!.Descricao))
        {
            wait.s(1);
            SetDescricao();
        }

        if (!string.IsNullOrEmpty(model!.ObservacoesSobreOAndamento) && !string.IsNullOrWhiteSpace(model!.ObservacoesSobreOAndamento))
        {
            wait.s(1);
            SetObservacoesSobreOAndamento();
        }

        wait.s(1);
        SaveAndClose();

        wait.s(1);
        SendResponse.Success("Andamento processual cadastrado com sucesso.");
    }

    private static void CheckData()
    {
        LogEntry.Info("Verificando dados...");

        try
        {
            model = JsonSerializer.Deserialize<MCadastroDeAndamentoProcesssual>(Settings.RequestFileJsonString);
        }
        catch
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson);
        }

        if (model is null)
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson);
        }

        if (string.IsNullOrEmpty(model!.Tipo) || string.IsNullOrWhiteSpace(model.Tipo))
        {
            SendResponse.Error("Tipo é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace);
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

        #region TXT DATA
        try
        {
            LogEntry.Info("Procurando elemento [Data]...");
            txt_Data = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi la fi ch11 fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Data] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region CMD TIPO
        try
        {
            LogEntry.Info("Procurando elemento [Tipo]...");
            cmb_Tipo = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi la fi ch2 fi3"].Find(3);
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
            cmb_Subtipo = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi la fi ch13 fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Subtipo] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region TXT DESCRICAO
        try
        {
            LogEntry.Info("Procurando elemento [Descrição]...");
            txt_Descricao = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi ch2 fi3 la fi"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Descrição] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region TXT OBSERVACOES SOBRE O ANDAMENTO
        try
        {
            LogEntry.Info("Procurando elemento [Observacoes sobre o andamento]...");
            txt_ObservacoesSobreOAndamento = win.Elm["CLIENT", win.Name, navig: "ch2 fi5 la fi ch2 fi ch3 fi la fi"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Observacoes sobre o andamento] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region BTN SALVAR E FECHAR
        try
        {
            LogEntry.Info("Procurando botão [Salvar e Fechar]...");
            btn_SalvarFechar = win.Elm["BUTTON", "Salvar e Fechar", "class=TPLKButton"].Find(3);
        }
        catch
        {
            SendResponse.Error("O botão [Salvar e Fechar] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

    }

    private static void SetData()
    {
        LogEntry.Info("Definindo valor do elemento [Data]...");
        MElementSettings es = new()
        {
            ElementName = "Data",
            ElementValue = model!.Data!,
            CheckIfItWasSuccessful = true,
            ClearContent = true,
            ClickBefore = true,
            SetFocusAndSelect = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_Data!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Data].", "E978903");
        }
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
            SendResponse.Error("Não foi possível definir o valor do elemento [Tipo].", "E642574");
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
            SendResponse.Error("Não foi possível definir o valor do elemento [Subtipo].", "E930857");
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
            SendResponse.Error("Não foi possível definir o valor do elemento [Descricao].", "E582400");
        }
    }

    private static void SetObservacoesSobreOAndamento()
    {
        LogEntry.Info("Definindo valor do elemento [Observacoes sobre o andamento]...");
        MElementSettings es = new()
        {
            ElementName = "Observacoes sobre o andamento",
            ElementValue = model!.ObservacoesSobreOAndamento!,
            CheckIfItWasSuccessful = true,
            ClearContent = true,
            ClickBefore = true,
            SetFocusAndSelect = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_ObservacoesSobreOAndamento!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Observacoes sobre o andamento].", "E560168");
        }
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

        SendResponse.Error($"Não foi possível salvar o novo compromisso.", "E796097");
    }
}
