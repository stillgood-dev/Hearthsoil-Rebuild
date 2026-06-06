using UnityEngine;

public class TreeRegrowthController : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string objectID;

    [Header("Regrow Settings")]
    [SerializeField] private int daysToRegrow = 5;

    [Header("Object Stages")]
    [SerializeField] private GameObject standingTree;
    [SerializeField] private GameObject felledLogEast;
    [SerializeField] private GameObject felledLogWest;

    [Header("Object Controllers")]
    [SerializeField] ChoppableTreeController treeController;
    [SerializeField] FelledLogController logControllerEast;
    [SerializeField] FelledLogController logControllerWest;

    [Header("Debug / Temporary")]
    [SerializeField] private int currentDayForTesting;

    private void Awake()
    {
        RefreshState();
    }

    public void ShowStandingTree()
    {
        standingTree?.SetActive(true);

        felledLogEast?.SetActive(false);
        felledLogWest?.SetActive(false);
    }

    public void ShowFelledLog(GameObject chosenFelledTree)
    {
        standingTree?.SetActive(false);

        felledLogEast?.SetActive(false);
        felledLogWest?.SetActive(false);

        chosenFelledTree?.SetActive(true);
    }

    public void MarkDepleted()
    {
        int regrowDay = currentDayForTesting + daysToRegrow;
        WorldState.RegrowDays[objectID] = regrowDay;

        ShowDepleted();
    }

    private void RefreshState()
    {
        if (!WorldState.RegrowDays.TryGetValue(objectID, out int regrowDay))
        {
            treeController?.RefreshStandingTree();
            logControllerEast?.RefreshLog();
            logControllerWest?.RefreshLog();
            return;
        }

        if (currentDayForTesting >= regrowDay)
        {
            WorldState.RegrowDays.Remove(objectID);
            treeController?.RefreshStandingTree();
            logControllerEast?.RefreshLog();   
            logControllerWest?.RefreshLog();
            return;
        }

        ShowDepleted();
    }

    public void ShowDepleted()
    {
        standingTree?.SetActive(false);
        felledLogEast?.SetActive(false);
        felledLogWest?.SetActive(false);
    }


    [ContextMenu("Debug Advance To Regrow Day")]
    private void DebugAdvanceToRegrowDay()
    {
        if (!WorldState.RegrowDays.TryGetValue(objectID, out int regrowDay))
        {
            Debug.Log("No regrow day stored for " + objectID, this);
            return;
        }

        currentDayForTesting = regrowDay;
        RefreshState();
    }
}
