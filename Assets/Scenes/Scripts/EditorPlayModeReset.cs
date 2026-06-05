using UnityEngine;

public class EditorPlayModeReset : MonoBehaviour
{
    private static bool hasResetThisPlaySession = false;

    [SerializeField] private bool resetWorldStateOnFirstStart = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticFlags()
    {
        hasResetThisPlaySession = false;
        SceneTransitionData.SpawnPointName = null;
    }

    private void Awake()
    {
        if (!resetWorldStateOnFirstStart) return;
        if (hasResetThisPlaySession) return;

        WorldState.Reset();
        hasResetThisPlaySession = true;
    }
}
