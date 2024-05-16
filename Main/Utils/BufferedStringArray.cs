namespace Main;

internal class BufferedStringArray
{
    private int MAX_LINES = 1000;
    private int writePosition = 0;
    readonly string[] buffer;

    public BufferedStringArray(int? maxLines = default)
    {
        MAX_LINES = maxLines ?? MAX_LINES;
        buffer = new string[MAX_LINES];
    }

    public void WriteStrings(IEnumerable<string> inputStrings)
    {
        foreach (var line in inputStrings)
        {
            buffer[writePosition++] = line;
            writePosition %= MAX_LINES;
        }
    }

    public void WriteString(string input)
    {
        buffer[writePosition++] = input;
        writePosition %= MAX_LINES;
    }

    // 1 2 3 n n n
    // wp = 3   3
    // s = 2    10
    // k = 1    0

    // 4 5 6 1 2 3
    // wp = 3    3
    // s = 2     10
    // k = 1     0

    public string[] ReadTopStrings(int size) =>
        buffer
            .Skip((writePosition - Math.Min(size, MAX_LINES)) % MAX_LINES)
            .Take(Math.Min(size, MAX_LINES))
            .Concat(buffer.Take((writePosition - Math.Min(size, MAX_LINES)) % MAX_LINES))
            .Where(line => line != null)
            .Take(Math.Min(size, MAX_LINES))
            .ToArray();


    public string[] ReadOldestStrings(int size) =>
        buffer
            .Skip(writePosition)
            .Take(Math.Min(size, writePosition))
            .Concat(buffer.Take(Math.Min(size, writePosition)))
            .Where(line => line != null)
            .Take(Math.Min(size, writePosition))
            .ToArray();
}
