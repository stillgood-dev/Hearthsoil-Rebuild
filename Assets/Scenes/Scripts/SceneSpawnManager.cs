using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private PlayerSpawnPoint spawnPoint;

    
    void Start()
    {
        if (player == null || spawnPoint == null) return;

        player.position = spawnPoint.transform.position;
    }

    
}
