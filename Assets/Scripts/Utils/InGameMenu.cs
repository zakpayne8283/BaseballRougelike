using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    private GameObject uiActionsManager;
    private UIActions uiActionsScript;

    [Header("Sub-Menu References")]
    [SerializeField] public GameObject saveGameMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Setup the menu
    /// </summary>
    public void Initialize(GameObject uiManager)
    {
        uiActionsManager = uiManager;
        uiActionsScript = uiActionsManager.GetComponent<UIActions>();
    }

    /// <summary>
    /// Close the menu when "Resume" is clicked
    /// </summary>
    public void onResumeClick()
    {
        uiActionsScript.toggleInGameMenu();
    }

    /// <summary>
    /// Toggles the save menu
    /// </summary>
    public void toggleSaveMenu()
    {
        // Check for active state
        if (saveGameMenu.activeInHierarchy)
        {
            // If active, disable
            saveGameMenu.SetActive(false);
        }
        else
        {
            // Set active
            saveGameMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Called when "Save Game" is clicked from the menu
    /// </summary>
    public void onSaveGameClick(GameObject inputField)
    {
        string fileName = inputField.GetComponent<TMPro.TMP_InputField>().text;

        CampaignSave.Instance.SaveGame(fileName);

        toggleSaveMenu();
    }
}
