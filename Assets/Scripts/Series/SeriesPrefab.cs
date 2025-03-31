using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeriesPrefab : MonoBehaviour
{
    // The series this prefab is created from
    private SeriesObj series;

    // The game tile prefab used to create the game tiles
    [SerializeField] public GameObject gameTilePrefab;

    private bool homeTeam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(SeriesObj fromSeries)
    {
        // Set which series this is based on
        series = fromSeries;

        // If the series.seriesGames is null or empty, populate with empty
        if (series.seriesGames == null || series.seriesGames.Length == 0)
        {
            series.seriesGames = new SeriesGame[series.totalGamesInSeries];

            for (int i = 0; i < series.totalGamesInSeries; i++)
            {
                series.seriesGames[i] = new SeriesGame(fromSeries, i+1);
            }
        }

        // Get the series description
        TMP_Text seriesDescription = this.transform.Find("Series Description").GetComponent<TMP_Text>();

        // Update the text
        seriesDescription.text = createSeriesDescription();

        setupGameTiles();
    }

    private string createSeriesDescription()
    {
        string output = "";

        switch (series.seriesLevel)
        {
            case (SERIES_LEVEL.WILD_CARD_SERIES):
                output += "Wild Card Series";
                break;
            default:
                break;
        }

        output += $" vs. {series.opponentName}";

        return output;
    }

    private void setupGameTiles()
    {
        Transform tileArea = transform.Find("Game Tile Area");

        // First setup the grid layout
        GridLayoutGroup gridArea = tileArea.GetComponent<GridLayoutGroup>();
        // - put all required games on top (half total games + 1), optional on bottom
        gridArea.constraintCount = (series.totalGamesInSeries / 2) + 1;
        // - set the width to actually look good. fill space of parent
        gridArea.cellSize = calculateCellSize(tileArea, gridArea);

        bool isFirstUnplayedGame = true;

        for (int i = 0; i < series.totalGamesInSeries; i++)
        {
            // Add a new tile to the tile area
            GameObject addedTile = Instantiate(gameTilePrefab, tileArea);

            // Initialize it
            addedTile.GetComponent<GameTilePrefab>().Initialize(homeTeam, i+1);

            // If this is the first unplayed game of the series, highlight it
            if (isFirstUnplayedGame && !series.seriesGames[i].gameCompleted)
            {
                addedTile.GetComponent<GameTilePrefab>().markAsCurrent();

                // so subsequent games don't highlight
                isFirstUnplayedGame = false;
            }
        }
    }

    private Vector2 calculateCellSize(Transform tileArea, GridLayoutGroup gridArea)
    {
        Vector2 output = new Vector2();

        // Get the parent recttransform
        RectTransform parentRect = tileArea.GetComponent<RectTransform>();

        // Set cell x size to parent width, divided into sizes of
        // - total games in series / 2 plus one
        output.x = parentRect.rect.size.x / ((series.totalGamesInSeries / 2) + 1);

        // Adjust for grid spacing
        output.x = output.x - (gridArea.spacing.x * ((series.totalGamesInSeries / 2) + 2)); // +2 because spacing on both sides

        // Set cell y size to parent height divided by 2 (at most two rows of games)
        output.y = parentRect.rect.size.y / 2;

        output.y = output.y - (gridArea.spacing.y * 3);

        return output;
    }
}
