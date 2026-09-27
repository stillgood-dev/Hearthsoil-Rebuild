using UnityEngine;


// Should the object still exist in this world?
public class PersistentDestroyableObject : MonoBehaviour
{
    [SerializeField] private string objectID;
    public string ObjectID => objectID;

    [SerializeField] private GameObject objectToDisable;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(objectID))
        {
            CheckSavedState();
        }
    }

    public void SetObjectID(string id)
    {
        objectID = id;
        CheckSavedState();  
    }

    private void CheckSavedState()
    {
        // is objectID in RemovedObjects? If yes, remove it
        if (WorldState.RemovedObjects.Contains(objectID))
        {
            objectToDisable.SetActive(false);
        }
    }

    public void MarkDestroyed()
    {
        if (string.IsNullOrEmpty(objectID))
        {
            Debug.LogError("Persistent object is missing an object ID!", this);
            return;
        }

        WorldState.RemovedObjects.Add(objectID);
        objectToDisable.SetActive(false);
    }
}
