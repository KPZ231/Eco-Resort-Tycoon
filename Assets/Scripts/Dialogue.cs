using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueNode[] nodes;
    public string interactionPrompt = "Naciśnij E aby rozmawiać";

    [Header("Detection Settings")]
    public float interactionRange = 3f;
    public LayerMask playerLayerMask = 1;

    private bool playerInRange = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CheckPlayerDistance();

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRange;

        if (playerInRange && !wasInRange)
        {
            ShowInteractionPrompt(true);
        }
        else if (!playerInRange && wasInRange)
        {
            ShowInteractionPrompt(false);
        }
    }

    void ShowInteractionPrompt(bool show)
    {
        if (show)
        {
            Debug.Log(interactionPrompt);
        }
    }

    public void StartDialogue()
    {
        if (DialogueManager.Instance != null && nodes.Length > 0)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;

            DialogueManager.Instance.StartDialogue(this);
        }
    }

    void OnMouseDown()
    {
        StartDialogue();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

