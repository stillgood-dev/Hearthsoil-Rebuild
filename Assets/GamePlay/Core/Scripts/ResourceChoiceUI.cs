using UnityEngine;
using TMPro;

public class ResourceChoiceUI : MonoBehaviour
{
    public enum ResourceChoice
    {
        Store,
        Eat,
        Drop
    }

    private ResourceChoice currentChoice;
    public ResourceChoice CurrentChoice => currentChoice;
    
    private bool canEat;
    private bool canDrop;
    private bool canStore;
    public bool IsOpen => choiceRoot.activeSelf;

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
        this.canEat = canEat;
        this.canDrop = canDrop;
        this.canStore = canStore;

        choiceRoot.SetActive(true);

        promptText.text = message;

        eatText.gameObject.SetActive(canEat);
        dropText.gameObject.SetActive(canDrop);
        storeText.gameObject.SetActive(canStore);

        if (canDrop)
        {
            SelectChoice(ResourceChoice.Drop);
        }
        else if (canStore)
        {
            SelectChoice(ResourceChoice.Store);
        }
        else if (canEat)
        {
            SelectChoice(ResourceChoice.Eat);
        }
    }

    public void Hide()
    {
        choiceRoot.SetActive(false);
    }

    private void SelectChoice(ResourceChoice choice)
    {
        currentChoice = choice;

        dropText.text = choice == ResourceChoice.Drop ? "> Drop" : "Drop";
        eatText.text = choice == ResourceChoice.Eat ? "> Eat" : "Eat";
        storeText.text = choice == ResourceChoice.Store ? "> Store" : "Store";
    }

    public void MoveSelection(int direction)
    {
        if (direction > 0)
        {
            // move right
            switch (currentChoice)
            {
                case ResourceChoice.Drop:
                    if (canEat)
                        SelectChoice(ResourceChoice.Eat);
                    else if (canStore)
                        SelectChoice(ResourceChoice.Store);
                    break;

                case ResourceChoice.Eat:
                    if (canStore)
                        SelectChoice(ResourceChoice.Store);
                    else
                        SelectChoice(ResourceChoice.Drop);
                    break;

                case ResourceChoice.Store:
                    SelectChoice(ResourceChoice.Drop);
                    break;
            }
        }
        else if (direction < 0)
        {
            // move left
            switch (currentChoice)
            {
                case ResourceChoice.Drop:
                    if (canStore)
                        SelectChoice(ResourceChoice.Store);
                    else if (canEat)
                        SelectChoice(ResourceChoice.Eat);
                    break;

                case ResourceChoice.Eat:
                    SelectChoice(ResourceChoice.Drop);
                    break;

                case ResourceChoice.Store:
                    if (canEat)
                        SelectChoice(ResourceChoice.Eat);
                    else
                        SelectChoice(ResourceChoice.Drop);
                    break;
            }
        }
    }
}
