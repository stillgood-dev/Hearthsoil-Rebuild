using System.Collections.Generic;
using UnityEngine;

public static class WorldState
{
    public static HashSet<string> RemovedObjects = new HashSet<string>();
    public static HashSet<string> ChangedObjects = new HashSet<string>();

    public static Dictionary<string, int> RegrowDays = new Dictionary<string, int>();
    public static Dictionary<string, Vector3> ObjectPositions = new Dictionary<string, Vector3>();

    // only for restarting the game or returning to title screen
    public static void Reset()
    {
        RemovedObjects.Clear();
        RegrowDays.Clear();
        ChangedObjects.Clear();
        ObjectPositions.Clear();

    }
}