using UnityEngine;

public class SceneEnvironmentSettings : MonoBehaviour
{
    [SerializeField] private PlayerEnvironment environment = PlayerEnvironment.Outside;

    private void Start()
    {
        if (PersistentPlayer.Instance == null) return;

        PlayerEnvironmentState playerEnvironment =
            PersistentPlayer.Instance.GetComponent<PlayerEnvironmentState>();

        if (playerEnvironment == null) return; 

        playerEnvironment.SetPlayerEnvironment(environment);
    }

}
