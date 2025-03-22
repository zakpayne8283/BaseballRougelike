using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckObj", menuName = "Scriptable Objects/DeckObj")]
public class DeckObj : ScriptableObject
{
    [SerializeField] private Card[] _cardsDefault;
    private Card[] cards;

    public DeckObj()
    {
        cards = _cardsDefault;
    }

    /// <summary>
    /// Take the card on top of the deck
    /// </summary>
    /// <returns></returns>
    public Card drawCardFromDeck()
    {
        // Pull the output
        Card output = cards[0];

        // Shift the array up one spot so the next value is at [0]
        Card[] nextCards = new Card[cards.Length - 1];
        Array.Copy(cards, 1, nextCards, 0, nextCards.Length);

        // Reset cards to the next state
        cards = nextCards;

        // out
        return output;
    }
    
    /// <summary>
    /// Creates a copied version of the deck
    /// </summary>
    /// <returns></returns>
    public DeckObj copyObject()
    {
        DeckObj copiedDeck = new DeckObj();
        copiedDeck.cards = cards;

        return copiedDeck;
    }

    /// <summary>
    /// Randomizes the deck
    /// </summary>
    public void randomize()
    {
        System.Random random = new System.Random();

        // Fischer-Yates shuffle - thanks GPT
        for (int i = cards.Length - 1; i > 0; i--)
        {
            // Generate random index
            int j = random.Next(i + 1);

            // Swap cards[i] with cards[j]
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
}
