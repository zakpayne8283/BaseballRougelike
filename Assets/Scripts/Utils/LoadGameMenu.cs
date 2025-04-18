using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameMenu : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] public GameObject savedGameObject;

    [Header("Transforms")]
    [SerializeField] public Transform scrollViewTransform;

    private TMP_Text selectedFileText;
    private string selectedFileName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedFileText = gameObject.transform.Find("Selected File Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When the load game menu is enabled, display the saved games as options
    /// </summary>
    void OnEnable()
    {
        string saveFolder = CampaignSave.Instance.saveFileFolder;

        // If the folder exists
        if (Directory.Exists(saveFolder))
        {
            // Get the list of files
            string[] saveFiles = Directory.GetFiles(saveFolder, "*.json");

            // For each file, create an entry in the menu
            foreach (string saveFile in saveFiles)
            {
                // Get the file name
                string fileName = Path.GetFileNameWithoutExtension(saveFile);

                // Create the entry
                GameObject saveObject = Instantiate(savedGameObject, scrollViewTransform);

                // Set the entry name
                saveObject.GetComponentInChildren<TMP_Text>().text = fileName;

                // Assign an onclick event so that when it's clicked, the file is selected
                // and returned to this object
                saveObject.GetComponent<Button>().onClick.AddListener(() => selectFile(fileName));
            }
        }
    }

    /// <summary>
    /// Called when the save file is selected in the scroll area
    /// </summary>
    /// <param name="fileName"></param>
    public void selectFile(string fileName)
    {
        // Set the file to load to be this file
        selectedFileText.text = fileName;
        selectedFileName = fileName;
    }

    /// <summary>
    /// Called when "Load Game" is clicked and a file is selected
    /// </summary>
    public void loadGameOnClick()
    {
        if (selectedFileName != null && selectedFileName != "")
        {
            CampaignSave.Instance.LoadGame(selectedFileName);
        }
    }
}
