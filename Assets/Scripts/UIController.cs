using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Player Stats UI Components")]
    [SerializeField] private TextMeshProUGUI moneyText = null;
    [SerializeField] private Slider xpSlider = null;
    [SerializeField] private TextMeshProUGUI levelText = null;
    [SerializeField] private TextMeshProUGUI rebirthsText = null;
    [SerializeField] private GameObject levelUpPanel = null;

    [Header("Shop")]
    public GameObject shopPanel = null;



    //Shopkeeper
    [HideInInspector] public bool isShopPanelOpen = false;

    //Update interval for UI updates
    private float updateInterval = 0.1f;
    private float lastUpdateTime;
    private PlayerStats playerStatsCache;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayerStats();
            lastUpdateTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePlayerStats();
    }

    private void InitializePlayerStats()
    {
        if (playerStatsCache == null)
        {
            playerStatsCache = PlayerStats.Instance;
        }
    }

    public void ShowShopPanel()
    {
        if (isShopPanelOpen)
        {
            shopPanel.SetActive(false);
            isShopPanelOpen = true;
        }
        else
        {
            shopPanel.SetActive(true);
            isShopPanelOpen = false;
        }

    }
    public async void UpdateUI()
    {
        await System.Threading.Tasks.Task.Yield();
        InitializePlayerStats();

        if (playerStatsCache == null)
        {
            Debug.LogWarning("PlayerStats instance is null!");
            return;
        }

        if (moneyText != null)
            moneyText.text = $"Money: {playerStatsCache.GetMoney()}";

        if (xpSlider != null)
        {
            // Ustaw maxValue slidera na XP potrzebne do nastêpnego poziomu
            xpSlider.maxValue = LevelUpSystem.Instance.GetXPForNextLevel(playerStatsCache.GetLevel());
            xpSlider.value = playerStatsCache.GetXP();
        }

        if (levelText != null)
            levelText.text = $"Level: {playerStatsCache.GetLevel()}";

        if (rebirthsText != null)
            rebirthsText.text = $"Rebirths: {playerStatsCache.GetRebirths()}";
    }

    private void Update()
    {
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateUI();
            lastUpdateTime = Time.time;

            // SprawdŸ czy mo¿na levelowaæ
            if (xpSlider.value >= xpSlider.maxValue)
            {
                levelUpPanel.SetActive(true);
                LevelUpSystem.Instance.LevelUp();
            }
        }
    }
}