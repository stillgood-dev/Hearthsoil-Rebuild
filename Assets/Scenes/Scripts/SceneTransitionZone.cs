using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionZone : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string spawnPointName = "PlayerSpawnPoint";

    [Header("Debug")]
    [SerializeField] private bool showDebug;

    private bool hasTriggered;

    [SerializeField] private bool preservePlayerPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null) return;

        hasTriggered = true;

        if (preservePlayerPosition)
        {
            SceneTransitionData.PreservedX = player.transform.position.x;
            SceneTransitionData.SpawnPointName = spawnPointName;
        }
        else
        {
            SceneTransitionData.SpawnPointName = spawnPointName;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}