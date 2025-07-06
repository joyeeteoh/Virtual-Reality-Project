// PlayerInteractor.cs (Minor Tweak)
// PURPOSE: Detects when the player is near a shop and handles opening/closing it.
// USAGE: Attach to the Player GameObject.

using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public KeyCode openShopKey = KeyCode.B;
    // We don't need a close key here anymore, as it's handled in ShopSystem.cs

    [Header("UI Connections")]
    [Tooltip("Assign a UI Text element to show prompts like 'Press B to Shop'.")]
    public TextMeshProUGUI interactionPrompt; 
    
    private ShopSystem currentShop;

    void Start()
    {
        // Hide the prompt at the start of the game
        if(interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the player is near a shop and presses the open key
        if (currentShop != null && Input.GetKeyDown(openShopKey))
        {
            currentShop.OpenShop();
            // Hide the interaction prompt once the shop is open
            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }

    // Called when the player enters a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we collided with has a ShopSystem
        ShopSystem shop = other.GetComponent<ShopSystem>();
        if (shop != null)
        {
            currentShop = shop;
            // Show the interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.text = "Press '" + openShopKey.ToString() + "' to Shop";
                interactionPrompt.gameObject.SetActive(true);
            }
        }
    }

    // Called when the player exits a trigger collider
    private void OnTriggerExit(Collider other)
    {
        ShopSystem shop = other.GetComponent<ShopSystem>();
        if (shop != null && shop == currentShop)
        {
            // Player has walked away, so clear the current shop reference
            currentShop = null;
            // Hide the interaction prompt
            if(interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}
