using System.Collections.Generic;
using UnityEngine;

public static class WorldState
{
    public static HashSet<string> RemovedObjects = new HashSet<string>();
    public static HashSet<string> ChangedObjects = new HashSet<string>();
    // object still exists, but no longer belongs at its original spawn point
    public static HashSet<string> RelocatedObjects = new HashSet<string>();

    public static Dictionary<string, int> RegrowDays = new Dictionary<string, int>();
    public static Dictionary<string, Vector3> ObjectPositions = new Dictionary<string, Vector3>();
    public static Dictionary<string, string> ObjectScenes = new Dictionary<string, string>();

    // only for restarting the game or returning to title screen
    public static void Reset()
    {
        RemovedObjects.Clear();
        ChangedObjects.Clear();
        RelocatedObjects.Clear();   

        RegrowDays.Clear();
        ObjectPositions.Clear();
        ObjectScenes.Clear();

    }
}