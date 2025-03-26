using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeStore : MonoBehaviour
{
    // Prefab to use for upgrades
    [SerializeField] public GameObject upgradePrefab;

    // Transform of where the upgrades go
    [SerializeField] public Transform upgradeArea;

    // Max number of upgrades allowed in the store at one time
    private int maxNumberUpgrades = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// Unity built in - called when game object is enabled
    /// </summary>
    private void OnEnable()
    {
        // Find available upgrades when the menu is enabled
        populateUpgrades();
    }

    /// <summary>
    /// Unity built in, called when game object is disabled
    /// </summary>
    private void OnDisable()
    {
        // Destroy available upgrades when the menu is disabled
        destroyUpgradesInStore();
    }

    /// <summary>
    /// onClick event that opens the store
    /// </summary>
    public void openUpgradeStore()
    {
        // Set the store to visible
        gameObject.SetActive(true);
    }

    /// <summary>
    /// onClick event that closes the store
    /// </summary>
    public void closeUpgradeStore()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Populates the upgrade store with random card's random mods
    /// </summary>
    private void populateUpgrades()
    {
        // Prevent us from upgrading the same card twice
        int[] drawnCards = new int[maxNumberUpgrades];

        // Get the cards with available upgrades
        List<Card> cardsInDeck = CampaignManager.Instance.getDeck().getCards().ToList();
        Card[] cardsInDeckWithUpgrades = cardsInDeck.Where(x => x.hasUpgrades()).ToArray();

        // Reinitialize the drawnCards array
        for (int i = 0; i < drawnCards.Length; i++)
        {
            drawnCards[i] = -1;
        }

        // Add cards until we hit max number
        for (int i = 0; i < maxNumberUpgrades; i++)
        {
            // Get cards from the deck
            // DO NOT draw them, because they should be updated by reference in this case

            // Generate random index
            System.Random random = new System.Random();
            int j = random.Next(cardsInDeckWithUpgrades.Length - 1);

            // Only pull cards if we haven't pulled them yet
            if (!arrayContainsValue(drawnCards, j))
            {
                // Store the index of the cards we've pulled
                drawnCards[i] = j;

                // Create a new upgrade prefab in upgradeArea
                GameObject createdUpgrade = Instantiate(upgradePrefab, upgradeArea);

                // Pull the script from the newly created object
                UpgradePrefab upgradeScript = createdUpgrade.GetComponent<UpgradePrefab>();
                // Initialize
                upgradeScript.Initialize(cardsInDeckWithUpgrades[j], gameObject);
            }
            else
            {
                // Try again
                i--;
            }
            
        }
    }

    /// <summary>
    /// Destroy all upgrade objects in the upgrade store
    /// </summary>
    private void destroyUpgradesInStore()
    {
        foreach (Transform child in upgradeArea.transform)
        {
            Destroy(child.gameObject); // Destroy the child GameObject
        }
    }

    /// <summary>
    /// Because for some reason Linq Array.Contains() isn't working...
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    private bool arrayContainsValue(int[] arr, int val)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == val)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Choose the upgrade and assign it to the deck
    /// </summary>
    /// <param name="chosenUpgrade"></param>
    public void chooseUpgrade(UpgradePrefab chosenUpgrade)
    {
        // Find the card in the deck that matches the baseCard of the chosen upgrade
        // Since the deck can have multiples of the same card (e.g. flyout)
        // which one we choose doesn't really matter
        Card[] cardsInDeck = CampaignManager.Instance.getDeck().getCards();

        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] == chosenUpgrade.baseCard)
            {
                cardsInDeck[i] = chosenUpgrade.upgradeCard;
                break;
            }
        }
    }
}
