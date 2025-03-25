using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckObj", menuName = "Scriptable Objects/DeckObj")]
public class DeckObj : ScriptableObject
{
    [SerializeField] public Card[] _cardsDefault;
    private Card[] cards;

    /// <summary>
    /// Used for copying to a new Deck Object
    /// </summary>
    public DeckObj(DeckObj _deck)
    {
        _cardsDefault = _deck._cardsDefault;
        cards = _deck.cards;
    }

    /// <summary>
    /// Used when loading a deck from a save
    /// </summary>
    /// <param name="deckSave"></param>
    public DeckObj(DeckSaveState deckSave)
    {
        // Load each card first
        _cardsDefault = new Card[deckSave._cardsDefault.Length];
        for (int i = 0; i < deckSave._cardsDefault.Length; i++)
        {
            _cardsDefault[i] = new Card(deckSave._cardsDefault[i]);
        }

        // Set the loaded cards
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
        if (cards.Length == 0)
            resetToInitialState();

        DeckObj copiedDeck = new DeckObj(this);

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

    /// <summary>
    /// Resets to initial state of deck. Used when changing innings
    /// </summary>
    public void resetToInitialState()
    {
        // Reset the size
        cards = new Card[_cardsDefault.Length];

        // Copy
        Array.Copy(_cardsDefault, cards, _cardsDefault.Length);
    }

    /// <summary>
    /// Gets a DeckSaveState based on the current deck. Used for saving to JSON
    /// </summary>
    /// <returns></returns>
    public DeckSaveState getDeckSaveState()
    {
        return new DeckSaveState(this);
    }
}

/// <summary>
/// Class used for saving the campaign's deck to JSON
/// </summary>
[System.Serializable]
public class DeckSaveState
{
    // CardSaveState so it also easily serializes
    public CardSaveState[] _cardsDefault;

    /// <summary>
    /// Create a new save state based on provided deck
    /// </summary>
    /// <param name="deck"></param>
    public DeckSaveState(DeckObj deck)
    {
        _cardsDefault = new CardSaveState[deck._cardsDefault.Length];

        // Save all the cards too
        for (int i = 0; i < deck._cardsDefault.Length; i++)
        {
            _cardsDefault[i] = new CardSaveState(deck._cardsDefault[i]);
        }
    }

    /// <summary>
    /// Copy the DeckSaveState to a usable DeckObj
    /// </summary>
    /// <returns></returns>
    public DeckObj copyToDeck()
    {
        return new DeckObj(this);
    }
}