using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

class Program
{
    static void Main()
    {
        string inputFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\input.txt");

        string outputFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\output.txt"); 
        string problemFile = Path.Combine(AppContext.BaseDirectory, @"..\..\..\problems.txt");

        var lines = File.ReadAllLines(inputFile);
        using StreamWriter outputWriter = new(outputFile);
        using StreamWriter problemWriter = new(problemFile);

        foreach (var line in lines)
        {
            string result = ParseLine(line);
            if (result == null)
                problemWriter.WriteLine(line);
            else
                outputWriter.WriteLine(result);
        }

        Console.WriteLine("Обработка завершена.");
    }

    static string ParseLine(string line)
    {
        // Формат 1: DD.MM.YYYY HH:mm:ss.fff LEVEL Сообщение
        var format1 = new Regex(@"^(?<date>\d{2}\.\d{2}\.\d{4}) (?<time>\d{2}:\d{2}:\d{2}\.\d{3}) (?<level>[A-Z]+) (?<message>.+)$");

        // Формат 2: YYYY-MM-DD HH:mm:ss.ffff| LEVEL|...|Method| Message
        var format2 = new Regex(@"^(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}\.\d+)\| (?<level>[A-Z]+)\|\d+\|(?<method>[^|]+)\| (?<message>.+)$");

        Match match;

        if ((match = format1.Match(line)).Success)
        {
            // Преобразуем дату
            if (!DateTime.TryParseExact(match.Groups["date"].Value, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime dt))
                return null;

            string date = dt.ToString("yyyy-MM-dd");
            string time = match.Groups["time"].Value;
            string level = NormalizeLevel(match.Groups["level"].Value);
            string method = "DEFAULT";
            string message = match.Groups["message"].Value;

            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }
        else if ((match = format2.Match(line)).Success)
        {
            string date = match.Groups["date"].Value;
            string time = match.Groups["time"].Value;
            string level = NormalizeLevel(match.Groups["level"].Value);
            string method = match.Groups["method"].Value;
            string message = match.Groups["message"].Value;

            return $"{date}\t{time}\t{level}\t{method}\t{message}";
        }

        return null; // Не удалось распарсить
    }

    static string NormalizeLevel(string level)
    {
        return level switch
        {
            "INFORMATION" => "INFO",
            "INFO" => "INFO",
            "WARNING" => "WARN",
            "WARN" => "WARN",
            "ERROR" => "ERROR",
            "DEBUG" => "DEBUG",
            _ => "INFO"
        };
    }
}
