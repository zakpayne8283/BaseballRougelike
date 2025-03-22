using UnityEngine;

public class HandleCardEffectLogic
{
    private static GameStateStruct _gameState;

    /// <summary>
    /// Begin handling the card effect
    /// </summary>
    /// <param name="currentGameState">[Ref] Game state provided by GameState.cs</param>
    /// <param name="cardEffect">Listed card effect</param>
    /// <param name="cardsManager">Script that manages hand - used to draw a card</param>
    public static void Start(ref GameStateStruct currentGameState, CARD_EFFECT cardEffect, CardsManager cardsManager)
    {
        // TODO: Reconsider handScript

        // Copy the current state so we can edit it, then set 
        // the original value to what we've updated.
        _gameState = currentGameState.copyCurrentState();

        switch (cardEffect)
        {
            case CARD_EFFECT.SINGLE:
                HandleSingle();
                cardsManager.drawCard();
                break;
            case CARD_EFFECT.DOUBLE:
                HandleDouble();
                cardsManager.drawCard();
                break;
            case CARD_EFFECT.TRIPLE:
                HandleTriple();
                cardsManager.drawCard();
                break;
            case CARD_EFFECT.HOME_RUN:
                HandleHomeRun();
                cardsManager.drawCard();
                break;
            case CARD_EFFECT.GROUNDOUT:
                HandleGroundOut();
                break;
            case CARD_EFFECT.FLYOUT:
                HandleFlyOut();
                break;
            case CARD_EFFECT.STRIKEOUT:
                HandleStrikeOut();
                break;
            case CARD_EFFECT.STRIKEOUT_ON_BASE:
                HandleStrikeOutOnBase();
                cardsManager.drawCard();
                break;
            case CARD_EFFECT.WALK:
                HandleWalk();
                cardsManager.drawCard();
                break;
            case CARD_EFFECT.DEFAULT_NO_EFFECT:
                break;
        }

        // Edit the main game state (used in GameState.cs)
        currentGameState = _gameState;
    }

    /// <summary>
    /// CARD_EFFECT.BASE_HIT -- all runners move one base
    /// </summary>
    private static void HandleSingle()
    {
        if (_gameState.runnerOnThird)
        {
            AdvanceRunner(3);
        }

        if (_gameState.runnerOnSecond)
        {
            AdvanceRunner(2);
        }

        if (_gameState.runnerOnFirst)
        {
            AdvanceRunner(1);
        }

        _gameState.runnerOnFirst = true;
    }

    /// <summary>
    /// CARD_EFFECT.GAP_DOUBLE -- move all runners two bases
    /// </summary>
    public static void HandleDouble()
    {
        if (_gameState.runnerOnThird)
        {
            AdvanceRunner(3, 2);
        }

        if (_gameState.runnerOnSecond)
        {
            AdvanceRunner(2, 2);
        }

        if (_gameState.runnerOnFirst)
        {
            AdvanceRunner(1, 2);
        }

        _gameState.runnerOnSecond = true;
    }

    public static void HandleTriple()
    {
        if (_gameState.runnerOnThird)
        {
            AdvanceRunner(3, 3);
        }

        if (_gameState.runnerOnSecond)
        {
            AdvanceRunner(2, 3);
        }

        if (_gameState.runnerOnFirst)
        {
            AdvanceRunner(1, 3);
        }

        _gameState.runnerOnThird = true;
    }

    public static void HandleHomeRun()
    {
        if (_gameState.runnerOnThird)
        {
            AdvanceRunner(3, 4);
        }

        if (_gameState.runnerOnSecond)
        {
            AdvanceRunner(2, 4);
        }

        if (_gameState.runnerOnFirst)
        {
            AdvanceRunner(1, 4);
        }

        ScoreRun();
    }

    /// <summary>
    /// CARD_EFFECT.GROUND_OUT -- cont.
    /// - 2 outs = end inning
    /// </summary>
    public static void HandleGroundOut()
    {
        // End half inning if 2 outs (1b force out)
        if (_gameState.outs >= 2)
        {
            TurnOverInning();
        }
        else
        {
            // Bases loaded
            if (
                _gameState.runnerOnThird &&
                _gameState.runnerOnSecond &&
                _gameState.runnerOnFirst
                )
            {
                // double play
                if (_gameState.outs >= 1)
                {
                    ClearBases();
                    TurnOverInning();
                }
                else
                {
                    // Double play, only remove runner on first
                    // X-2-3 double play to prevent the run
                    _gameState.runnerOnFirst = false;
                    _gameState.outs = 2;
                }
            }
            // Second and 3rd
            else if (
                _gameState.runnerOnThird &&
                _gameState.runnerOnSecond
                )
            {
                // non-force runners advance
                AdvanceRunner(3);
                AdvanceRunner(2);

                // force out
                _gameState.outs++;
            }
            // 1st and 3rd
            else if (
                _gameState.runnerOnThird &&
                _gameState.runnerOnFirst
                )
            {
                // double play, turn over inning
                if (_gameState.outs >= 1)
                {
                    TurnOverInning();
                }
                else
                {
                    // runner on 3rd scores
                    AdvanceRunner(3);

                    // Two force outs
                    _gameState.outs = 2;
                }
            }
            // 3rd only
            else if (_gameState.runnerOnThird)
            {
                // Runner on 3rd scores
                AdvanceRunner(3);

                // force out at 1b
                _gameState.outs++;
            }
            // 2nd and 1st
            else if (
                _gameState.runnerOnSecond &&
                _gameState.runnerOnFirst
                )
            {
                // Ground out double play
                if (_gameState.outs >= 1)
                {
                    TurnOverInning();
                }
                else
                {
                    // Runner on 2nd advances
                    AdvanceRunner(2);

                    // Runner on first is doubled up
                    _gameState.runnerOnFirst = false;

                    // And also the force out at 1b makes 2
                    _gameState.outs = 2;
                }
            }
            // 2nd only
            else if (_gameState.runnerOnSecond)
            {
                // Runner on 2nd advances
                AdvanceRunner(2);

                // force out at 1b
                _gameState.outs++;
            }
            // 1st only
            else if (_gameState.runnerOnFirst)
            {
                // Runner on 1st forced out at 2nd for DP
                _gameState.runnerOnFirst = false;
                _gameState.outs = 2;
            }
            // bases empty
            else
            {
                // <2 outs just add an out
                _gameState.outs++;
            }
        }
    }

