// using UnityEngine;
// using UnityEngine.Events; // Required for UnityEvent

// public class CurrencySystem : MonoBehaviour
// {
//     // --- Singleton Pattern ---
//     // Ensures there is only one instance of this class.
//     public static CurrencySystem Instance { get; private set; }

//     [SerializeField]
//     private int currentCoins;

//     // This event will be broadcasted whenever the coin amount changes.
//     // Other scripts (like your UI) can subscribe to this event.
//     public UnityEvent<int> OnCoinsChanged;

//     void Awake()
//     {
//         // Implement the singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // Optional: makes the currency persist between scenes
//         }
//         else
//         {
//             Destroy(gameObject);
//         }

//         // Initialize events if they are null
//         if (OnCoinsChanged == null)
//         {
//             OnCoinsChanged = new UnityEvent<int>();
//         }
//     }

//     void Start()
//     {
//         // Load saved coins at the start of the game
//         currentCoins = PlayerPrefs.GetInt("PlayerCoins", 100); // Start with 100 default coins
//         // Notify any listeners of the initial coin amount
//         OnCoinsChanged.Invoke(currentCoins);
//     }

//     public int GetCurrentCoins()
//     {
//         return currentCoins;
//     }

//     public void AddCoins(int amount)
//     {
//         currentCoins += amount;
//         SaveCoins();
//         OnCoinsChanged.Invoke(currentCoins); // Broadcast the change
//     }

//     public bool SpendCoins(int amount)
//     {
//         if (currentCoins >= amount)
//         {
//             currentCoins -= amount;
//             SaveCoins();
//             OnCoinsChanged.Invoke(currentCoins); // Broadcast the change
//             return true; // Purchase successful
//         }
//         return false; // Not enough coins
//     }

//     private void SaveCoins()
//     {
//         PlayerPrefs.SetInt("PlayerCoins", currentCoins);
//         PlayerPrefs.Save();
//     }
// }

using UnityEngine;
using UnityEngine.Events;
using System.Collections; // Required for coroutines (IEnumerator)

public class CurrencySystem : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static CurrencySystem Instance { get; private set; }

    [SerializeField]
    private int currentCoins;

    // --- Added: Fields for Timed Currency Increase ---
    [Header("Timed Currency Increase")]
    [Tooltip("The amount of coins to add at each interval.")]
    public int increaseAmount = 10;
    [Tooltip("The interval in seconds to add the coins.")]
    public float increaseIntervalSeconds = 5f;
    // --- End of Added Section ---

    // This event will be broadcasted whenever the coin amount changes.
    public UnityEvent<int> OnCoinsChanged;

    void Awake()
    {
        // Implement the singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: makes the currency persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize events if they are null
        if (OnCoinsChanged == null)
        {
            OnCoinsChanged = new UnityEvent<int>();
        }
    }

    void Start()
    {
        // Load saved coins at the start of the game
        currentCoins = PlayerPrefs.GetInt("PlayerCoins", 100); // Start with 100 default coins
        // Notify any listeners of the initial coin amount
        OnCoinsChanged.Invoke(currentCoins);

        // --- Added: Start the currency increase coroutine ---
        StartCoroutine(IncreaseCurrencyRoutine());
    }

    // --- Added: Coroutine for increasing currency over time ---
    private IEnumerator IncreaseCurrencyRoutine()
    {
        // This creates an infinite loop that runs for the entire game session.
        while (true)
        {
            // Wait for the specified number of seconds
            yield return new WaitForSeconds(increaseIntervalSeconds);

            // Add the coins and notify listeners
            AddCoins(increaseAmount);
        }
    }
    // --- End of Added Section ---

    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        SaveCoins();
        OnCoinsChanged.Invoke(currentCoins); // Broadcast the change
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            SaveCoins();
            OnCoinsChanged.Invoke(currentCoins); // Broadcast the change
            return true; // Purchase successful
        }
        return false; // Not enough coins
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("PlayerCoins", currentCoins);
        PlayerPrefs.Save();
    }
}