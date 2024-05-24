namespace Main;

internal class BufferedArray<T>
{
    private int MAX_LINES = 1000;
    private int writePosition = 0;
    readonly T[] buffer;

    public BufferedArray(int? maxLines = default)
    {
        MAX_LINES = maxLines ?? MAX_LINES;
        buffer = new T[MAX_LINES];
    }

    public void WriteValues(IEnumerable<T> input)
    {
        foreach (var value in input)
        {
            buffer[writePosition++] = value;
            writePosition %= MAX_LINES;
        }
    }

    public void WriteValue(T input)
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

    public T[] ReadTopValues(int size) =>
        buffer
            .Skip((writePosition - Math.Min(size, MAX_LINES)) % MAX_LINES)
            .Take(Math.Min(size, MAX_LINES))
            .Concat(buffer.Take((writePosition - Math.Min(size, MAX_LINES)) % MAX_LINES))
            .Where(line => line != null)
            .Take(Math.Min(size, MAX_LINES))
            .ToArray();


    public T[] ReadOldestValues(int size) =>
        buffer
            .Skip(writePosition)
            .Take(Math.Min(size, writePosition))
            .Concat(buffer.Take(Math.Min(size, writePosition)))
            .Where(line => line != null)
            .Take(Math.Min(size, writePosition))
            .ToArray();
}
