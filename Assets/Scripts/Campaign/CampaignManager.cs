using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;

    // The deck currently being used by this campaign
    [SerializeField] private DeckObj deckDefault;
    private DeckObj deck;

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
    /// Called when loading save game data
    /// </summary>
    public void loadDeck(DeckSaveState _deck)
    {
        // Copy the data from DeckSaveState into the campaign deck
        deckDefault = _deck.copyToDeck();
        deck = _deck.copyToDeck();
    }
}
