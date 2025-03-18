using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBugManager : MonoBehaviour
{
    [SerializeField] private Transform scoreBug;

    private bool topInning = true;
    private int inning = 1;

    private int awayScore = 0;
    private int homeScore = 0;

    private int outs = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateInning();
        UpdateScores();
        UpdateOuts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInning()
    {
        string outputText = topInning ? $"Top {inning}" : $"Bottom {inning}";
        scoreBug.transform.Find("Inning").GetComponent<TMP_Text>().text = outputText;
    }

    public void UpdateScores()
    {
        string awayScoreText = $"Away {awayScore}";
        string homeScoreText = $"Home {homeScore}";

        scoreBug.transform.Find("Away Score").GetComponent<TMP_Text>().text = awayScoreText;
        scoreBug.transform.Find("Home Score").GetComponent<TMP_Text>().text = homeScoreText;
    }

    public void UpdateOuts()
    {
        string outputText = $"{outs} Outs";
        scoreBug.transform.Find("Outs").GetComponent<TMP_Text>().text = outputText;
    }
}
