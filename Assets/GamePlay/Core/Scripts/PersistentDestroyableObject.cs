using UnityEngine;
using UnityEngine.SceneManagement;


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

    // for dropping in another scene
    public void MarkDropped(Vector3 worldPosition)
    {
        if (string.IsNullOrEmpty(objectID))
        {
            Debug.LogError("Persistent object is missing an object ID!", this);
            return;
        }

        WorldState.RelocatedObjects.Add(objectID);
        WorldState.ObjectPositions[objectID] = worldPosition;
        WorldState.ObjectScenes[objectID] = SceneManager.GetActiveScene().name;

        Debug.Log(
            $"{objectID} dropped in {SceneManager.GetActiveScene().name} " +
            $"at {worldPosition}"
        );
    }

    // for carrying from one scene to another
    public void MarkRelocated()
    {
        if (string.IsNullOrEmpty(objectID))
        {
            Debug.LogError("Persistent object is missing an object ID!", this);
            return;
        }

        WorldState.RelocatedObjects.Add(objectID);
        WorldState.ObjectPositions.Remove(objectID);
        WorldState.ObjectScenes.Remove(objectID);
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
