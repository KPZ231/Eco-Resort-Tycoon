using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Transform choicesParent;
    public GameObject choiceButtonPrefab;

    private Dialogue currentDialogue;
    private int currentNodeIndex = 0;
    private List<GameObject> currentChoiceButtons = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CloseDialogue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNodeIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayCurrentNode();
    }

    void DisplayCurrentNode()
    {
        if (currentDialogue == null || currentNodeIndex >= currentDialogue.nodes.Length)
        {
            CloseDialogue();
            return;
        }

        DialogueNode currentNode = currentDialogue.nodes[currentNodeIndex];
        dialogueText.text = currentNode.text;

        ClearChoiceButtons();

        if (currentNode.isEndNode)
        {
            CreateChoiceButton("Zakończ", () => CloseDialogue());
        }
        else if (currentNode.choices != null && currentNode.choices.Length > 0)
        {
            foreach (DialogueChoice choice in currentNode.choices)
            {
                int nextIndex = choice.nextNodeIndex;
                CreateChoiceButton(choice.choiceText, () => SelectChoice(nextIndex));
            }
        }
        else
        {
            CreateChoiceButton("Kontynuuj", () => SelectChoice(currentNodeIndex + 1));
        }
    }

    void CreateChoiceButton(string text, System.Action onClickAction)
    {
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesParent);
        Button button = buttonObj.GetComponent<Button>();
        Text buttonText = buttonObj.GetComponentInChildren<Text>();

        buttonText.text = text;
        button.onClick.AddListener(() => onClickAction());

        currentChoiceButtons.Add(buttonObj);
    }

    public void SelectChoice(int nextNodeIndex)
    {
        currentNodeIndex = nextNodeIndex;
        DisplayCurrentNode();
    }

    void ClearChoiceButtons()
    {
        foreach (GameObject button in currentChoiceButtons)
        {
            Destroy(button);
        }
        currentChoiceButtons.Clear();
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
        ClearChoiceButtons();

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
