using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandActions : MonoBehaviour
{
    [SerializeField] private Deck deck;
    private Deck deckDefaultState;      // Keep this so we can reset the deck between innings

    [SerializeField] private Transform handArea;

    // Prefabs to use
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject playersObject;
    private PlayersInit playerScript;

    private int startingHandSize = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deckDefaultState = deck;

        playerScript = playersObject.GetComponent<PlayersInit>();

        DrawStartingHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard()
    {
        Card drawnCard = deck.DrawCardFromDeck();

        GameObject cardObject = Instantiate(cardPrefab, handArea);

        // Assign the drawnCard scriptable object to the Card prefab object
        CardPrefab cardScript = cardObject.GetComponent<CardPrefab>();
        if (cardScript != null)
        {
            cardScript.Initialize(drawnCard, gameManager); // Assign the card
        }

        // Assign card data to the UI
        cardObject.transform.Find("Card Name").GetComponent<TMP_Text>().text = drawnCard.cardName;

        // Update all visual card texts as needed
        UpdateCardTextBasedOnMods();
    }

    public void ResetDeck()
    {
        deck = deckDefaultState;
        deck.Randomize();

        foreach (Transform child in handArea)
        {
            Destroy(child.gameObject);
        }

        DrawStartingHand();
    }

    private void DrawStartingHand()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard();
        }
    }

    private void UpdateCardTextBasedOnMods()
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
}
