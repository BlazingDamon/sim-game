namespace Main;
internal class GameRandom
{
    public static Random Random = new Random(GameGlobals.CurrentGameState.GameSeed);

    public static int NextInt() =>
        Random.Next();

    public static int NextInt(int max) =>
        Random.Next(max);

    public static int NextInt(int min, int max) =>
        Random.Next(min, max);

    public static int NextIntNormalized(int min, int max, int samples = 10) =>
        Enumerable.Range(0, samples).Select(x => NextInt(min, max)).Sum() / samples;
}
