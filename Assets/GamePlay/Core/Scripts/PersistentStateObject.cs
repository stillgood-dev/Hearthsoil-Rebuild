using UnityEngine;

public class PersistentStateObject : MonoBehaviour
{
    [SerializeField] private string objectID;

    [Header("State Objects")]
    [SerializeField] private GameObject originalState;
    [SerializeField] private GameObject changedState;

    [Header("Objects Disabled After State Change")]
    [SerializeField] private GameObject[] objectsToDisable;

    private void Awake()
    {
        if (WorldState.ChangedObjects.Contains(objectID))
        {
            ApplyChangedState();
        }
    }

    public void MarkChanged()
    {
        WorldState.ChangedObjects.Add(objectID);
        ApplyChangedState();
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
}
