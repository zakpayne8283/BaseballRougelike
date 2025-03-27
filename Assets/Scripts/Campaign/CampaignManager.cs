using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;

    // The deck currently being used by this campaign
    [SerializeField] private DeckObj deckDefault;
    private DeckObj deck;

    // All of the series played on the current campaign
    private SeriesObj[] series;

    // Flag to tell if we're just leaving a game, to act on post game actions.
    public bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            // TODO: Destroy self on load of a different campaign
            Destroy(gameObject); // Prevent duplicate instances
        }

        // Copy the default deck to prevent a scriptable object from being overwritten
        deck = deckDefault.copyObject();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If exiting a game, do this
        if (gameEnded)
        {
            // TODO
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCampaignProgress()
    {
        // Save data will go here
    }

    /// <summary>
    /// Copies the deck for the overall campaign. Game Instances will copy this so no changes can happen in game.
    /// </summary>
    /// <returns></returns>
    public DeckObj copyDeck()
    {
        return deck.copyObject();
    }

    /// <summary>
    /// Returns the current deck
    /// </summary>
    /// <returns></returns>
    public DeckObj getDeck()
    {
        return deck;
    }

    /// <summary>
    /// Called when loading save game data
    /// </summary>
    public void loadDeck(DeckSaveState _deck)
    {
        // Copy the data from DeckSaveState into the campaign deck
        deckDefault = _deck.copyToDeck();
        deck = _deck.copyToDeck();
    }

    /// <summary>
    /// Copies ALL series to a new array
    /// </summary>
    /// <returns></returns>
    public SeriesObj[] copySeries()
    {
        SeriesObj[] output = new SeriesObj[series.Length];

        for (int i = 0; i < series.Length; i++)
        {
            output[i] = series[i];
        }

        return output;
    }

    /// <summary>
    /// Copy the current series to a save state format
    /// </summary>
    /// <returns></returns>
    public SeriesSaveState[] copySeriesSaveState()
    {
        SeriesSaveState[] output = new SeriesSaveState[series.Length];

        for (int i = 0; i < series.Length; i++)
        {
            output[i] = new SeriesSaveState(series[i]);
        }

        return output;
    }

    /// <summary>
    /// Load the series data from the save file
    /// </summary>
    /// <param name="_series"></param>
    public void loadSeriesData(SeriesSaveState[] _series)
    {
        series = new SeriesObj[_series.Length];

        for (int i = 0; i < _series.Length; i++)
        {
            series[i] = _series[i].copyToSeries();
        }
    }
}
