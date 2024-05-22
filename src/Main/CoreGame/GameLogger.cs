namespace Main.CoreGame;
internal class GameLogger
{
    private BufferedStringArray _logs = new BufferedStringArray(1000);

    public void WriteLogs(IEnumerable<string> inputStrings) =>
        _logs.WriteStrings(inputStrings);

    public void WriteLog(string input) =>
        _logs.WriteString(input);

    public string[] ReadLogs(int size) =>
        _logs.ReadTopStrings(size);
}
