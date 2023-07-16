using FIXsi.Shared.Enums;

namespace FIXsi.Shared.Models;

public class MElementSettings
{
    public string ElementName { get; set; } = string.Empty;
    public string ElementValue { get; set; } = string.Empty;
    public EElementValueMode SetValueMode { get; set; }
    public bool ClearContent { get; set; }
    public bool SetFocusAndSelect { get; set; }
    public bool HitEnterAfter { get; set; }
    public bool ClickBefore { get; set; }
    public bool CheckIfItWasSuccessful { get; set; }
    public bool CloseUnwantedWindows { get; set; }
    public bool CheckIfTheApplicationCrashedCtrlF2 { get; set; }
}