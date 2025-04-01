using UnityEngine;

/// <summary>
/// Behavior used to open the in-game menu
/// Attached to campaign manager gameobject currently
/// </summary>
public class UIActions : MonoBehaviour
{
    [Header("Prefab Bases")]
    [SerializeField] public GameObject inGameMenuPrefab;
    private GameObject inGameMenu;

    [Header("Transforms")]
    [SerializeField] public Transform menuParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Call the function you want to trigger
            toggleInGameMenu();
        }

    }

    public void toggleInGameMenu()
    {
        // If the escape menu currently exists, destroy it
        if (inGameMenu != null)
        {
            Destroy(inGameMenu);
        }
        else
        {
            inGameMenu = Instantiate(inGameMenuPrefab, menuParent);
            inGameMenu.GetComponent<InGameMenu>().Initialize(gameObject);
        }
    }
}
