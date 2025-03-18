using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    public string PlayerName;
    [SerializeField]
    public PLAYER_TYPE PlayerType;
    [SerializeField]
    public Image BackgroundImageRef;

    [SerializeField]
    public Color defaultColor;

    private Color activeColor = Color.red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultColor.a = 1.0f;

        // Only set to default color if the prefab is not the active player
        if (BackgroundImageRef.color != activeColor)
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
        BackgroundImageRef.color = activeColor;
    }

    public void UnsetCurrent()
    {
        SetBackgroundToDefaultColor();     
    }

    private void SetBackgroundToDefaultColor()
    {
        BackgroundImageRef.color = defaultColor;
    }
}


public enum PLAYER_TYPE
{
    NONE,
    CONTACT,
    POWER,
    SPEED
}
