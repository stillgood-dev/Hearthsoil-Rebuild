using UnityEngine;
using TMPro; // for type-writer effects
using System.Collections; // for coroutines    

public class NPCDialogueController : MonoBehaviour
{

    [Header("NPC Info")]
    [SerializeField] private string npcName;
    [SerializeField] private string playerName = "Kansas";

    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI npcHeader;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    // reveal one character every 0.04 seconds
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue")] 
    [SerializeField] private DialogueLine[] dialogueLines;
    [SerializeField] private int currentDialogueIndex = 0; // arrays start at index 0, not 1!
    [SerializeField] private bool conversationOver = false;


    [Header("Debug")]
    [SerializeField] private bool debug;

    // for playerInteractionController reference
    public bool IsTalking { get; private set; }
    public bool IsTyping => typingCoroutine != null; // let playerInteractionController know the typing coroutine is running

    // track what our coroutine is doing, if it's not null it's running, if it's null it's not running
    private Coroutine typingCoroutine;

    // don't show dialogue canvas until player interacts with NPC
    private void Awake()
    {
        if (dialogueCanvas != null) 
            dialogueCanvas.SetActive(false);
    }

    // public method for setting the name of the speaker to the npc header
    public void SetSpeaker(string speakerName)
    {
        npcHeader.text = speakerName;

    }

    // public method for when dialogue begins
    public void StartDialogue()
    {
        IsTalking = true; // conversation is open
        dialogueCanvas.SetActive (true); // canvas is open
        currentDialogueIndex = 0; // must be 0 when starting a conversation

        if (debug)
            Debug.Log("Current dialogue index: " + currentDialogueIndex);

        DialogueLine currentLine = dialogueLines[currentDialogueIndex];
        string speakerName = currentLine.isPlayer ? playerName : npcName;

        DisplayDialogue(speakerName, currentLine.text); // start convo with NPC greeting (first line in convo)

        if (debug)
        {
            Debug.Log("Started dialogue with" + name);
        }
    }

    // public method for how the dialogue will be displayed when it's running
    public void DisplayDialogue(string speakerName, string dialogue)
    {
        // set the current speaker
        SetSpeaker(speakerName);

        // update the dialogue text
        dialogueText.text = dialogue;

        // Prepare the typewriter effect
        dialogueText.ForceMeshUpdate(); // tell TextMeshPro to update text info immediately
        dialogueText.maxVisibleCharacters = 0; // no characters visible at first

        // Start revealing characters, coroutine != null
        typingCoroutine = StartCoroutine(TypeDialogue());

        if (debug)
            Debug.Log("Displaying dialogue [" + currentDialogueIndex + "]: " + speakerName + ": " + dialogue);

    }

    // this is called in PlayerInteractionController if the NPC is not currently typing
    public void AdvanceDialogue()
    {
       // Move to the next line in the conversation
       currentDialogueIndex++; // i.e., increment by 1 each time

        // check whether we've reached the end of the conversation
        if (currentDialogueIndex >= dialogueLines.Length)
        {
            // end conversation if so
            EndDialogue();
            return;
        }

        // Otherwise
        // Get current dialogue line
        DialogueLine currentLine = dialogueLines[currentDialogueIndex];

        // Determine who is speaking
        string speakerName = currentLine.isPlayer? playerName : npcName;

        // Display dialogue
        DisplayDialogue(speakerName, currentLine.text);
    }

    public void EndDialogue()
    {
        // stop the typewriter if it's still running
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        IsTalking = false; // close the conversation
        dialogueCanvas.SetActive (false); // close the dialogue canvas
        conversationOver = true;
        WorldState.SatchelUnlocked = true;

        if (debug)
        {
            Debug.Log("Conversation Over:" + conversationOver);
            Debug.Log("Ended dialogue with" + name);
        }
    }

    // method for showing all characters if the NPC/player is talking to speed up conversation
    public void CompleteDialogueLines()
    {
        // stop typing
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // show all the characters at once
        dialogueText.maxVisibleCharacters = int.MaxValue; // tell TextMeshPro to show every character

    }

    // coroutine to reveal letters as NPC talks
    private IEnumerator TypeDialogue()
    {
        int totalCharacters = dialogueText.textInfo.characterCount;

        for(int i = 1; // starting value, this is our counter for how many characters we want to show
            i <= totalCharacters; // keep looping as long as i <= totalCharacters
            i++) // increment by 1 (every time the loop completes an interaction, i increases by 1)
        {
            dialogueText.maxVisibleCharacters = i;

            // pauses the coroutine during interactions
            // so player sees characters appearing rather than
            // all at once
            yield return new WaitForSeconds(typingSpeed); 
        }

        // for R comparison, this is the same as
        // for (i in 1:totalCharacters)
        // {
        // print(i)
        //}

        // End coroutine
        // Clear our reference because it doesn't automatically
        // set to null when the coroutine finishes
        typingCoroutine = null;

    }
}
