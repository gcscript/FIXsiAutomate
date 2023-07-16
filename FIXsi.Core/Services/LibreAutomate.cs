using Au;
using Au.Types;
using FIXsi.Shared;
using FIXsi.Shared.Enums;
using FIXsi.Shared.Models;

namespace FIXsi.Core.Services;

public static class LibreAutomate
{
    /// <summary>
    /// Define o valor de um elemento especificado.
    /// </summary>
    /// <param name="win">Janela onde o elemento está localizado.</param>
    /// <param name="element">Elemento a ser definido.</param>
    /// <param name="es">Configurações do elemento.</param>
    /// <returns>true se o valor foi definido com sucesso.</returns>
    public static bool SetValueOnElement(wnd win, elm element, MElementSettings es)
    {
        try
        {
            if (es.CloseUnwantedWindows) { Task.Run(() => { CloseUnwantedWindows(); }); } // Ativa a função Fechar Janelas Indesejadas em uma nova thread.
            win.ActivateL(true); // Ativa a janela principal.
            es.ElementValue = es.ElementValue.Trim();
            if (es.SetFocusAndSelect) { SetFocusAndSelectElement(element); } // Define o foco e/ou seleciona o elemento.
            if (es.ClearContent) { ClearElementContents(element); } // Limpa o conteúdo do elemento.
            if (es.ClickBefore) { try { mouse.doubleClick(element.Rect.CenterX, element.Rect.CenterY); } catch { } } // Clica no elemento antes de definir o valor.

            switch (es.SetValueMode)
            {
                case EElementValueMode.Clipboard: try { clipboard.clear(); clipboard.paste(es.ElementValue); } catch { } break; // Define o valor do elemento através da área de transferência.
                case EElementValueMode.SendKeys: try { element.SendKeys($"!{es.ElementValue}"); } catch { } break; // Define o valor do elemento através do SendKeys.
                case EElementValueMode.KeysSend: try { keys.send($"{es.ElementValue}"); } catch { } break; // Define o valor do elemento através do KeysSend.
                default: break;
            }

            if (es.CheckIfItWasSuccessful) // Verifica se o valor foi definido com sucesso.
            {
                if (!IsCurrentValueEqualToDefinedValue(element, es.ElementValue)) { return false; } // Se o valor não foi definido com sucesso, retorna false.
            }

            if (es.HitEnterAfter) { element.SendKeys("Enter"); } // Pressiona a tecla Enter após definir o valor.

            if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue)) { return true; } else { return false; } // Verifica se o valor foi definido com sucesso. Se sim, retorna true. Se não, retorna false.
        }
        catch { return false; } // Se ocorrer algum erro, retorna false.
        finally
        {
            Settings.CloseUnwantedWindowsRepeat = false; // Desativa a função Fechar Janelas Indesejadas.
        }
    }


    /// <summary>
    /// Fecha as janelas indesejadas.
    /// </summary>
    /// <param name="repetitions">Quantidade de vezes que a função será executada. Se o valor for igual a 0, a função será executada até que a variável Settings.CloseUnwantedWindowsRepeat seja igual a false.</param>
    private static void CloseUnwantedWindows(int repetitions = 0)
    {
        Settings.CloseUnwantedWindowsRepeat = true; // Ativa a função Fechar Janelas Indesejadas.
        repetitions = repetitions < 0 ? repetitions * -1 : repetitions; // Se o valor for negativo, converte para positivo.
        if (repetitions == 0) { while (Settings.CloseUnwantedWindowsRepeat == true) { CloseWin(); } } // Enquanto a função Fechar Janelas Indesejadas estiver ativa, fecha as janelas.
        else { for (int i = 0; i < repetitions; i++) { if (Settings.CloseUnwantedWindowsRepeat == true) { CloseWin(); } } }

        static void CloseWin()
        {
            try
            {
                wnd.find(1, null, "**m(^^^) TfrmMsg^^^#32770^^^TcxComboBoxPopupWindow", "CProc.exe").Close();
            }
            catch (Exception) { }
        }
        return;
    }

    /// <summary>
    /// Define o foco e seleciona um elemento especificado.
    /// </summary>
    /// <param name="element">O elemento a ser focado e selecionado.</param>
    /// <returns>Um valor inteiro que indica o resultado da operação:
    /// 0 = Focado e Selecionado;
    /// 1 = Focado;
    /// 2 = Selecionado;
    /// 3 = Erro</returns>
    public static int SetFocusAndSelectElement(elm element)
    {
        try
        {
            try { element.Focus(true); } catch (Exception) { }
            if (element.IsFocused && element.IsSelected) { return 0; }

            try { element.Focus(); } catch (Exception) { }
            if (element.IsFocused) { return 1; }

            try { element.Select(); } catch (Exception) { }
            if (element.IsSelected) { return 2; }

            return 3;
        }
        catch { return 0; }
    }

    /// <summary>
    /// Limpa o conteúdo de um elemento especificado.
    /// </summary>
    /// <param name="element">O elemento a ser limpo.</param>
    public static void ClearElementContents(elm element)
    {
        try
        {
            try { element.SendKeys("Ctrl+A", "Del"); } catch { }
            if (string.IsNullOrEmpty(element.Value)) { return; }

            try { element.SendKeys("Ctrl+A", "Back"); } catch { }
            if (string.IsNullOrEmpty(element.Value)) { return; }

            try { element.SendKeys("Ctrl+Home", "Ctrl+Shift+End", "Del"); } catch { }
            if (string.IsNullOrEmpty(element.Value)) { return; }

            try { element.SendKeys("Ctrl+Home", "Ctrl+Shift+End", "Back"); } catch { }
            if (string.IsNullOrEmpty(element.Value)) { return; }

            try { element.Value = ""; } catch { }
        }
        catch { }
    }

    /// <summary>
    /// Verifica se o valor atual de um elemento é igual ao valor definido.
    /// </summary>
    /// <param name="element">O elemento a ser verificado.</param>
    /// <param name="value">O valor a ser comparado.</param>
    /// <param name="wait">O tempo de espera em segundos.</param>
    /// <returns>true se o valor atual for igual ao valor definido.</returns>
    public static bool IsCurrentValueEqualToDefinedValue(elm element, string value, int wait = 1)
    {
        try
        {
            wait = wait < 0 ? wait *= -1 : wait;
            element.WaitFor(wait, o => Tools.OnlyLettersAndNumbers(o.WndContainer.ControlText).Eq(Tools.OnlyLettersAndNumbers(value)));
            return true;
        }
        catch (Exception) { return false; }
    }

    /// <summary>
    /// Verifica se a aplicação está travada.
    /// </summary>
    /// <param name="element">O elemento onde vai ser enviado o comando Ctrl+F2.</param>
    /// <returns>true se a aplicação estiver travada.</returns>
    private static bool ApplicationIsStuckMode1(elm element)
    {
        for (int b = 0; b < 18; b++)
        {
            var elementCenterX = element.Rect.CenterX;
            var elementCenterY = element.Rect.CenterY;
            mouse.doubleClick(elementCenterX, elementCenterY);
            element.SendKeys("Ctrl+F2");
            var win = wnd.find(-5, null, "TfrmManutTarefa");
            if (win.Is0) { continue; }
            win.Close();
            win.WaitForClosed(-5);
            win = wnd.find(-1, null, "TfrmManutTarefa");
            if (win.Is0) { return false; }
        }
        return true;
    }

    /// <summary>
    /// Define o valor de um elemento do tipo ComboBox.
    /// </summary>
    /// <param name="win">Janala onde o elemento está localizado.</param>
    /// <param name="element">Elemento a ser definido.</param>
    /// <param name="es">Configurações do elemento.</param>
    /// <returns>true se o valor foi definido com sucesso.</returns>
    public static bool ElementSetTextComboboxMode1(wnd win, elm element, MElementSettings es)
    {
        try
        {
            Task.Run(() => { CloseUnwantedWindows(); }); // Ativa a função Fechar Janelas Indesejadas em uma nova thread.

            es.ElementValue = es.ElementValue.Trim();

            if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue)) { return true; }
            if (es.CheckIfTheApplicationCrashedCtrlF2) { if (ApplicationIsStuckMode1(element)) { return false; } }
            SetValueOnElement(win, element, new MElementSettings
            {
                ElementName = es.ElementName,
                ElementValue = es.ElementValue,
                SetValueMode = EElementValueMode.Clipboard,
                ClearContent = true,
                SetFocusAndSelect = true,
                CheckIfItWasSuccessful = true,
                CloseUnwantedWindows = false,
                HitEnterAfter = true
            });

            if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue))
            {
                wait.s(3);
                if (es.CheckIfTheApplicationCrashedCtrlF2) { if (ApplicationIsStuckMode1(element)) { return false; } }
                if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue)) { return true; }
            }

            element.SendKeys("Up*50");

            if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue))
            {
                wait.s(1);
                if (es.CheckIfTheApplicationCrashedCtrlF2) { if (ApplicationIsStuckMode1(element)) { return false; } }
                if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue)) { return true; }

            }

            string lastValue = "[L][a][s][t][V][a][l][u][e]";
            while (true)
            {
                element.SendKeys("Down");
                wait.s(1);

                if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue))
                {
                    wait.s(1);
                    if (es.CheckIfTheApplicationCrashedCtrlF2) { if (ApplicationIsStuckMode1(element)) { return false; } }
                    if (IsCurrentValueEqualToDefinedValue(element, es.ElementValue)) { return true; }
                }

                if (element.WndContainer.ControlText.Trim() == lastValue.Trim()) // CHEGOU AO FIM DO COMBOBOX
                {
                    break;
                }
                else
                {
                    lastValue = element.WndContainer.ControlText;
                    continue;
                }
            }

            return false;
        }
        catch { return false; }
        finally
        {
            Settings.CloseUnwantedWindowsRepeat = false; // Desativa a função Fechar Janelas Indesejadas.
        }

    }

}
