using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    private GameObject uiActionsManager;
    private UIActions uiActionsScript;

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
}
