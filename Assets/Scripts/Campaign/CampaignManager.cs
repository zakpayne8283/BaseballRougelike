using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;

    // Holds the actual campaign data
    public Campaign campaignData;

    // The deck currently being used by this campaign
    [SerializeField] private DeckObj deckDefault;

    // Where the campaign starts
    [SerializeField] public SeriesObj startingSeries;

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
        campaignData.deck = deckDefault.copyObject();

        // If there are currently no series, initialize
        // - should not be called when loading data, which should populate
        //   all previous series also  
        if (campaignData.series == null || campaignData.series.Length == 0)
        {
            campaignData.series = new SeriesObj[1];
            campaignData.series[0] = startingSeries;
        }
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

    /// <summary>
    /// Called from GameState.cs game manager to end a game
    /// </summary>
    /// <param name="gameEndState"></param>
    public void endGame(GameStateStruct gameEndState)
    {
        // Note the game just ended
        gameEnded = true;

        // Record the results in the stored series data
        SeriesObj currentSeries = campaignData.series[campaignData.series.Length - 1];
        currentSeries.addCompletedGame(gameEndState);

        // Award an upgrade point if applicable
        if (currentSeries.playerWonLastGame())
        {
            campaignData.upgradesAvailable++;
        }

        SceneManager.LoadScene("CampaignScene");
    }
}
