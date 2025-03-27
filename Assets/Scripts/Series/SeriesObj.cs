using UnityEngine;

[CreateAssetMenu(fileName = "SeriesObj", menuName = "Scriptable Objects/SeriesObj")]
public class SeriesObj : ScriptableObject
{
    [SerializeField] public SERIES_LEVEL seriesLevel;    // Also used for determining difficulty

    public int totalGamesInSeries;
    public int currentGameInSeries;

    public string opponentName;

    public bool playerIsHomeTeamInSeries;

    public SeriesObj(SeriesObj copyFrom)
    {
        seriesLevel = copyFrom.seriesLevel;
        totalGamesInSeries = copyFrom.totalGamesInSeries;
        currentGameInSeries = copyFrom.currentGameInSeries;
        opponentName = copyFrom.opponentName;
        playerIsHomeTeamInSeries = copyFrom.playerIsHomeTeamInSeries;
    }

    public SeriesObj(SeriesSaveState saveState)
    {
        seriesLevel = saveState.seriesLevel;
        totalGamesInSeries = saveState.totalGamesInSeries;
        currentGameInSeries = saveState.currentGameInSeries;
        opponentName = saveState.opponentName;
        playerIsHomeTeamInSeries = saveState.playerIsHomeTeamInSeries;
    }

    public SeriesObj copySeries()
    {
        return new SeriesObj(this);
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
    }

    public SeriesObj copyToSeries()
    {
        return new SeriesObj(this);
    }
}