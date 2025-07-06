// // ShopSystem.cs (Refactored)
// // PURPOSE: Manages the logic for a single shop stall, including its inventory and UI interaction.
// // USAGE: Attach this to the physical shop stall object in your scene (e.g., a 3D model of a market stall).

// using UnityEngine;
// using System.Collections.Generic;
// using TMPro;

// public class ShopSystem : MonoBehaviour
// {
//     [Header("Shop Inventory")]
//     [Tooltip("The list of items that THIS specific stall will sell.")]
//     public List<ShopItem> shopItems;

//     [Header("UI Connections")]
//     [Tooltip("The main UI panel for this shop.")]
//     public GameObject shopPanel;
//     [Tooltip("The text element inside the shop that displays the player's current coins.")]
//     public TextMeshProUGUI shopCoinText;
//     [Tooltip("Drag the individual Item UI panels here. The order must match the Shop Items list.")]
//     public List<ShopItemUI> itemDisplays; // List of UI slots

//     private int currentItemIndex = 0;
//     private bool isShopOpen = false;

//     void Start()
//     {
//         // Ensure the shop panel is closed on start
//         if (shopPanel != null)
//         {
//             shopPanel.SetActive(false);
//         }
//     }

//     // Opens the shop, enables UI, and updates all displays.
//     public void OpenShop()
//     {
//         isShopOpen = true;
//         shopPanel.SetActive(true);
//         currentItemIndex = 0;
        
//         // Find the player and disable their movement
//         PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
//         if (playerMovement != null)
//         {
//             playerMovement.SetCanMove(false);
//         }

//         UpdateShopUI();
//     }

//     // Closes the shop and re-enables player movement.
//     public void CloseShop()
//     {
//         isShopOpen = false;
//         shopPanel.SetActive(false);
        
//         PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
//         if (playerMovement != null)
//         {
//             playerMovement.SetCanMove(true);
//         }
//     }

//     void Update()
//     {
//         // Only process input if the shop panel is active
//         if (!isShopOpen) return;

//         // Navigate to the next item
//         if (Input.GetKeyDown(KeyCode.Tab))
//         {
//             // Move to the next item, wrapping around if at the end
//             currentItemIndex = (currentItemIndex + 1) % itemDisplays.Count;
//             UpdateHighlights();
//         }

//         // Buy the currently selected item
//         if (Input.GetKeyDown(KeyCode.C))
//         {
//             BuyCurrentItem();
//         }

//         // Close the shop
//         if (Input.GetKeyDown(KeyCode.X))
//         {
//             CloseShop();
//         }
//     }

//     // Updates all visual elements of the shop.
//     private void UpdateShopUI()
//     {
//         // Update the coin display inside the shop
//         if (CurrencySystem.Instance != null)
//         {
//             shopCoinText.text = "Coins: " + CurrencySystem.Instance.GetCurrentCoins();
//         }

//         // Update each item display slot
//         for (int i = 0; i < itemDisplays.Count; i++)
//         {
//             // Check if there is a corresponding item in the inventory for this UI slot
//             if (i < shopItems.Count)
//             {
//                 itemDisplays[i].UpdateDisplay(shopItems[i]);
//             }
//         }
//         UpdateHighlights();
//     }

//     // Updates which item has the highlight border.
//     private void UpdateHighlights()
//     {
//         for (int i = 0; i < itemDisplays.Count; i++)
//         {
//             // Set highlight true only for the selected item
//             itemDisplays[i].SetHighlight(i == currentItemIndex);
//         }
//     }

//     // Handles the purchase logic for the currently selected item.
//     public void BuyCurrentItem()
//     {
//         // Ensure the selected index is valid for the shop's inventory
//         if (currentItemIndex >= shopItems.Count) return;

//         ShopItem selectedItem = shopItems[currentItemIndex];

//         // Attempt to spend coins via the central CurrencySystem
//         if (CurrencySystem.Instance.SpendCoins(selectedItem.price))
//         {
//             // Purchase successful
//             Debug.Log("Bought " + selectedItem.itemName);

//             // Increment and save the owned count for the purchased item
//             int ownedCount = PlayerPrefs.GetInt(selectedItem.playerPrefsKey, 0);
//             PlayerPrefs.SetInt(selectedItem.playerPrefsKey, ownedCount + 1);
//             PlayerPrefs.Save();

