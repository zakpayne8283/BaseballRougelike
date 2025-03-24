using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager Instance;

    [SerializeField] private DeckObj deckDefault;
    private DeckObj deck;

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
}
