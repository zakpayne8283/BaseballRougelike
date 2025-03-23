using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] public GameObject fieldOfPlayManager;
    private FieldOfPlayManager fieldOfPlayManagerScript;
    [SerializeField] public GameObject scoreBugManager;
    private ScoreBugManager scoreBugManagerScript;
    [SerializeField] public GameObject playersManager;
    private PlayersManager playersManagerScript;

    // Hand area object and script
    [SerializeField] public GameObject cardsManager;
    private CardsManager cardsManagerScript;

    private CustomLogger logger;

    // Current state (inning, score, outs, etc.)
    GameStateStruct currentGameState;

    private bool GAME_ENDED = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get all the scripts setup
        initializeScripts();

        // Setup custom logger
        logger = new CustomLogger();

        // Set the initial game state
        initializeGameState();

        // draw cards for the game to start
        cardsManagerScript.drawStartingHand();

        // Update the card texts now because they will always be default otherwise
        cardsManagerScript.updateCardUI(currentGameState.copyCurrentState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCard(Card card)
    {
        HandleCardEffect(card);
        CheckForEndState();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (GAME_ENDED)
        {
            EndGame();
        }

        // Update the field of play (e.g. where runners are)
        fieldOfPlayManagerScript.UpdateFromGameState(currentGameState);

        // Update the score bug as needed (inning, outs, score, etc.)
        scoreBugManagerScript.UpdateFromGameState(currentGameState);

        // Set the next active player
        playersManagerScript.setNextPlayer(currentGameState.changeInning);
        
        // Update the card texts for the next batter
        cardsManagerScript.updateCardUI(currentGameState);

        // Change inning alwyas false after updating UI
        // UI should be updated with new inning already, so we want to default back to no change afterwards
        currentGameState.changeInning = false;
    }

    public void HandleCardEffect(Card card)
    {
        // Set the effect to look for
        CARD_EFFECT _effect = card.cardEffect;

        // Get the current player type
        PLAYER_TYPE currentPlayerType = playersManagerScript.currentPlayerScript.PlayerType;

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

        // Logging for balance purposes
        logger.WriteLog($"{playersManagerScript.currentPlayerScript.PlayerType},{_effect.ToString()},{currentGameState.topInning}");

        // Handle the effect of the played card
        HandleCardEffectLogic.Start(ref currentGameState, _effect, cardsManagerScript);

        // If we're changing innings, reset the deck
        if (currentGameState.changeInning)
        {
            // Empty the current hand
            cardsManagerScript.clearHand();
            // Draw a new hand
            cardsManagerScript.drawStartingHand();
        }
    }

    private void CheckForEndState()
    {
        // Game ends when:
        // End of 9th (or higher) inning and one team has a higher score
        if (currentGameState.inning >= 9)
        {
            // Away team wins
            if (currentGameState.awayScore > currentGameState.homeScore)
            {
                GAME_ENDED = true;
            }
            // Home team wins
            else if (currentGameState.homeScore > currentGameState.awayScore)
            {
                GAME_ENDED = true;
            }
        }
    }

    private void EndGame()
    {
        //
        Application.Quit();
    }

    /// <summary>
    /// Gets all of the required game scripts from other managers
    /// </summary>
    private void initializeScripts()
    {
        cardsManagerScript = cardsManager.GetComponent<CardsManager>();
        fieldOfPlayManagerScript = fieldOfPlayManager.GetComponent<FieldOfPlayManager>();
        playersManagerScript = playersManager.GetComponent<PlayersManager>();
        scoreBugManagerScript = scoreBugManager.GetComponent<ScoreBugManager>();
    }

    private void initializeGameState()
    {
        // hard coded here because these values will likely never change
        currentGameState = new GameStateStruct(true, 1, 0, 0, 0, false, false, false, false);
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