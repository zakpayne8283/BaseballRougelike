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

    // DEFAULT VALUES OF GAME STATE
    // TODO: Find better way to set this up
    private readonly bool topInning = true;
    private readonly int inning = 1;
    private readonly int awayScore = 0;
    private readonly int homeScore = 0;
    private readonly int outs = 0;
    private readonly bool runnerOnFirst = false;
    private readonly bool runnerOnSecond = false;
    private readonly bool runnerOnThird = false;
    private readonly bool changeInning = false;

    private bool GAME_ENDED = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGameState = new GameStateStruct(topInning, inning, awayScore, homeScore, outs, runnerOnFirst, runnerOnSecond, runnerOnThird, changeInning);
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
            playersScript.SetNextPlayer(currentGameState.changeInning);
        }

        // Change inning alwyas false after updating UI
        // UI should be updated with new inning already, so we want to default back to no change afterwards
        currentGameState.changeInning = false;
    }

    public void HandleCardEffect(Card card)
    {
        // TODO: Move modification logic into Card code

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

    public bool changeInning;

    public GameStateStruct(bool _topInning, int _inning, int _awayScore, int _homeScore, int _outs, bool _first, bool _second, bool _third, bool _changeInning)
    {
        topInning = _topInning;
        inning = _inning;
        awayScore = _awayScore;
        homeScore = _homeScore;
        outs = _outs;
        runnerOnFirst = _first;
        runnerOnSecond = _second;
        runnerOnThird = _third;
        changeInning = _changeInning;
    }

    public GameStateStruct copyCurrentState()
    {
        return new GameStateStruct(topInning, inning, awayScore, homeScore, outs, runnerOnFirst, runnerOnSecond, runnerOnThird, changeInning);
    }
}