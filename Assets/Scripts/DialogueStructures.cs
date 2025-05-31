using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [TextArea(3, 5)]
    public string text;
    public DialogueChoice[] choices;
    public bool isEndNode = false;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public int nextNodeIndex;
}