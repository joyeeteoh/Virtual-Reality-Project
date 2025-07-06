// ShopItem.cs (Create this as a NEW file)
// PURPOSE: To define the data structure for a single shop item.
// By being in its own file, it can be easily accessed by any other script.

using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public Sprite itemIcon;
    public int price;
    public string playerPrefsKey; // Unique key to save the item's owned count.
}

