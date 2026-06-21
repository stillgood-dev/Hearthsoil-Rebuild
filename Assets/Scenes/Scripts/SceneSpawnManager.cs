using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    [SerializeField] private string defaultSpawnPointName = "PlayerSpawnPoint";

    void Start()
    {
        if (PersistentPlayer.Instance == null) return;

        string spawnName = string.IsNullOrEmpty(SceneTransitionData.SpawnPointName)
            ? defaultSpawnPointName
            : SceneTransitionData.SpawnPointName;

        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint == null)
            spawnPoint = GameObject.Find(defaultSpawnPointName);

        if (spawnPoint == null) return;

        PersistentPlayer.Instance.transform.position = spawnPoint.transform.position;

        SceneTransitionData.SpawnPointName = null;
    }
}
