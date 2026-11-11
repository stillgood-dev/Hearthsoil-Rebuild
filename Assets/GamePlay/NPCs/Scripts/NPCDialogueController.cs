using UnityEngine;

public class NPCDialogueController : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueCanvas;
    public bool IsTalking { get; private set; }

    private void Awake()
    {
        if (dialogueCanvas != null) 
            dialogueCanvas.SetActive(false);
    }

    public void StartDialogue()
    {
        IsTalking = true;
        dialogueCanvas.SetActive (true);
        Debug.Log("Started dialogue with" + name);
    }

    public void EndDialogue()
    {
        IsTalking = false;
        dialogueCanvas.SetActive (false);
        Debug.Log("Ended dialogue with" + name);
    }
}
