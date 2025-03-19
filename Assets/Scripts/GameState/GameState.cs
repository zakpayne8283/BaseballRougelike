using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] public GameObject fieldOfPlay;
    [SerializeField] public GameObject scoreBug;
    [SerializeField] public GameObject players; 

    GameStateStruct currentGameState;

    private bool topInning = true;
    private int inning = 1;

    private int awayScore = 0;
    private int homeScore = 0;

    private int outs = 0;

    private bool runnerOnFirst = false;
    private bool runnerOnSecond = false;
    private bool runnerOnThird = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGameState = new GameStateStruct(topInning, inning, awayScore, homeScore, outs, runnerOnFirst, runnerOnSecond, runnerOnThird);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCard(Card card)
    {
        HandleCardEffect(card);
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update the field of play (e.g. where runners are)
        FieldOfPlayManager fieldScript = fieldOfPlay.GetComponent<FieldOfPlayManager>();
        if (fieldScript != null)
        {
            fieldScript.UpdateFromGameState(currentGameState);
        }

        // Update the score bug as needed (inning, outs, score, etc.)
        ScoreBugManager scoreBugScript = scoreBug.GetComponent<ScoreBugManager>();
        if (scoreBugScript != null)
        {
            scoreBugScript.UpdateFromGameState(currentGameState);
        }

        // Update who the current player is in the player window
        PlayersInit playersScript = players.GetComponent<PlayersInit>();
        if (playersScript != null)
        {
            playersScript.SetNextPlayer();
        }
    }

    public void HandleCardEffect(Card card)
    {
        // Set the effect to look for
        CARD_EFFECT _effect = card.cardEffect;

        // Load the players script and check the current active player type for modifications
        PlayersInit playersScript = players.GetComponent<PlayersInit>();
        if (playersScript != null)
        {
            // Get the current player type
            PLAYER_TYPE currentPlayerType = playersScript.currentPlayer.GetComponent<Player>().PlayerType;

            // If player type is a special one
            if (currentPlayerType != PLAYER_TYPE.NONE)
            {
                // Check for modifications for that player type
                Modification foundModification = card.getCardModByPlayerType(currentPlayerType);

                if (foundModification.newEffect != CARD_EFFECT.DEFAULT_NO_EFFECT)
                {
                    _effect = foundModification.newEffect;
                }
            }
        }

        // Handle the effect of the played card
        HandleCardEffectLogic.Start(ref currentGameState, _effect);
    }
}

public struct GameStateStruct
{
    public bool topInning;
    public int inning;

    public int awayScore;
    public int homeScore;

    public int outs;

    public bool runnerOnFirst;
    public bool runnerOnSecond;
    public bool runnerOnThird;

    public GameStateStruct(bool _topInning, int _inning, int _awayScore, int _homeScore, int _outs, bool _first, bool _second, bool _third)
    {
        topInning = _topInning;
        inning = _inning;
        awayScore = _awayScore;
        homeScore = _homeScore;
        outs = _outs;
        runnerOnFirst = _first;
        runnerOnSecond = _second;
        runnerOnThird = _third;
    }

    public GameStateStruct copyCurrentState()
    {
        return new GameStateStruct(topInning, inning, awayScore, homeScore, outs, runnerOnFirst, runnerOnSecond, runnerOnThird);
    }
}