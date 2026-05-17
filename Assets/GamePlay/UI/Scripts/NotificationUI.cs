using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject notificationRoot;
    [SerializeField] private TMP_Text notificationText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string message)
    {
        notificationRoot.SetActive(true);
        notificationText.text = message;
    }

    public void Hide()
    {
        notificationRoot.SetActive(false);
    }
}