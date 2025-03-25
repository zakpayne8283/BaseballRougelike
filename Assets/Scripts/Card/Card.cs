using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    [SerializeField] public string cardName;
    [SerializeField] public CARD_EFFECT cardEffect;
    [SerializeField] public CARD_RARITY cardRarity;
    [SerializeField] public List<Modification> cardMods = new List<Modification>();
    [SerializeField] public List<Card> cardUpgradesTo = new List<Card>();

    /// <summary>
    /// Used to create a card based on the loaded game save
    /// </summary>
    /// <param name="cardSave"></param>
    public Card(CardSaveState cardSave)
    {
        cardName = cardSave.cardName;
        cardEffect = cardSave.cardEffect;
        cardRarity = cardSave.cardRarity;
        cardMods = cardSave.cardMods;

        // Get the list of cards this card upgrades to
        cardUpgradesTo = new List<Card>();
        foreach (CardSaveState upgrade in cardSave.cardUpgradesTo)
        {
            // Sometimes the values can be null
            if (upgrade != null)
            {
                cardUpgradesTo.Add(new Card(upgrade));
            }
        }
    }

    /// <summary>
    /// Returns a mod for provided player type
    /// </summary>
    /// <param name="currentPlayerType"></param>
    /// <returns></returns>
    public Modification getCardModByPlayerType(PLAYER_TYPE currentPlayerType)
    {
        return cardMods.Where(x => x.playerType == currentPlayerType).FirstOrDefault();
    }
}

public enum CARD_EFFECT
{
    DEFAULT_NO_EFFECT,
    SINGLE,
    DOUBLE,
    TRIPLE,
    HOME_RUN,
    GROUNDOUT,
    FLYOUT,
    STRIKEOUT,
    STRIKEOUT_ON_BASE,
    WALK,
    LINEOUT
}

public enum CARD_RARITY
{
    COMMON,
    UNCOMMON,
    RARE
}

[System.Serializable]
public class Modification
{
    public PLAYER_TYPE playerType;
    public CARD_EFFECT newEffect;
    public bool improvement;        // Ability mod improves the card's outcome. Used by UI
}

[System.Serializable]
public class CardSaveState
{
    public string cardName;
    public CARD_EFFECT cardEffect;
    public CARD_RARITY cardRarity;
    public List<Modification> cardMods;
    public List<CardSaveState> cardUpgradesTo;

    /// <summary>
    /// Used to manually copy a Card object to a CardSaveState
    /// </summary>
    /// <param name="fromCard"></param>
    public CardSaveState(Card fromCard)
    {
        cardName = fromCard.cardName;
        cardEffect = fromCard.cardEffect;
        cardRarity = fromCard.cardRarity;
        cardMods = fromCard.cardMods;

        // Create a new CardSaveState for each card it can upgrade to
        cardUpgradesTo = new List<CardSaveState>();
        foreach (Card card in fromCard.cardUpgradesTo)
        {
            if (card != null)
            {
                cardUpgradesTo.Add(new CardSaveState(card));
            }
        }
    }

    public Card copyToCard()
    {
        return new Card(this);
    }
}