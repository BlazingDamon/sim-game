namespace Main;

internal static class GameDebugLogger
{
    private static BufferedStringArray _logs = new BufferedStringArray(1000);

    public static void WriteLogs(IEnumerable<string> inputStrings) => 
        _logs.WriteStrings(inputStrings);

    public static void WriteLog(string input) =>
        _logs.WriteString(input);

    public static string[] ReadLogs(int size) => 
        _logs.ReadTopStrings(size);
}
