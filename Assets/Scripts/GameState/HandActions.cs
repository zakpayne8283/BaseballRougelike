using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandActions : MonoBehaviour
{
    [SerializeField] private Deck deck;

    [SerializeField] private Transform handArea;

    // Prefabs to use
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private GameObject gameManager;

    private int startingHandSize = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard();
        }
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
    }
}
