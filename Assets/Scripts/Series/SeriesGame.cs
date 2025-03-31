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

    public bool gameCompleted = false;

    public SeriesGame(SeriesObj fromSeries, int gameNumber)
    {
        // Set the game number
        gameInSeries = gameNumber;

        // Set the home/away teams for this game, based on the series data
        // 3 games = WC series - home/away same each game
        if (fromSeries.totalGamesInSeries == 3)
        {
            if (fromSeries.playerIsHomeTeamInSeries)
            {
                homeTeam = "Player"; // TODO: use campaign manager's player team name
                awayTeam = fromSeries.opponentName;
            }
            else
            {
                homeTeam = fromSeries.opponentName;
                awayTeam = "Player";    // TODO
            }
        }
        else if (fromSeries.totalGamesInSeries == 5)
        {
            // 5 Games = Divisional Series - 2/2/1 split on home/away
            if (fromSeries.playerIsHomeTeamInSeries)
            {
                if (gameNumber == 1 || gameNumber == 2 || gameNumber == 5)
                {
                    homeTeam = "Player"; // TODO: use campaign manager's player team name
                    awayTeam = fromSeries.opponentName;
                }
                else
                {
                    homeTeam = fromSeries.opponentName;
                    awayTeam = "Player";    // TODO
                }
            }
            else
            {
                if (gameNumber == 1 || gameNumber == 2 || gameNumber == 5)
                {
                    homeTeam = fromSeries.opponentName; // TODO: use campaign manager's player team name
                    awayTeam = "Player";
                }
                else
                {
                    homeTeam = "Player";
                    awayTeam = fromSeries.opponentName;    // TODO
                }
            }
        }
        else if (fromSeries.totalGamesInSeries == 7)
        {
            // 7 Games = CS or WS - 2/4/2 Split on home/away
            if (fromSeries.playerIsHomeTeamInSeries)
            {
                if (gameNumber == 1 || gameNumber == 2 || gameNumber == 6 || gameNumber == 7)
                {
                    homeTeam = "Player"; // TODO: use campaign manager's player team name
                    awayTeam = fromSeries.opponentName;
                }
                else
                {
                    homeTeam = fromSeries.opponentName;
                    awayTeam = "Player";    // TODO
                }
            }
            else
            {
                if (gameNumber == 1 || gameNumber == 2 || gameNumber == 6 || gameNumber == 7)
                {
                    homeTeam = fromSeries.opponentName; // TODO: use campaign manager's player team name
                    awayTeam = "Player";
                }
                else
                {
                    homeTeam = "Player";
                    awayTeam = fromSeries.opponentName;    // TODO
                }
            }
        }
    }

    public void updateFromCompletedGame(GameStateStruct completedGame)
    {
        awayScore = completedGame.awayScore;
        homeScore = completedGame.homeScore;

        // Complete the Game
        gameCompleted = true;
    }
}
