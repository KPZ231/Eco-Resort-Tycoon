using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    public float sensitivity = 2f;
    public float smoothTime = 0.1f;

    private Vector3 lastMousePosition;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private bool isDragging = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        SmoothMovement();
    }

    void HandleInput()
    {
        // Sprawdzenie czy zaczyna siê przeci¹ganie
        if (Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }

        // Sprawdzenie czy koñczy siê przeci¹ganie
        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }

        // Obs³uga ruchu podczas przeci¹gania
        if (isDragging)
        {
            HandleDragging();
        }
    }

    void StartDragging()
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition;
    }

    void StopDragging()
    {
        isDragging = false;
    }

    void HandleDragging()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;

        // Normalizacja ruchu myszy na jednostki ekranu
        mouseDelta.x /= Screen.width;
        mouseDelta.y /= Screen.height;

        // Konwersja na ruch w przestrzeni œwiata
        // Dla kamery izometrycznej ruch Y myszy przek³ada siê na ruch Z w œwiecie
        Vector3 movement = new Vector3(
            -mouseDelta.x * sensitivity * 10f,  // Ruch lewo/prawo
            0,                                   // Bez ruchu w górê/dó³
            -mouseDelta.y * sensitivity * 10f   // Ruch myszy góra/dó³ = ruch kamery przód/ty³
        );

        // Dodanie ruchu do pozycji docelowej
        targetPosition += movement;

        lastMousePosition = currentMousePosition;
    }

    void SmoothMovement()
    {
        // P³ynne przejœcie do pozycji docelowej u¿ywaj¹c SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    // Opcjonalne: Metoda do ustawienia granic ruchu kamery
    public void SetBounds(float minX, float maxX, float minZ, float maxZ)
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);
    }
}