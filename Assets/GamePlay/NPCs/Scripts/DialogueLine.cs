using System;
using UnityEngine;

[Serializable] // tells Unity it can display and save instances in the Inspector
public class DialogueLine
{
    public bool isPlayer;
    [TextArea(2, 5)] // makes dialogue field a multiline text box for longer conversations
    public string text;
}
