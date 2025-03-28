using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePrefab : MonoBehaviour
{
    // Card prefab to create
    [SerializeField] public GameObject cardPrefab;

    // References to the ugprade store
    private GameObject upgradeStore;
    private UpgradeStore upgradeStoreScript;

    // Base card available to upgrade and the option for it to upgrade to
    public Card baseCard;
    public Card upgradeCard;

    void Start()
    {

    }

    /// <summary>
    /// Called when creating a new instance of the prefab
    /// </summary>
    /// <param name="card"></param>
    /// <param name="_upgradeStore"></param>
    public void Initialize(Card card, GameObject _upgradeStore)
    {
        // Get the upgrade store info
        upgradeStore = _upgradeStore;
        upgradeStoreScript = upgradeStore.GetComponent<UpgradeStore>();

        // Set the base card
        baseCard = card;
        addCardToPrefab(baseCard);

        // Find the next upgrade
        upgradeCard = card.getRandomNextUpgrade();
        addCardToPrefab(upgradeCard);

        // Set the prefab's width to the children width
        udpateWidth();
    }

    /// <summary>
    /// Instantiates a card prefab and adds it to the upgrade
    /// </summary>
    /// <param name="card"></param>
    private void addCardToPrefab(Card card)
    {
        // Create a new instance of the card prefab and place it inside the upgrade area
        GameObject cardObject = Instantiate(cardPrefab, this.transform);

        // Assign the drawnCard scriptable object to the Card prefab object
        CardPrefab cardScript = cardObject.GetComponent<CardPrefab>();
        if (cardScript != null)
        {
            cardScript.Initialize(card); // Assign the card
        }

        // Assign card data to the UI
        cardObject.transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardName;

        // Also remove the button from the card, so it doesn't try to "play" it
        Button cardButton = cardObject.GetComponent<Button>();
        Destroy(cardButton);
    }

    /// <summary>
    /// Updates the Upgrade prefab object's width to accomodate cards
    /// </summary>
    private void udpateWidth()
    {
        // Find the transform to size
        RectTransform parentTransform = this.GetComponent<RectTransform>();
        float totalWidth = 0f;

        // Loop through each 1st child
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            // Get child
            Transform child = parentTransform.GetChild(i);

            // Check if the child has a RectTransform
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                // Add the child's width to the total
                totalWidth += childRect.rect.width;
            }
        }

        // Adjust the parent's width
        Vector2 newSize = parentTransform.sizeDelta;
        newSize.x = totalWidth; // Update the width
        parentTransform.sizeDelta = newSize;
    }

    /// <summary>
    /// onClick event to choose the upgrade
    /// </summary>
    public void chooseUpgrade()
    {
        // Apply the upgrade
        upgradeStoreScript.chooseUpgrade(this);

        // Close the store
        upgradeStoreScript.closeUpgradeStore();
    }
}
