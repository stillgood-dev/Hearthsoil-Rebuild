using UnityEngine;
using UnityEngine.SceneManagement;

public class RelocatedResourceRestorer : MonoBehaviour
{

    [SerializeField] private GameObject resourcePrefab;
    [SerializeField] private bool showDebug = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RestoreLocatedResources();
    }

    private void RestoreLocatedResources()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        foreach (string objectID in WorldState.RelocatedObjects)
        {
            // object doesn't currently have a saved scene
            if (!WorldState.ObjectScenes.ContainsKey(objectID))
                continue;

            // object belongs in a different scene
            if (WorldState.ObjectScenes[objectID] != currentScene)
                continue;

            // object doesn't currently have a saved position
            if (!WorldState.ObjectPositions.ContainsKey(objectID))
                continue;

            // otherwise, get world position of object
            Vector3 position = WorldState.ObjectPositions[objectID];

            // instantiate the object at it's world position
            GameObject restoredResource = Instantiate(
                resourcePrefab,
                position,
                Quaternion.identity

            );

            // Get object persistence
            PersistentDestroyableObject persistentObject = 
                restoredResource.GetComponent<PersistentDestroyableObject>();

            if(persistentObject != null ) persistentObject.SetObjectID(objectID);

            if (showDebug)
            {
                Debug.Log(
                    $"Restored relocated resource {objectID}" +
                    $"in {currentScene} at {position}"
                );
            }
        }
    }
}
