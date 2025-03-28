using UnityEngine;

/// <summary>
/// Stores the games played in a series
/// </summary>
[System.Serializable]
public class SeriesGame
{
    public string awayTeam;
    public string homeTeam;

    public int awayScore;
    public int homeScore;

    public int gameInSeries;
}
