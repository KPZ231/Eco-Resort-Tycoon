using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public static LevelUpSystem Instance { get; private set; }

    [Header("Level System Settings")]
    [SerializeField] private float baseXP = 100f;
    [SerializeField] private float scalingFactor = 1.5f;

    void Start()
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

    /// <summary>
    /// Oblicza ile XP potrzeba do nastêpnego poziomu
    /// </summary>
    public int GetXPForNextLevel(int currentLevel)
    {
        return Mathf.RoundToInt(baseXP * Mathf.Pow(currentLevel, scalingFactor));
    }

    /// <summary>
    /// Leveluje postaæ o 1 poziom wy¿ej
    /// </summary>
    public void LevelUp()
    {
        PlayerStats.Instance.SetLevel(PlayerStats.Instance.GetLevel() + 1);
        PlayerStats.Instance.SetXP(0);
        Debug.Log($"Level Up! Nowy poziom: {PlayerStats.Instance.GetLevel()}");
    }
}