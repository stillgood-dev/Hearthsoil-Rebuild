using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject notificationRoot;
    [SerializeField] private TMP_Text notificationText;
    private bool isOpen;
    public bool IsOpen => isOpen;

    private void Awake()
    {
        Hide();
    }

    public void Show(string message)
    {
        isOpen = true;
        notificationRoot.SetActive(true);
        notificationText.text = message;
    }

    public void Hide()
    {
        isOpen = false;
        notificationRoot.SetActive(false);
    }
}