// ===== Dialogue.cs =====
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueNode[] nodes;

    [Header("Visual Feedback")]
    public bool showHighlightOnHover = true;
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    private Renderer objectRenderer;
    private Color originalColor;
    private bool isHighlighted = false;

    void Start()
    {
        // Pobierz renderer dla efektu podświetlenia
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // Upewnij się, że obiekt ma Collider dla wykrywania kliknięć
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"Obiekt {gameObject.name} z Dialogue nie ma Collider! Dodaj Collider aby umożliwić kliknięcia.");
        }
    }

    // Główna metoda - wywołanie dialogu po kliknięciu
    void OnMouseDown()
    {
        Debug.Log($"Kliknięto na obiekt: {gameObject.name}");
        StartDialogue();
    }

    // Opcjonalne podświetlenie przy najechaniu kursorem
    void OnMouseEnter()
    {
        if (showHighlightOnHover && objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = originalColor * highlightIntensity;
            isHighlighted = true;
        }
    }

    void OnMouseExit()
    {
        if (showHighlightOnHover && objectRenderer != null && isHighlighted)
        {
            objectRenderer.material.color = originalColor;
            isHighlighted = false;
        }
    }

    public void StartDialogue()
    {
        Debug.Log($"Próba rozpoczęcia dialogu na obiekcie: {gameObject.name}");
        Debug.Log($"DialogueManager.Instance: {(DialogueManager.Instance != null ? "OK" : "NULL")}");
        Debug.Log($"Liczba węzłów: {nodes.Length}");

        if (DialogueManager.Instance != null && nodes.Length > 0)
        {
            Debug.Log("Rozpoczynam dialog!");
            // Pokaż kursor i zatrzymaj czas
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            DialogueManager.Instance.StartDialogue(this);
        }
        else
        {
            Debug.LogWarning($"Nie można rozpocząć dialogu: {(DialogueManager.Instance == null ? "Brak DialogueManager" : "Brak węzłów dialogu")}");
        }
    }

    // Opcjonalna metoda do wywołania dialogu z kodu
    public void TriggerDialogue()
    {
        StartDialogue();
    }
}