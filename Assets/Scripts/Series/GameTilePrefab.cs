using TMPro;
using UnityEngine;

public class GameTilePrefab : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(bool homeTeam, int gameNumber)
    {
        // Set home/away text
        TMP_Text homeOrAwayText = this.transform.Find("Home or Away").GetComponent<TMP_Text>();
        homeOrAwayText.text = homeTeam ? "Home" : "Away"; 

        // Set game number text
        TMP_Text gameNumText = this.transform.Find("Game Number").GetComponent<TMP_Text>();
        gameNumText.text = $"Game #{gameNumber}";
    }
}
