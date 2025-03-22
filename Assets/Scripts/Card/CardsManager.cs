using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages both the cards in hand and the cards in the deck
/// </summary>
public class CardsManager : MonoBehaviour
{
    // Area where cards will be added to
    [SerializeField] private Transform handArea;

    // Prefabs to use for the cards
    [SerializeField] private GameObject cardPrefab;

    // GameState.cs
    [SerializeField] private GameObject gameStateManager;

    // Player lineup area
    [SerializeField] private GameObject playersObject;
    private PlayersInit playerScript;

    [SerializeField] private DeckObj deck;

    // Cards in starting hand
    private int startingHandSize = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initializeScripts();

        // Update all visual card texts as needed
        // updateCardTextBasedOnMods();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drawCard()
    {
        // Draw a card from the deck
        Card drawnCard = deck.drawCardFromDeck();

        // Create a new instance of the card prefab and place it in the hand area
        GameObject cardObject = Instantiate(cardPrefab, handArea);

        // Assign the drawnCard scriptable object to the Card prefab object
        CardPrefab cardScript = cardObject.GetComponent<CardPrefab>();
        if (cardScript != null)
        {
            cardScript.Initialize(drawnCard, gameStateManager); // Assign the card
        }

        // Assign card data to the UI
        cardObject.transform.Find("Card Name").GetComponent<TMP_Text>().text = drawnCard.cardName;
    }

    public void drawStartingHand()
    {
        // Called here because of the timing of object creation
        deck.resetToInitialState();

        for (int i = 0; i < startingHandSize; i++)
        {
            drawCard();
        }
    }

    public void updateCardTextBasedOnMods()
    {
        // Get the current player type
        PLAYER_TYPE currentPlayerType = playerScript.currentPlayer.GetComponent<Player>().PlayerType;

        // Get the cards available
        List<CardPrefab> cardPrefabs = handArea.GetComponentsInChildren<CardPrefab>().ToList();

        foreach (CardPrefab cardPrefab in cardPrefabs)
        {
            Modification foundModification = cardPrefab.card.getCardModByPlayerType(currentPlayerType);

            Color textColor = Color.black;

            if (foundModification.newEffect != CARD_EFFECT.DEFAULT_NO_EFFECT)
            {
                if (foundModification.improvement)
                {
                    textColor = Color.green;
                }
                else
                {
                    textColor = Color.red;
                }
            }

            cardPrefab.transform.Find("Card Name").GetComponent<TextMeshProUGUI>().color = textColor;
        }
    }

    private void initializeScripts()
    {
        // PlayerInit.cs
        playerScript = playersObject.GetComponent<PlayersInit>();
    }

    public DeckObj getDeck()
    {
        return deck;
    }
}
