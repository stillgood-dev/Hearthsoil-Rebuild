using UnityEngine;

// Ensure there is only one player object and keep that player alive when the scene changes
public class PersistentPlayer : MonoBehaviour
{
    // shared reference to current PersistentPlayer
    // because it's static, every script can refer to the same one with PersistentPlayer.Instance
    public static PersistentPlayer Instance;

    private void Awake()
    {

        // if there is another gameObject in the scene with PersistentPlayer and it's not me, destroy it
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // if you are the persistent player, do not destroy and recreate between scenes
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
