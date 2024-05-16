namespace Main;

public static class Helpers
{
    public static void RunMethodManyTimes(Action action, int times)
    {
        for (int i = 0; i < times; i++)
        {
            action.Invoke();
        }
    }
}
