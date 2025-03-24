using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefab : MonoBehaviour
{
    [SerializeField]
    public string PlayerName;
    [SerializeField]
    public PLAYER_TYPE PlayerType;
    
    public Image backgroundImage;

    [SerializeField]
    public Color defaultColor;

    private Color activeColor = Color.red;

    public bool currentPlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the background image to the attached image component
        backgroundImage = this.GetComponent<Image>();

        // Force 0 transparency
        defaultColor.a = 1.0f;

        // Set the background color based on if this player is active or not.
        if (currentPlayer)
        {
            SetCurrent();
        }
        else
        {
            SetBackgroundToDefaultColor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrent()
    {
        currentPlayer = true;

        if (backgroundImage != null)
            backgroundImage.color = activeColor;
    }

    public void UnsetCurrent()
    {
        currentPlayer = false;
        SetBackgroundToDefaultColor();     
    }

    private void SetBackgroundToDefaultColor()
    {
        if (backgroundImage != null)
            backgroundImage.color = defaultColor;
    }

    public void setPlayerNameText(string textToSetTo)
    {
        // Update ]the prefab text
        this.transform.Find("Player Name").GetComponent<TMP_Text>().text = textToSetTo;
    }

    public void setPlayerType(PLAYER_TYPE typeToSetTo)
    {
        this.PlayerType = typeToSetTo;
    }
}

// Keep "NONE" at 0 index -> card modifications are dependent
public enum PLAYER_TYPE
{
    NONE,
    CONTACT,
    POWER,
    SPEED
}
