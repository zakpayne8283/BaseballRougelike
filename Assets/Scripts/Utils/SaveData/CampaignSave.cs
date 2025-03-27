using System.IO;
using UnityEngine;

// Object to be serialized in JSON for save data.
[System.Serializable]
public class CampaignSaveData
{
    public DeckSaveState deckSave;         // a DeckObj serialized as a string
    public SeriesSaveState[] seriesSaves;
}

public class CampaignSave : MonoBehaviour
{
    // Persistent instance
    public static CampaignSave Instance; 

    // File path for save files
    private string saveFilePath;

    void Awake()
    {
        // Only set one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    void Start()
    {
        // Set the path for the save file
        // TODO: seperate save files
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    /// <summary>
    /// Saves the game to local JSON file
    /// </summary>
    public void SaveGame()
    {
        // Get the save state setup
        CampaignSaveData saveData = new CampaignSaveData
        {
            deckSave = CampaignManager.Instance.copyDeck().getDeckSaveState(),
            seriesSaves = CampaignManager.Instance.copySeriesSaveState()
        };

        // Serialize to JSON
        string json = JsonUtility.ToJson(saveData, false);
        // Write the file
        File.WriteAllText(saveFilePath, json);
    }

    /// <summary>
    /// Loads the game from local JSON file
    /// </summary>
    public void LoadGame()
    {
        // File is found
        if (File.Exists(saveFilePath))
        {
            // Get the JSON data as a string
            string json = File.ReadAllText(saveFilePath);

            // Deserialize it to a new CampaignSaveData object
            CampaignSaveData data = JsonUtility.FromJson<CampaignSaveData>(json);

            // Deserialize the deck into the game state
            CampaignManager.Instance.loadDeck(data.deckSave);

            // Deserialize the list of series played into the game stae
            CampaignManager.Instance.loadSeriesData(data.seriesSaves);
        }
        else
        {
            Debug.Log("No save file found!");
        }
    }
}
