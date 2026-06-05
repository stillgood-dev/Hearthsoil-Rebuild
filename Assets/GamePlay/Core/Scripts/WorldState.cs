using System.Collections.Generic;

public static class WorldState
{
    public static HashSet<string> RemovedObjects = new HashSet<string>();

    public static void Reset()
    {
        RemovedObjects.Clear();
    }
}