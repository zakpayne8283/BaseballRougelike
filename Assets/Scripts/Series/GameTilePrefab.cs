using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// Handles needed logic for marking a game tile as the next game to be played
    /// </summary>
    public void markAsCurrent()
    {
        // Highlight this prefab to show it's the current game
        this.transform.Find("Background Image").GetComponent<Outline>().enabled = true;

        // Enablethe button component so that the game starts when it's clicked
        this.GetComponent<Button>().enabled = true;
    }
}
