using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int money = 1000; // Starting money
    public int xp = 0; // Starting XP
    public int level = 1; // Starting level
    public int rebirths = 0; // Number of rebirths

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        LoadStats(); // Load stats when the game starts
        PrintStats(); // Print initial stats for debugging
    }

    public int SetMoney(int amount)
    {
        money = amount;
        return money;
    }

    public int GetMoney()
    {
        return money;
    }

    public int SetXP(int amount)
    {
        xp = amount;
        return xp;
    }
    public int GetXP()
    {
        return xp;
    }

    public int SetLevel(int amount)
    {
        level = amount;
        return level;
    }
    public int GetLevel()
    {
        return level;
    }

    public int SetRebirths(int amount)
    {
        rebirths = amount;
        return rebirths;
    }

    public int GetRebirths()
    {
        return rebirths;
    }

    public void ResetStats()
    {
        money = 1000;
        xp = 0;
        level = 1;
        rebirths = 0;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void AddXP(int amount)
    {
        xp += amount;
    }

    public void AddRebirths(int amount)
    {
        rebirths += amount;
    }

    public void AddLevel(int amount)
    {
        level += amount;
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        if (money < 0) money = 0; // Prevent negative money
    }

    public void RemoveXP(int amount)
    {
        xp -= amount;
        if (xp < 0) xp = 0; // Prevent negative XP
    }

    public void RemoveRebirths(int amount)
    {
        rebirths -= amount;
        if (rebirths < 0) rebirths = 0; // Prevent negative rebirths
    }
    public void RemoveLevel(int amount)
    {
        level -= amount;
        if (level < 1) level = 1; // Prevent level from going below 1
    }
    public void PrintStats()
    {
        Debug.Log($"Money: {money}, XP: {xp}, Level: {level}, Rebirths: {rebirths}");
    }
    public void SaveStats()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Rebirths", rebirths);
        PlayerPrefs.Save();
    }
    public void LoadStats()
    {
        money = PlayerPrefs.GetInt("Money", 1000); // Default to 1000 if not set
        xp = PlayerPrefs.GetInt("XP", 0); // Default to 0 if not set
        level = PlayerPrefs.GetInt("Level", 1); // Default to 1 if not set
        rebirths = PlayerPrefs.GetInt("Rebirths", 0); // Default to 0 if not set
    }
    private void OnApplicationQuit()
    {
        SaveStats(); // Save stats when the application quits
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveStats(); // Save stats when the application is paused
        }

    }
    private void OnEnable()
    {
        LoadStats(); // Load stats when the script is enabled
    }

    private void OnDisable()
    {
        SaveStats(); // Save stats when the script is disabled
    }
    private void OnDestroy()
    {
        SaveStats(); // Save stats when the script is destroyed

    }
}
