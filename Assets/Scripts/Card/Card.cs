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
public struct Modification
{
    public PLAYER_TYPE playerType;
    public CARD_EFFECT newEffect;
    public bool improvement;        // Ability mod improves the card's outcome. Used by UI
}