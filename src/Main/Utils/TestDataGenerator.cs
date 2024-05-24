namespace Main.Utils;
internal class TestDataGenerator
{
    public static string[] GetFillerStrings(int numberOfLines)
    {
        var stringList = new List<string>(numberOfLines);
        foreach (var i in Enumerable.Range(0, numberOfLines))
        {
            stringList.Add($"{i,2}" + "1-2-3-4-5-6-7-8-9-1011121314151617181920212223242526272829303132333435363738394041424344454647484950");
        }

        return stringList.ToArray();
    }
}
