using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI itemOwnedText;
    public Image itemIcon;
    public GameObject highlightBorder;

    public void UpdateDisplay(ShopItem item)
    {
        itemNameText.text = item.itemName;
        itemPriceText.text = "Price: " + item.price.ToString();
        itemIcon.sprite = item.itemIcon;

        int ownedCount = PlayerPrefs.GetInt(item.playerPrefsKey, 0);
        itemOwnedText.text = "Owned: " + ownedCount.ToString();

        itemIcon.enabled = (item.itemIcon != null);
    }

    public void SetHighlight(bool isActive)
    {
        if (highlightBorder != null)
        {
            highlightBorder.SetActive(isActive);
        }
    }
}