using UnityEngine;

public class HearthsoilSign : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private GameObject signUI;

    [Header("Debug")]
    [SerializeField] private bool isOpen;

    public bool IsOpen => isOpen;

    public void Open()
    {
        signUI.SetActive(true);
        isOpen = true;
    }

    public void Close()
    {
        signUI.SetActive(false);
        isOpen = false;
    }
}