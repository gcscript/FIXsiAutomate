using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FIXsi.Shared;
public static class Tools
{
    public static string TreatText(string text, bool trim = true, bool toUpper = true, bool removeAccents = true, bool removeSpaces = true)
    {
        if (trim)
            text = text.Trim();
        if (toUpper)
            text = text.ToUpper();
        if (removeAccents)
            text = RemoveAccents(text);
        if (removeSpaces)
            text = RemoveSpaces(text);
        return text;
    }

    public static string RemoveAccents(string texto)
    {
        var stringBuilder = new StringBuilder();
        StringBuilder sbReturn = stringBuilder;
        var arrayText = texto.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }

    public static string RemoveSpaces(string texto)
    {
        texto = Regex.Replace(texto, @"\s", "");
        texto = texto.Trim();

        return texto;
    }

    public static string OnlyLetters(string? text)
    {
        if (text is null) { return ""; }
        text = text.ToUpper();
        text = Regex.Replace(text, @"[^a-zA-Z]", "");
        return text;
    }

    public static string OnlyLettersAndNumbers(string? text)
    {
        if (text is null) { return ""; }
        text = text.ToUpper();
        text = Regex.Replace(text, @"[^a-zA-Z0-9]", "");
        return text;
    }

    public static DateTime DateParser(string? input)
    {
        if (input is null) { return DateTime.MinValue; }

        try
        {
            //var formats = DateTimeFormatInfo.InvariantInfo.GetAllDateTimePatterns();

            string[] formats = { "dd/MM/yyyy HH:mm:ss:fff",
                                 "dd/MM/yyyy HH:mm:ss",
                                 "dd/MM/yyyy HH:mm",
                                 "dd/MM/yyyy",
                                 "d/MM/yyyy HH:mm:ss:fff",
                                 "d/MM/yyyy HH:mm:ss",
                                 "d/MM/yyyy HH:mm",
                                 "d/MM/yyyy",
                                 "d/M/yyyy HH:mm:ss:fff",
                                 "d/M/yyyy HH:mm:ss",
                                 "d/M/yyyy HH:mm",
                                 "d/M/yyyy",
                                 "d/M/yy HH:mm:ss:fff",
                                 "d/M/yy HH:mm:ss",
                                 "d/M/yy HH:mm",
                                 "d/M/yy" };

            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        catch (Exception)
        {

            return DateTime.MinValue;
        }
    }

    public static int GetDifferenceInDays(DateTime date1, DateTime date2)
    {
        return (int)(date2 - date1).TotalDays;
    }
}