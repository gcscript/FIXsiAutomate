namespace FIXsi.Shared;
public class ErrorCode
{
    public static string DirectoryNotFound { get; } = "E168681";
    public static string ElementNotFound { get; } = "E148717";
    public static string ErrorDefiningElement { get; } = "E167969";
    public static string ErrorDefiningElements { get; } = "E167705";
    public static string ErrorFillingElement { get; } = "E166536";
    public static string FileNotFound { get; } = "E169882";
    public static string FunctionNotFound { get; } = "E171435";
    public static string GenericCatchError { get; } = "E159767";
    public static string InvalidJson { get; } = "E169888";
    public static string InvalidTypeOrSubtype { get; } = "E151744"; // Tipo|Subtipo Inválido!
    public static string IsNullOrEmptyOrWhiteSpace { get; } = "E166995";
    public static string NoRecordsFoundInSearch { get; } = "E101782"; // Processo não encontrado!
    public static string TimedOutAttempts { get; } = "E167249";
    public static string WindowNotFound { get; } = "E165496";
    public static string ErrorExecutingFunction { get; } = "E175494";
    public static string InvalidParameters { get; } = "E177478";
    public static string EmptyFile { get; } = "E180026";
    public static string IsNull { get; } = "E180489";
    public static string NullProperty { get; } = "E180763";
    public static string AppClosedByUser { get; } = "E673343";

}