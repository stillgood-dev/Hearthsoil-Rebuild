using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Start()
    {
        if (player == null) return;

        GameObject spawnPoint = GameObject.Find(SceneTransitionData.SpawnPointName);

        if (spawnPoint == null) return;

        player.position = spawnPoint.transform.position;
    }
}
