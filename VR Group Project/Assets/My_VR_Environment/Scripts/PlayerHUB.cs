using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI coinText; // Assign your main coin display text here

    void Start()
    {
        // Ensure the CurrencySystem exists
        if (CurrencySystem.Instance != null)
        {
            // Subscribe the UpdateCoinText method to the OnCoinsChanged event.
            // Now, whenever the event is invoked, our method will be called.
            CurrencySystem.Instance.OnCoinsChanged.AddListener(UpdateCoinText);

            // Set the initial value
            UpdateCoinText(CurrencySystem.Instance.GetCurrentCoins());
        }
        else
        {
            Debug.LogError("CurrencySystem instance not found!");
        }
    }

    // This method is called automatically when the OnCoinsChanged event is fired.
    private void UpdateCoinText(int amount)
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + amount;
        }
    }

    // It's good practice to unsubscribe from events when the object is destroyed.
    void OnDestroy()
    {
        if (CurrencySystem.Instance != null)
        {
            CurrencySystem.Instance.OnCoinsChanged.RemoveListener(UpdateCoinText);
        }
    }
}