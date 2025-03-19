using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    [SerializeField] public string cardName;
    [SerializeField] public CARD_EFFECT cardEffect;
    [SerializeField] public List<Modification> cardMods = new List<Modification>();

    public Modification getCardModByPlayerType(PLAYER_TYPE currentPlayerType)
    {
        return cardMods.Where(x => x.playerType == currentPlayerType).FirstOrDefault();
    }
}

public enum CARD_EFFECT
{
    DEFAULT_NO_EFFECT,
    BASE_HIT,
    GROUND_OUT,
    GAP_DOUBLE,
    WEAK_FLYOUT,
    HUSTLE_TRIPLE,
    HOME_RUN
}

[System.Serializable]
public struct Modification
{
    public PLAYER_TYPE playerType;
    public CARD_EFFECT newEffect;
    public bool improvement;        // Ability mod improves the card's outcome. Used by UI
}