//             // Refresh the entire shop UI to show new coin total and owned count
//             UpdateShopUI();
//         }
//         else
//         {
//             Debug.Log("Not enough coins to buy " + selectedItem.itemName + "!");
//             // Optional: Add a sound effect or UI feedback for failed purchase
//         }
//     }
// }

using UnityEngine;
using System.Collections.Generic;
using TMPro;

// --- Modified: Ensures an AudioSource component is on the same GameObject ---
[RequireComponent(typeof(AudioSource))]
public class ShopSystem : MonoBehaviour
{
    [Header("Shop Inventory")]
    [Tooltip("The list of items that THIS specific stall will sell.")]
    public List<ShopItem> shopItems;

    [Header("UI Connections")]
    [Tooltip("The main UI panel for this shop.")]
    public GameObject shopPanel;
    [Tooltip("The text element inside the shop that displays the player's current coins.")]
    public TextMeshProUGUI shopCoinText;
    [Tooltip("Drag the individual Item UI panels here. The order must match the Shop Items list.")]
    public List<ShopItemUI> itemDisplays; // List of UI slots

    // --- Added: Audio Fields ---
    [Header("Audio")]
    [Tooltip("Sound to play when the shop is opened.")]
    public AudioClip openShopSound;
    [Tooltip("Sound to play when the shop is closed.")]
    public AudioClip closeShopSound;
    [Tooltip("Sound to play when an item is purchased successfully.")]
    public AudioClip buyItemSound;
    [Tooltip("Sound to play when switching between items.")]
    public AudioClip switchItemSound;
    // --- End of Added Section ---

    private int currentItemIndex = 0;
    private bool isShopOpen = false;
    private AudioSource audioSource; // --- Added: Reference to the AudioSource ---

    void Start()
    {
        // --- Added: Get the AudioSource component ---
        audioSource = GetComponent<AudioSource>();

        // Ensure the shop panel is closed on start
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    // Opens the shop, enables UI, and updates all displays.
    public void OpenShop()
    {
        // --- Added: Play open sound effect ---
        if (openShopSound != null) audioSource.PlayOneShot(openShopSound);

        isShopOpen = true;
        shopPanel.SetActive(true);
        currentItemIndex = 0;
        
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        UpdateShopUI();
    }

    // Closes the shop and re-enables player movement.
    public void CloseShop()
    {
        // --- Added: Play close sound effect ---
        if (closeShopSound != null) audioSource.PlayOneShot(closeShopSound);

        isShopOpen = false;
        shopPanel.SetActive(false);
        
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }

    void Update()
    {
        if (!isShopOpen) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentItemIndex = (currentItemIndex + 1) % itemDisplays.Count;
            UpdateHighlights();
            // --- Added: Play item switch sound effect ---
            if (switchItemSound != null) audioSource.PlayOneShot(switchItemSound);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            BuyCurrentItem();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CloseShop();
        }
    }

    private void UpdateShopUI()
    {
        if (CurrencySystem.Instance != null)
        {
            shopCoinText.text = "Coins: " + CurrencySystem.Instance.GetCurrentCoins();
        }

        for (int i = 0; i < itemDisplays.Count; i++)
        {
            if (i < shopItems.Count)
            {
                itemDisplays[i].UpdateDisplay(shopItems[i]);
            }
        }
        UpdateHighlights();
    }

    private void UpdateHighlights()
    {
        for (int i = 0; i < itemDisplays.Count; i++)
        {
            itemDisplays[i].SetHighlight(i == currentItemIndex);
        }
    }

    public void BuyCurrentItem()
    {
        if (currentItemIndex >= shopItems.Count) return;

        ShopItem selectedItem = shopItems[currentItemIndex];

        if (CurrencySystem.Instance.SpendCoins(selectedItem.price))
        {
            // --- Added: Play buy sound effect ---
            if (buyItemSound != null) audioSource.PlayOneShot(buyItemSound);

            Debug.Log("Bought " + selectedItem.itemName);
            int ownedCount = PlayerPrefs.GetInt(selectedItem.playerPrefsKey, 0);
            PlayerPrefs.SetInt(selectedItem.playerPrefsKey, ownedCount + 1);
            PlayerPrefs.Save();
            UpdateShopUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy " + selectedItem.itemName + "!");
        }
    }
}