    public static void HandleFlyOut()
    {
        if (_gameState.outs >= 2)
        {
            TurnOverInning();
        }
        else
        {
            if (_gameState.runnerOnThird)
            {
                AdvanceRunner(3, 1);
            }

            _gameState.outs++;
        }
    }

    public static void HandleStrikeOut()
    {
        if (_gameState.outs >= 2)
        {
            TurnOverInning();
        }
        else
        {
            _gameState.outs++;
        }
    }

    public static void HandleStrikeOutOnBase()
    {
        // Handle unusual cases first
        // no drop 3rd strike if 1st is occupied, unless there's 2 outs
        if (_gameState.outs < 2 && _gameState.runnerOnFirst && !BasesAreLoaded())
        {
            _gameState.outs++;
        }
        else if (BasesAreLoaded())
        {
            // Bases loaded gets a double play
            if (_gameState.outs >= 1)
            {
                TurnOverInning();
            }
            else
            {
                _gameState.outs = 2;
            }
        }
        else
        {
            if (_gameState.runnerOnSecond)
            {
                AdvanceRunner(2, 1);
            }
            
            if (_gameState.runnerOnFirst)
            {
                AdvanceRunner(1, 1);
            }

            _gameState.runnerOnFirst = true;
        }
    }

    public static void HandleWalk()
    {
        // Need to move all runners where there's an available force
        if (BasesAreLoaded())
        {
            AdvanceRunner(3, 1);
            AdvanceRunner(2, 1);
            AdvanceRunner(1, 1);
        }
        else
        {
            if (_gameState.runnerOnSecond && _gameState.runnerOnFirst)
            {
                AdvanceRunner(2, 1);
                AdvanceRunner(1, 1);
            }

            if (_gameState.runnerOnFirst)
            {
                AdvanceRunner(1, 1);
            }
        }

        _gameState.runnerOnFirst = true;
    }

    /// <summary>
    /// Run scores, increase active team score
    /// </summary>
    public static void ScoreRun()
    {
        if (_gameState.topInning)
        {
            _gameState.awayScore++;
        }
        else
        {
            _gameState.homeScore++;
        }
    }

    /// <summary>
    /// Remove all runners from base
    /// </summary>
    private static void ClearBases()
    {
        _gameState.runnerOnFirst = false;
        _gameState.runnerOnSecond = false;
        _gameState.runnerOnThird = false;
    }

    /// <summary>
    /// Change to next half inning. Clear bases, move to top/bottom
    /// </summary>
    private static void TurnOverInning()
    {
        ClearBases();

        _gameState.outs = 0;
        _gameState.topInning = !_gameState.topInning;
        if(_gameState.topInning) _gameState.inning++;
        _gameState.changeInning = true;
    }

    /// <summary>
    /// Advances the runner the specified number of bases, default 1
    /// </summary>
    /// <param name="currentBase"></param>
    /// <param name="basesToAdvance"></param>
    private static void AdvanceRunner(int currentBase, int basesToAdvance=1)
    {
        // TODO: Make this more algorithm-y and less hard code-y

        if (currentBase == 1)
        {
            _gameState.runnerOnFirst = false;

            if (basesToAdvance == 1)
            {
                _gameState.runnerOnSecond = true;
            }
            else if (basesToAdvance == 2)
            {
                _gameState.runnerOnThird = true;
            }
            else
            {
                ScoreRun();
            }
        }
        else if (currentBase == 2)
        {
            _gameState.runnerOnSecond = false;

            if (basesToAdvance == 1)
            {
                _gameState.runnerOnThird = true;
            }
            else
            {
                ScoreRun();
            }
        }
        else if (currentBase == 3)
        {
            _gameState.runnerOnThird = false;

            ScoreRun();
        }
    }

    private static bool BasesAreLoaded()
    {
        if (_gameState.runnerOnFirst && _gameState.runnerOnSecond && _gameState.runnerOnThird)
        {
            return true;
        }

        return false;
    }
}
