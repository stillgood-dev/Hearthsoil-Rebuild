using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    [SerializeField] private string defaultSpawnPointName = "PlayerSpawnPoint";

    void Start()
    {
        Debug.Log("SCENE SPAWN MANAGER STARTED");

        if (PersistentPlayer.Instance == null) return;

        if (string.IsNullOrEmpty(SceneTransitionData.SpawnPointName))
        {
            // No spawn point requested:
            // leave persistent player exactly where they already are.
            return;
        }

        GameObject spawnPoint =
            GameObject.Find(SceneTransitionData.SpawnPointName);

        Debug.Log("Looking for spawn point: " + SceneTransitionData.SpawnPointName);
        Debug.Log("Found spawn point: " + spawnPoint);

        if (spawnPoint == null)
            spawnPoint = GameObject.Find(defaultSpawnPointName);

        if (spawnPoint == null) return;

        Vector3 spawnPosition = spawnPoint.transform.position;
        spawnPosition.x = SceneTransitionData.PreservedX;

        PersistentPlayer.Instance.transform.position = spawnPosition;

        SceneTransitionData.SpawnPointName = null;
    }
}
