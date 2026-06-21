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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showDebug)
            Debug.Log("Something entered transition trigger: " + other.name);

        if (hasTriggered) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player == null)
        {
            if (showDebug)
                Debug.Log("Object did not have PlayerController in parent.");

            return;
        }

        hasTriggered = true;

        if (showDebug)
            Debug.Log("Transitioning to scene: " + sceneToLoad);

        SceneTransitionData.SpawnPointName = spawnPointName;

        SceneManager.LoadScene(sceneToLoad);
    }
}