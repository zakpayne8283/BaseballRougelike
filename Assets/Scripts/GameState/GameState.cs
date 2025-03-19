using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] public GameObject fieldOfPlay;
    [SerializeField] public GameObject scoreBug;
    [SerializeField] public GameObject players; 

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCard(Card card)
    {
        HandleCardEffect(card.cardEffect);
        UpdateUI();
    }

    public void UpdateUI()
    {
        FieldOfPlayManager fieldScript = fieldOfPlay.GetComponent<FieldOfPlayManager>();
        if (fieldScript != null)
        {
            fieldScript.UpdateFromGameState(runnerOnFirst, runnerOnSecond, runnerOnThird);
        }

        ScoreBugManager scoreBugScript = scoreBug.GetComponent<ScoreBugManager>();
        if (scoreBugScript != null)
        {
            scoreBugScript.UpdateFromGameState(topInning, inning, awayScore, homeScore, outs);
        }

        PlayersInit playersScript = players.GetComponent<PlayersInit>();
        if (playersScript != null)
        {
            playersScript.SetNextPlayer();
        }
    }

    public void HandleCardEffect(CARD_EFFECT effect)
    {
        switch (effect)
        {
            case CARD_EFFECT.BASE_HIT:
                HandleBaseHit();
                break;
            case CARD_EFFECT.GAP_DOUBLE:
                HandleGapDouble();
                break;
            case CARD_EFFECT.GROUND_OUT:
                HandleGroundOut();
                break;
            default:
                break;
        }
    }
    
    public void HandleBaseHit()
    {
        if (runnerOnThird)
        {
            runnerOnThird = false;
            ScoreRun();
        }

        if (runnerOnSecond)
        {
            runnerOnSecond = false;
            runnerOnThird = true;
        }

        if (runnerOnFirst)
        {
            runnerOnFirst = false;
            runnerOnSecond = true;
        }

        runnerOnFirst = true;
    }

    public void HandleGapDouble()
    {
        if (runnerOnThird)
        {
            runnerOnThird = false;
            ScoreRun();
        }

        if (runnerOnSecond)
        {
            runnerOnSecond = false;
            ScoreRun();
        }

        if (runnerOnFirst)
        {
            runnerOnFirst = false;
            runnerOnThird = true;
        }

        runnerOnSecond = true;
    }

    public void HandleGroundOut()
    {
        if (outs >= 2)
        {
            TurnOverInning();
        }
        else
        {
            // Bases loaded
            if (runnerOnThird && runnerOnSecond && runnerOnFirst)
            {
                // double play
                if (outs >= 1)
                {
                    ClearBases();
                }
                else
                {
                    // Double play, only remove runner on first
                    runnerOnFirst = false;
                    outs = 2;
                }
            }
            // Second and 3rd
            else if (runnerOnThird && runnerOnSecond)
            {
                runnerOnSecond = false;
                outs++;
                ScoreRun();
            }
            // 1st and 3rd
            else if (runnerOnThird && runnerOnFirst)
            {
                if (outs >= 1)
                {
                    TurnOverInning();
                }
                else
                {
                    outs = 2;
                    ScoreRun();
                }
            }
            // 3rd only
            else if (runnerOnThird)
            {
                runnerOnThird = false;
                outs++;
                ScoreRun();
            }
            // 2nd and 1st
            else if (runnerOnSecond && runnerOnFirst)
            {
                if (outs >= 1)
                {
                    TurnOverInning();
                }
                else
                {
                    runnerOnFirst = false;
                    runnerOnSecond = false;
                    runnerOnThird = true;
                    outs += 2;
                }
            }
            // 2nd only
            else if (runnerOnSecond)
            {
                runnerOnSecond = false;
                runnerOnThird = true;
                outs++;
            }
            // 1st only
            else if (runnerOnFirst)
            {
                runnerOnFirst = false;
                outs += 2;
            }
            // bases empty
            else
            {
                outs++;
            }
        }
    }

    public void ScoreRun()
    {
        if (topInning)
        {
            awayScore++;
        }
        else
        {
            homeScore++;
        }
    }

    private void ClearBases()
    {
        runnerOnFirst = false;
        runnerOnSecond = false;
        runnerOnThird = false;
    }

    private void TurnOverInning()
    {
        runnerOnFirst = false;
        runnerOnSecond = false;
        runnerOnThird = false;

        outs = 0;
        topInning = !topInning;
        if(topInning) inning++;
    }
}
