using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    [SerializeField] public string cardName;
    [SerializeField] public CARD_EFFECT cardEffect;
    [SerializeField] public List<Modification> cardMods = new List<Modification>();
}

public enum CARD_EFFECT
{
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