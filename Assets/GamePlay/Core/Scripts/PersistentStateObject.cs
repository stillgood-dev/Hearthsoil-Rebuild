using UnityEngine;

public enum PersistenceType
{
    None,
    Persist,
    Regrow
}

// What version of this object should exist??
public class PersistentStateObject : MonoBehaviour
{
    [SerializeField] private string objectID;

    [Header("Persistence")]
    [SerializeField] private PersistenceType persistence = PersistenceType.Persist;
    public PersistenceType Persistence => persistence;

    [Header("State Objects")]
    [SerializeField] private GameObject originalState;
    [SerializeField] private GameObject changedState;

    [Header("Objects Disabled After State Change")]
    [SerializeField] private GameObject[] objectsToDisable;

    [Header("Regrow Parameters")]
    [SerializeField] private int daysToRegrow;
    [Tooltip("Testing")]
    [SerializeField] private int currentDay;
    [SerializeField] private GameObject[] objectsToRefresh;

    [Header("Debug/Testing")]
    [SerializeField] private bool testRegrowth;
    [SerializeField] private float testRegrowDelay = 2f;

    
    public bool HasChanged()
    {
        switch(persistence)
        {
            case PersistenceType.None:
                return false;
            
            case PersistenceType.Persist:
                return WorldState.ChangedObjects.Contains(objectID);
                

            case PersistenceType.Regrow:
                return WorldState.RegrowDays.ContainsKey(objectID);

            default:
                return false;
                
        }
    }
    

    private void Awake()
    {
        switch (persistence)
        {
            case PersistenceType.None:
                break;

            case PersistenceType.Persist:
                if (WorldState.ChangedObjects.Contains(objectID))
                {
                    ApplyChangedState();
                }
                break;

            case PersistenceType.Regrow:
                Regrow();
                break;
        }
    }

    public void MarkChanged()
    {
        WorldState.ChangedObjects.Add(objectID);
        ApplyChangedState();
    }

    public void MarkForRegrowth()
    {
        int regrowDay = currentDay + daysToRegrow;
        WorldState.RegrowDays[objectID] = regrowDay;
        ApplyChangedState();

        if (testRegrowth)
        {
            Invoke(nameof(TestRegrow), testRegrowDelay);
        }
    }


    private void ApplyChangedState()
    {
        if(originalState != null) originalState.SetActive(false);

        if(changedState != null) changedState.SetActive(true);

        if(objectsToDisable != null)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if(obj != null) obj.SetActive(false);
            }
        }
    }

    private void Regrow()
    {
        if (WorldState.RegrowDays.TryGetValue(objectID, out int regrowDay))
        {
            if (currentDay >= regrowDay)
            {
                RefreshState();
            }
            else
            {
                // keep damaged state active until regrow day
                ApplyChangedState();
            }
        }
    }

    private void RefreshState()
    {
        if (objectsToRefresh != null)
        {
            foreach (GameObject obj in objectsToRefresh)
            {
                if (obj != null) obj.SetActive(true);
            }
        }
        WorldState.RegrowDays.Remove(objectID);
    }

    private void TestRegrow()
    {
        currentDay += daysToRegrow;
        Regrow();
    }
}
