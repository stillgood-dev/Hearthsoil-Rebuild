using UnityEngine;
using Unity.Cinemachine;

public class CinemachineFollowPersistentPlayer : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        if (PersistentPlayer.Instance == null) return;

        cinemachineCamera.Follow = PersistentPlayer.Instance.transform;
    }
}
