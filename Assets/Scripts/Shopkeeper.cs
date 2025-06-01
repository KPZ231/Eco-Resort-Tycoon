using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shopkeeper : MonoBehaviour
{
    [Header("Shopkeeper Configuration")]
    public ShopItem[] shopItems; // Array of items available in the shop
    public int shopkeeperID; // Unique identifier for the shopkeeper
    public string shopkeeperName; // Name of the shopkeeper
    public string shopkeeperDescription; // Description of the shopkeeper
    public Sprite shopkeeperIcon; // Icon representing the shopkeeper
    public bool isShopOpen; // Flag to indicate if the shop is currently open

    [Header("UI")]
    [SerializeField] private GameObject shopItemPrefab = null; // Prefab for shop items

    private Color originalColor;
    private MeshRenderer meshRenderer;
    private List<GameObject> currentShopItems = new List<GameObject>(); // Track created shop items
    private bool isShopPanelOpen = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
    }

    private void OnMouseEnter()
    {
        if (meshRenderer == null) return;

        meshRenderer.material.color = isShopOpen ? Color.green : Color.red;
    }

    private void OnMouseExit()
    {
        if (meshRenderer == null) return;

        meshRenderer.material.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (isShopOpen)
        {
            StartShopConvo();
        }
    }

    public void StartShopConvo()
    {
        if (!isShopPanelOpen)
        {
            UIController.Instance.ShowShopPanel();
            DrawItems();
            isShopPanelOpen = true;
        }
    }

    public void CloseShop()
    {
        ClearShopItems();
        isShopPanelOpen = false;
    }

    private void DrawItems()
    {
        // Clear existing items first
        ClearShopItems();

        if (shopItems == null || shopItemPrefab == null)
        {
            Debug.LogError("Shop items or prefab not assigned!");
            return;
        }

        foreach (ShopItem item in shopItems)
        {
            if (item == null) continue;

            GameObject itemGO = Instantiate(shopItemPrefab, UIController.Instance.shopPanel.transform);
            currentShopItems.Add(itemGO); // Track the created item

            SetupItemUI(itemGO, item);
        }
    }

    private void SetupItemUI(GameObject itemGO, ShopItem item)
    {
        // Safely set UI elements with null checks
        SetTextComponent(itemGO, 0, item.itemName);
        SetTextComponent(itemGO, 1, item.itemDescription);
        SetTextComponent(itemGO, 2, item.itemPrice.ToString());
        SetTextComponent(itemGO, 3, item.itemQuantity.ToString());
        SetTextComponent(itemGO, 4, item.maxQuantity.ToString());
        SetTextComponent(itemGO, 5, item.levelRequirement.ToString());
        SetTextComponent(itemGO, 6, item.isAvailable ? "Available" : "Unavailable");

        // Set item icon
        if (itemGO.transform.childCount > 7)
        {
            Image iconImage = itemGO.transform.GetChild(7).GetComponent<Image>();
            if (iconImage != null && item.itemIcon != null)
            {
                iconImage.sprite = item.itemIcon;
            }
        }

        // Setup purchase button
        SetupPurchaseButton(itemGO, item);
    }

    private void SetTextComponent(GameObject parent, int childIndex, string text)
    {
        if (parent.transform.childCount > childIndex)
        {
            TextMeshProUGUI textComponent = parent.transform.GetChild(childIndex).GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        }
    }

    private void SetupPurchaseButton(GameObject itemGO, ShopItem item)
    {
        if (itemGO.transform.childCount > 8)
        {
            Button purchaseButton = itemGO.transform.GetChild(8).GetComponent<Button>();
            if (purchaseButton != null)
            {
                // Clear existing listeners to prevent duplicates
                purchaseButton.onClick.RemoveAllListeners();

                bool canPurchase = CanPurchaseItem(item);
                purchaseButton.interactable = canPurchase;

                if (canPurchase)
                {
                    purchaseButton.onClick.AddListener(() => PurchaseItem(item));
                }
            }
        }
    }

    private bool CanPurchaseItem(ShopItem item)
    {
        if (PlayerStats.Instance == null) return false;

        // Check conditions for purchasing (no isPurchased check - can buy multiple times)
        bool hasEnoughLevel = PlayerStats.Instance.GetLevel() >= item.levelRequirement;
        bool hasEnoughMoney = PlayerStats.Instance.GetMoney() >= item.itemPrice;
        bool itemInStock = item.itemQuantity > 0;
        bool itemAvailable = item.isAvailable;

        return hasEnoughLevel && hasEnoughMoney && itemInStock && itemAvailable;
    }

    public void PurchaseItem(ShopItem item)
    {
        if (!CanPurchaseItem(item))
        {
            Debug.LogWarning($"Cannot purchase item: {item.itemName}");
            return;
        }

        // Process the purchase
        PlayerStats.Instance.RemoveMoney(item.itemPrice);
        item.itemQuantity--;

        if (item.itemQuantity <= 0)
        {
            item.isAvailable = false;
        }

        // Refresh the shop display to update quantities and button states
        RefreshShopDisplay();

        Debug.Log($"Purchased: {item.itemName} for {item.itemPrice}");
    }

    public void RefreshShopDisplay()
    {
        if (isShopPanelOpen)
        {
            DrawItems();
        }
    }

    private void ClearShopItems()
    {
        foreach (GameObject item in currentShopItems)
        {
            if (item != null)
            {
                DestroyImmediate(item);
            }
        }
        currentShopItems.Clear();
    }

    private void OnDestroy()
    {
        ClearShopItems();
    }

    // Optional: Add validation in editor
    private void OnValidate()
    {
        if (shopItems != null)
        {
            foreach (var item in shopItems)
            {
                if (item != null && item.itemPrice < 0)
                {
                    Debug.LogWarning($"Item {item.itemName} has negative price!");
                }
            }
        }
    }
}