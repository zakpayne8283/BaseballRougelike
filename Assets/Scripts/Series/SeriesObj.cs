using UnityEngine;

[CreateAssetMenu(fileName = "SeriesObj", menuName = "Scriptable Objects/SeriesObj")]
public class SeriesObj : ScriptableObject
{
    [SerializeField] public SERIES_LEVEL seriesLevel;    // Also used for determining difficulty

    public int totalGamesInSeries;
    public int currentGameInSeries;

    public string opponentName;

    public bool playerIsHomeTeamInSeries;

    [HideInInspector] public SeriesGame[] seriesGames;

    public SeriesObj(SeriesObj copyFrom)
    {
        seriesLevel = copyFrom.seriesLevel;
        totalGamesInSeries = copyFrom.totalGamesInSeries;
        currentGameInSeries = copyFrom.currentGameInSeries;
        opponentName = copyFrom.opponentName;
        playerIsHomeTeamInSeries = copyFrom.playerIsHomeTeamInSeries;
        seriesGames = copyFrom.seriesGames;
    }

    public SeriesObj(SeriesSaveState saveState)
    {
        seriesLevel = saveState.seriesLevel;
        totalGamesInSeries = saveState.totalGamesInSeries;
        currentGameInSeries = saveState.currentGameInSeries;
        opponentName = saveState.opponentName;
        playerIsHomeTeamInSeries = saveState.playerIsHomeTeamInSeries;
        seriesGames = saveState.seriesGames;
    }

    public SeriesObj copySeries()
    {
        return new SeriesObj(this);
    }

    public void addCompletedGame(GameStateStruct gameEndState)
    {
        for (int i = 0; i < seriesGames.Length; i++)
        {
            if (seriesGames[i].gameCompleted)
            {
                continue;
            }
            else
            {
                // Update with the results
                seriesGames[i].updateFromCompletedGame(gameEndState);

                // Update the only the first incomplete game
                break;
            }
        }
    }

    public bool playerWonLastGame()
    {
        int gameIndex = -1;

        for (int i = 0; i < seriesGames.Length; i++)
        {
            // If last game in series
            if (i == seriesGames.Length - 1)
            {
                gameIndex = i;
            }
            else
            {
                // Otherwise, check if the next game hasn't been played
                if (seriesGames[i+1].gameCompleted)
                {
                    // If it has been, go to the next one
                    continue;
                }
                else
                {
                    gameIndex = i;
                }
            }
        }
    
        
        if (playerIsHomeTeamInSeries)
            return seriesGames[gameIndex].homeScore > seriesGames[gameIndex].awayScore;
        else
            return seriesGames[gameIndex].awayScore > seriesGames[gameIndex].homeScore;
    }
}

public enum SERIES_LEVEL
{
    WILD_CARD_SERIES,
    DIVISIONAL_SERIES,
    CHAMPIONSHIP_SERIES,
    WORLD_SERIES
}

/// <summary>
/// Used for saving the 
/// </summary>
[System.Serializable]
public class SeriesSaveState
{
    public SERIES_LEVEL seriesLevel;    // Also used for determining difficulty

    public int totalGamesInSeries;
    public int currentGameInSeries;

    public string opponentName;

    public bool playerIsHomeTeamInSeries;
    
    public SeriesGame[] seriesGames;

    /// <summary>
    /// Copy a SeriesObj to a save state
    /// </summary>
    /// <param name="fromSeries"></param>
    public SeriesSaveState(SeriesObj fromSeries)
    {
        seriesLevel = fromSeries.seriesLevel;
        totalGamesInSeries = fromSeries.totalGamesInSeries;
        currentGameInSeries = fromSeries.currentGameInSeries;
        opponentName = fromSeries.opponentName;
        playerIsHomeTeamInSeries = fromSeries.playerIsHomeTeamInSeries;
        seriesGames = fromSeries.seriesGames;
    }

    public SeriesObj copyToSeries()
    {
        return new SeriesObj(this);
    }
}