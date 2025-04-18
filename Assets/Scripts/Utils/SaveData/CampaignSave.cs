using System.IO;
using UnityEngine;

public class CampaignSave : MonoBehaviour
{
    // Persistent instance
    public static CampaignSave Instance; 

    // File path for save files
    private string saveFileName;
    public string saveFileFolder;

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
        // Set the path for the save folder
        saveFileFolder = Path.Combine(Application.persistentDataPath, "savedata");

        // Create the save folder if it doesn't exist
        if (!Directory.Exists(saveFileFolder))
        {
            Directory.CreateDirectory(saveFileFolder);
        }

        // Default save file name
        saveFileName = "savedata.json";
    }

    /// <summary>
    /// Saves the game to local JSON file
    /// </summary>
    public void SaveGame()
    {
        SaveGame(saveFileName);
    }

    /// <summary>
    /// Saves the game with provided file path
    /// </summary>
    /// <param name="fileName"></param>
    public void SaveGame(string fileName)
    {
        // Make sure filePath is a JSON
        if (!fileName.Contains(".json"))
        {
            // TODO better sanitization here
            fileName += ".json";
        }

        string filePath = Path.Combine(saveFileFolder, fileName);

        // Get the save state setup from campaign data
        CampaignSaveData saveData = new CampaignSaveData(CampaignManager.Instance.campaignData);

        // Serialize to JSON
        string json = JsonUtility.ToJson(saveData, false);

        // Write the file
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Loads the game from local JSON file
    /// </summary>
    public void LoadGame(string fileName=null)
    {
        // Set to the default save file name if one isn't provided
        if (fileName == null)
        {
            fileName = saveFileName;
        }

        // Sanitize the file name
        fileName = Path.Combine(saveFileFolder, fileName);

        if (!fileName.Contains(".json"))
        {
            fileName += ".json";
        }

        // File is found
        if (File.Exists(fileName))
        {
            // Get the JSON data as a string
            string json = File.ReadAllText(fileName);

            // Deserialize it to a new CampaignSaveData object
            CampaignSaveData data = JsonUtility.FromJson<CampaignSaveData>(json);

            // Setup campaign data using the save data
            CampaignManager.Instance.campaignData = new Campaign(data);
        }
        else
        {
            Debug.Log("No save file found!");
        }
    }
}
