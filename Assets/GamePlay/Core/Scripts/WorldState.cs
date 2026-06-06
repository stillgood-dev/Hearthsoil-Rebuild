using System.Collections.Generic;

public static class WorldState
{
    public static HashSet<string> RemovedObjects = new HashSet<string>();

    public static Dictionary<string, int> RegrowDays = new Dictionary<string, int>();

    public static void Reset()
    {
        RemovedObjects.Clear();
        RegrowDays.Clear();
    }
}