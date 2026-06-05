using UnityEngine;

public class PersistentDestroyableObject : MonoBehaviour
{
    [SerializeField] private string objectID;
    [SerializeField] private GameObject objectToDisable;

    private void Awake()
    {
        if (WorldState.RemovedObjects.Contains(objectID))
        {
            objectToDisable.SetActive(false);
        }
    }

    public void MarkDestroyed()
    {
        WorldState.RemovedObjects.Add(objectID);
        objectToDisable.SetActive(false);
    }
}
