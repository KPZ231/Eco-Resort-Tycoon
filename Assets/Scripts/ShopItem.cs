using System.Collections;
using UnityEngine;

public enum ItemType
{
    QuestItem
}

[System.Serializable]
public class ShopItem
{
    public string itemName; // Name of the item
    public string itemDescription; // Description of the item
    public Sprite itemIcon; // Icon representing the item
    public int itemPrice; // Price of the item in the shop
    public int itemID; // Unique identifier for the item
    public bool isPurchased; // Flag to indicate if the item has been purchased
    public int itemQuantity; // Quantity of the item available in the shop
    public int maxQuantity; // Maximum quantity of the item that can be purchased
    public bool isAvailable; // Flag to indicate if the item is available for purchase
    public int levelRequirement; // Level requirement to purchase the item
    public ItemType itemType; // Type of the item (e.g., QuestItem, Consumable, etc.)
}
