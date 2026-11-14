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
    [SerializeField] private string npcGreeting; // what to start conversation with
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    // reveal one character every 0.04 seconds
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue")]
    [SerializeField] private string playerResponse = "Um... do I know you?";

    [Header("Debug")]
    [SerializeField] private bool debug;

    // for playerInteractionController reference
    public bool IsTalking { get; private set; }
    public bool IsTyping => typingCoroutine != null; // let playerInteractionController know the typing coroutine is running
    public bool IsPlayerTurn => isPlayerTurn;

    // track what our coroutine is doing, if it's not null it's running, if it's null it's not running
    private Coroutine typingCoroutine;
    // beginning of conversation is never player turn
    private bool isPlayerTurn = false;

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

        if (debug)
            Debug.Log("Current speaker: " + speakerName);
    }

    // public method for when dialogue begins
    public void StartDialogue()
    {
        IsTalking = true; // conversation is open
        dialogueCanvas.SetActive (true); // canvas is open
        isPlayerTurn = false; // every new conversation begins with NPC
        DisplayDialogue(npcName, npcGreeting); // start convo with NPC greeting

        if (debug)
        {
            Debug.Log("Started dialogue with" + name);
            Debug.Log("Dialogue text found: " + npcGreeting);
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
            Debug.Log("Displaying dialogue: " + speakerName + ": " + dialogue);

    }

    // this is called in PlayerInteractionController if the NPC is not currently typing
    public void AdvanceDialogue()
    {
        // if it is currently the NPC's turn
        if (!isPlayerTurn)
        {
            // switch the conversation to the player
            isPlayerTurn = true;

            // show player reponse
            DisplayDialogue(playerName, playerResponse);

            if (debug)
                Debug.Log("Player response started!");
        }
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

        if (debug)
        {
            Debug.Log("Ended dialogue with" + name);
        }
    }

    // method for showing all characters if the NPC/player is talking to speed up conversation
    public void CompleteDialogue()
    {
        // stop typing
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // show all the characters at once
        dialogueText.maxVisibleCharacters = int.MaxValue; // tell TextMeshPro to show every character

        if (debug) Debug.Log("Dialogue completed!");
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

        if (debug) Debug.Log("Finished typing dialogue!");
    }
}
