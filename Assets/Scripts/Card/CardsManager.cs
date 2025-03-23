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
    [SerializeField] private GameObject playersManager;
    private PlayersManager playersManagerScript;

    [SerializeField] private DeckObj deck;

    // Cards in starting hand
    private int startingHandSize = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initializeScripts();
        deck.randomize();
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

        // Randomzie before drawing
        deck.randomize();

        for (int i = 0; i < startingHandSize; i++)
        {
            drawCard();
        }
    }

    public void updateCardUI(GameStateStruct currentGameState)
    {
        // Get the current player type
        PLAYER_TYPE currentPlayerType = playersManagerScript.currentPlayerScript.PlayerType;

        // Get the cards available
        List<CardPrefab> cardPrefabs = handArea.GetComponentsInChildren<CardPrefab>().ToList();

        foreach (CardPrefab cardPrefab in cardPrefabs)
        {
            // The previewed game state
            GameStateStruct previewGameState = currentGameState.copyCurrentState();

            // Save the card effect for preview generation
            CARD_EFFECT cardEffect = cardPrefab.card.cardEffect;

            // Find any modifications for this card/player_type
            Modification foundModification = cardPrefab.card.getCardModByPlayerType(currentPlayerType);

            // If a mod is found, update the card's text to reflect mod
            if (foundModification.newEffect != CARD_EFFECT.DEFAULT_NO_EFFECT)
            {
                cardPrefab.updateCardTextFromMod(foundModification);

                // Save this because we use it during updating the preview
                cardEffect = foundModification.newEffect;
            }

            // Update the visuals on the card to preview the next game state

            // Copy the state
            GameStateStruct initialState = previewGameState.copyCurrentState();

            // Update the preview state
            HandleCardEffectLogic.Start(ref previewGameState, cardEffect);

            // Dispatch the updates
            cardPrefab.updateCardPreview(initialState, previewGameState);
        }
    }

    private void initializeScripts()
    {
        // PlayerInit.cs
        playersManagerScript = playersManager.GetComponent<PlayersManager>();
    }

    public DeckObj getDeck()
    {
        return deck;
    }

    public void clearHand()
    {
        foreach (SelfDestruct cardToDestroy in handArea.GetComponentsInChildren<SelfDestruct>().ToList())
        {
            cardToDestroy.DestroySelf();
        }
    }
}
