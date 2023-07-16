using Au;
using FIXsi.Core.Services;
using FIXsi.Shared.Enums;
using FIXsi.Shared.Models;
using FIXsi.Shared;
using System.Text.Json;

namespace FIXsi.Core.Functions;

public static class FLogin
{
    #region WINDOW
    private const string winName = "CP-Pro - Login de usuário";
    private const string winClass = "TfrmLoginMais";
    private const string winProcess = "CProc.exe";
    #endregion

    #region ELEMENTS
    private static wnd win;
    private static elm? txt_Usuario;
    private static elm? txt_Senha;
    #endregion

    #region MODEL
    private static MLogin? model;
    #endregion

    public static void Start()
    {
        CheckData();
        SetElements();
        SetUsername();
        SetPassword();
        wait.s(1);
        SendResponse.Success("Login realizado com sucesso.");
    }

    private static void CheckData()
    {
        LogEntry.Info("Verificando dados...");

        try
        {
            model = JsonSerializer.Deserialize<MLogin>(Settings.RequestFileJsonString);
        }
        catch
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson); return;
        }

        if (model is null)
        {
            SendResponse.Error("Request.json inválido.", ErrorCode.InvalidJson); return;
        }

        if (string.IsNullOrEmpty(model.Usuario) || string.IsNullOrWhiteSpace(model.Usuario))
        {
            SendResponse.Error("Usuário é nulo ou vazio ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return;
        }

        if (string.IsNullOrEmpty(model.Senha) || string.IsNullOrWhiteSpace(model.Senha))
        {
            SendResponse.Error("Senha é nula ou vazia ou contém apenas espaços em branco.", ErrorCode.IsNullOrEmptyOrWhiteSpace); return;
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

        #region TXT USUARIO
        try
        {
            LogEntry.Info("Procurando elemento [Usuario]...");
            txt_Usuario = win.Elm["CLIENT", win.Name, navig: "la fi3"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Usuario] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion

        #region TXT SENHA
        try
        {
            LogEntry.Info("Procurando elemento [Senha]...");
            txt_Senha = win.Elm["CLIENT", win.Name, navig: "fi4"].Find(3);
        }
        catch
        {
            SendResponse.Error("O elemento [Senha] não foi encontrado.", ErrorCode.ElementNotFound);
        }
        #endregion
    }

    private static void SetUsername()
    {
        LogEntry.Info("Definindo valor do elemento [Usuário]...");
        MElementSettings es = new()
        {
            ElementName = "Usuário",
            ElementValue = model!.Usuario!,
            CheckIfItWasSuccessful = true,
            ClearContent = true,
            SetFocusAndSelect = true,
            ClickBefore = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_Usuario!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Usuário].", "E826873");
        }
    }
    private static void SetPassword()
    {
        LogEntry.Info("Definindo valor do elemento [Senha]...");
        MElementSettings es = new()
        {
            ElementName = "Senha",
            ElementValue = model!.Senha!,
            CheckIfItWasSuccessful = true,
            ClearContent = true,
            HitEnterAfter = true,
            SetFocusAndSelect = true,
            ClickBefore = true,
            SetValueMode = EElementValueMode.Clipboard
        };

        if (!LibreAutomate.SetValueOnElement(win, txt_Senha!, es))
        {
            SendResponse.Error("Não foi possível definir o valor do elemento [Senha].", "E826873");
        }
    }
}