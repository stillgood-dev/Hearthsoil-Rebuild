using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    [SerializeField] private string defaultSpawnPointName = "PlayerSpawnPoint";
    [SerializeField] private bool showDebug = false;

    void Start()
    {
       if(showDebug) Debug.Log("SCENE SPAWN MANAGER STARTED");

        if (PersistentPlayer.Instance == null) return;

        if (string.IsNullOrEmpty(SceneTransitionData.SpawnPointName))
        {
            // No spawn point requested:
            // leave persistent player exactly where they already are.
            return;
        }

        // find the spawn point from SceneTransitionData.cs
        GameObject spawnPoint =
            GameObject.Find(SceneTransitionData.SpawnPointName);

        if (showDebug)
        {
            Debug.Log("Looking for spawn point: " + SceneTransitionData.SpawnPointName);
            Debug.Log("Found spawn point: " + spawnPoint);
        }

        // spawn at default if no other spawn point provided
        if (spawnPoint == null)
            spawnPoint = GameObject.Find(defaultSpawnPointName);

        if (spawnPoint == null) return;

        Vector3 spawnPosition = spawnPoint.transform.position;

        if (SceneTransitionData.PreservePlayerX)
        {
            spawnPosition.x = SceneTransitionData.PreservedX;
        }

        // put the persistent player at the spawn point
        PersistentPlayer.Instance.transform.position = spawnPosition;

        Physics2D.SyncTransforms();

        if (showDebug)
        {
            Rigidbody2D rb = PersistentPlayer.Instance.GetComponent<Rigidbody2D>();
            Debug.Log($"SPAWN TARGET: {spawnPoint.transform.position}");
            Debug.Log($"PLAYER TRANSFORM: {PersistentPlayer.Instance.transform.position}");
            Debug.Log($"PLAYER RIGIDBODY: {rb.position}");
        }

        SceneTransitionData.SpawnPointName = null;
        SceneTransitionData.PreservePlayerX = false;
    }
}
