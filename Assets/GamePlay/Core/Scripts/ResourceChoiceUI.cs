using UnityEngine;
using TMPro;

public class ResourceChoiceUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject choiceRoot;
    [SerializeField] private TMP_Text promptText;

    [Header("Choices")]
    [SerializeField] private TMP_Text eatText;
    [SerializeField] private TMP_Text dropText;
    [SerializeField] private TMP_Text storeText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string message, bool canEat, bool canDrop, bool canStore)
    {
        choiceRoot.SetActive(true);

        promptText.text = message;

        eatText.gameObject.SetActive(canEat);
        dropText.gameObject.SetActive(canDrop);
        storeText.gameObject.SetActive(canStore);
    }

    public void Hide()
    {
        choiceRoot.SetActive(false);
    }
}
